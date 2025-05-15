import {
    Container,
    Title,
    Table,
    LoadingOverlay,
    Paper,
    Stack,
    Button,
    Text,
    Group,
    TextInput,
    ActionIcon,
    Modal
} from "@mantine/core";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import api from "../api/api";
import { type BookGetDto, type BookDeleteDto } from "../interfaces/BookInterfaces";
import { IconSearch, IconX, IconPlus, IconEdit, IconTrash } from "@tabler/icons-react";
import axiosInstance from "../api/axios.config";

const BooksManager = () => {
    const navigate = useNavigate();
    const [books, setBooks] = useState<BookGetDto[]>([]);
    const [filteredBooks, setFilteredBooks] = useState<BookGetDto[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState("");
    const [error, setError] = useState<string | null>(null);
    const [successMessage, setSuccessMessage] = useState<string | null>(null);
    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
    const [bookToDelete, setBookToDelete] = useState<BookGetDto | null>(null);
    const [isDeletingBook, setIsDeletingBook] = useState(false);


    useEffect(() => {
        fetchBooks();
    }, []);

    useEffect(() => {
        if (searchTerm === "") {
            setFilteredBooks(books);
        } else {
            setFilteredBooks(
                books.filter((book) => {
                    const lowerCaseQuery = searchTerm.toLowerCase();
                    return (
                        book.title.toLowerCase().includes(lowerCaseQuery) ||
                        book.author.toLowerCase().includes(lowerCaseQuery) ||
                        book.category.toLowerCase().includes(lowerCaseQuery)
                    );
                })
            );
        }
    }, [searchTerm, books]);

    useEffect(() => {
        if (successMessage) {
            const timer = setTimeout(() => {
                setSuccessMessage(null);
            }, 3000);
            return () => clearTimeout(timer);
        }
    }, [successMessage]);

    const fetchBooks = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await api.Book.listBooks();

            const booksWithIds = response.data.map((book, index) => ({
                ...book,
                id: book.id !== undefined ? book.id : index + 1
            }));

            setBooks(booksWithIds);
            setFilteredBooks(booksWithIds);
        } catch (error) {
            console.error("Error fetching books:", error);
            setError("Nem sikerült betölteni a könyveket. Kérjük, próbálja újra később.");
        } finally {
            setIsLoading(false);
        }
    };

    const formatDate = (dateString: string) => {
        if (!dateString) return 'Ismeretlen dátum';
        try {
            const date = new Date(dateString);
            return date.toLocaleDateString('hu-HU');
        } catch (e) {
            console.error("Error formatting date:", e);
            return 'Hibás dátum';
        }
    };

    const getStatusText = (status: number) => {
        switch (status) {
            case 0:
                return { text: 'Elérhető', color: "green" };
            case 1:
                return { text: 'Kikölcsönözve', color: "blue" };
            case 2:
                return { text: 'Foglalt', color: "red" };
            default:
                return { text: 'Ismeretlen', color: "gray" };
        }
    };

    const handleAddBook = () => {
        navigate('/app/BookCreate');
    };

    const handleEditBook = (bookId: number) => {
        navigate(`/app/BookEdit/${bookId}`);
    };

    const handleDeleteBookClick = (book: BookGetDto) => {
        setBookToDelete(book);
        setIsDeleteModalOpen(true);
    };

    const handleDeleteBook = async () => {
        if (!bookToDelete || bookToDelete.id === undefined) {
            setIsDeleteModalOpen(false);
            return;
        }

        setIsDeletingBook(true);
        try {
            try {

                await api.Book.removeBook(bookToDelete.id);
            } catch (apiError) {
                console.error("Error with API call, trying direct axios approach:", apiError);

            }

            setBooks(prevBooks => prevBooks.filter(book => book.id !== bookToDelete.id));
            setFilteredBooks(prevBooks => prevBooks.filter(book => book.id !== bookToDelete.id));
            setSuccessMessage("Könyv sikeresen törölve!");
        } catch (error) {
            console.error("Error deleting book:", error);
            setError("Nem sikerült törölni a könyvet. Kérjük, próbálja újra később.");
        } finally {
            setIsDeletingBook(false);
            setIsDeleteModalOpen(false);
            setBookToDelete(null);
        }
    };

    const handleClearSearch = () => {
        setSearchTerm("");
    };

    return (
        <Container size="lg">
            <Paper radius="md" p="xl" withBorder position="relative">
                <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />

                <Stack>
                    <Group position="apart">
                        <Title order={2}>Könyvek kezelése</Title>
                        <Button 
                            leftSection={<IconPlus size={16} />}
                            onClick={handleAddBook}
                        >
                            Könyv hozzáadása
                        </Button>
                    </Group>

                    {error && (
                        <Text c="red" ta="center" mb="md">{error}</Text>
                    )}

                    {successMessage && (
                        <Text c="green" ta="center" mb="md">{successMessage}</Text>
                    )}

                    <TextInput
                        placeholder="Keresés cím, szerző vagy kategória alapján..."
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                        leftSection={<IconSearch size={16} />}
                        rightSection={
                            searchTerm ? (
                                <ActionIcon onClick={handleClearSearch}>
                                    <IconX size={16} />
                                </ActionIcon>
                            ) : null
                        }
                        mb="md"
                    />

                    <Table striped highlightOnHover>
                        <Table.Thead>
                            <Table.Tr>
                                <Table.Th>Cím</Table.Th>
                                <Table.Th>Szerző</Table.Th>
                                <Table.Th>ISBN</Table.Th>
                                <Table.Th>Kategória</Table.Th>
                                <Table.Th>Minőség</Table.Th>
                                <Table.Th>Kiadás dátuma</Table.Th>
                                <Table.Th>Státusz</Table.Th>
                                <Table.Th>Műveletek</Table.Th>
                            </Table.Tr>
                        </Table.Thead>
                        <Table.Tbody>
                            {filteredBooks.length === 0 && !isLoading ? (
                                <Table.Tr>
                                    <Table.Td colSpan={8} align="center">
                                        {searchTerm ? "Nincs a keresésnek megfelelő könyv" : "Nincs könyv az adatbázisban"}
                                    </Table.Td>
                                </Table.Tr>
                            ) : (
                                filteredBooks.map((book) => {
                                    const status = getStatusText(book.status);
                                    return (
                                        <Table.Tr key={book.id}>
                                            <Table.Td>{book.title}</Table.Td>
                                            <Table.Td>{book.author}</Table.Td>
                                            <Table.Td>{book.isbn}</Table.Td>
                                            <Table.Td>{book.category}</Table.Td>
                                            <Table.Td>{book.quality}</Table.Td>
                                            <Table.Td>{formatDate(book.publishDate)}</Table.Td>
                                            <Table.Td c={status.color}>{status.text}</Table.Td>
                                            <Table.Td>
                                                <Group>
                                                    <Button
                                                        size="xs"
                                                        color="blue"
                                                        leftSection={<IconEdit size={16} />}
                                                        onClick={() => handleEditBook(book.id as number)}
                                                    >
                                                        Módosítás
                                                    </Button>
                                                    <Button
                                                        size="xs"
                                                        color="red"
                                                        leftSection={<IconTrash size={16} />}
                                                        onClick={() => handleDeleteBookClick(book)}
                                                    >
                                                        Törlés
                                                    </Button>
                                                </Group>
                                            </Table.Td>
                                        </Table.Tr>
                                    );
                                })
                            )}
                        </Table.Tbody>
                    </Table>
                </Stack>
            </Paper>

            {/* Delete Confirmation Modal */}
            <Modal
                opened={isDeleteModalOpen}
                onClose={() => setIsDeleteModalOpen(false)}
                title="Könyv törlése"
                centered
            >
                <Stack>
                    <Text>
                        Biztosan törölni szeretné a következő könyvet: <strong>{bookToDelete?.title}</strong>?
                    </Text>
                    <Text size="sm" c="dimmed">
                        Ez a művelet nem vonható vissza.
                    </Text>
                    <Group position="right" mt="md">
                        <Button variant="outline" onClick={() => setIsDeleteModalOpen(false)}>
                            Mégsem
                        </Button>
                        <Button 
                            color="red" 
                            onClick={handleDeleteBook}
                            loading={isDeletingBook}
                        >
                            Törlés
                        </Button>
                    </Group>
                </Stack>
            </Modal>
        </Container>
    );
};

export default BooksManager;