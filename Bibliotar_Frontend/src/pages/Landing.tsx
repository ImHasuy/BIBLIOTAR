// src/pages/Landing.tsx
import { 
  Container, 
  Title, 
  Text, 
  Button, 
  Group, 
  Stack, 
  Paper, 
  Center, 
  Image, 
  TextInput,
  Table,
  rem,
  LoadingOverlay
} from "@mantine/core";
import { IconSearch } from "@tabler/icons-react";
import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import api from "../api/api";
import type {BookGetDto} from "../interfaces/BookInterfaces";

// Rest of your component code remains the same
const Landing = () => {
  const navigate = useNavigate();
  const [books, setBooks] = useState<BookGetDto[]>([]);
  const [filteredBooks, setFilteredBooks] = useState<BookGetDto[]>([]);
  const [searchQuery, setSearchQuery] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Fetch books from API
  useEffect(() => {
    const fetchBooks = async () => {
      setIsLoading(true);
      setError(null);
      try {
        const response = await api.Book.listBooks();
        // Assuming the API returns books with IDs; if not, we'll add them
        const booksWithIds = response.data.map((book, index) => ({
          ...book,
          id: book.id || index + 1 // Use existing ID or create one
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

    fetchBooks();
  }, []);

  // Handle search
  const handleSearch = (query: string) => {
    setSearchQuery(query);
    
    if (!query.trim()) {
      setFilteredBooks(books);
      return;
    }
    
    const lowerCaseQuery = query.toLowerCase();
    const filtered = books.filter(
      book => 
        book.title.toLowerCase().includes(lowerCaseQuery) || 
        book.author.toLowerCase().includes(lowerCaseQuery) ||
        book.category.toLowerCase().includes(lowerCaseQuery)
    );
    
    setFilteredBooks(filtered);
  };

  // Handle reservation button click
  const handleReservation = (bookId: number) => {
    navigate(`/books/${bookId}`);
  };

  // Helper to convert status to readable format
  const getStatusText = (status: number) => {
    switch (status) {
      case 0:
        return { text: 'Elérhető', available: true };
      case 1:
        return { text: 'Kikölcsönözve', available: false };
      case 2:
        return { text: 'Foglalt', available: false };
      default:
        return { text: 'Ismeretlen', available: false };
    }
  };

  // Format date
  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('hu-HU');
  };

  return (
    <Container size="lg" py="xl">
      <Center mb={40}>
        <Image src="/logo.png" alt="logo" w={150} />
      </Center>
      
      <Paper radius="md" p="xl" withBorder mb={40}>
        <Stack>
          <Title order={1} align="center" justify="center" mt="xl">Bibliotár könyvtár</Title>
          <Text align="center" justify="center" size="lg" c="dimmed">
            A legjobb online könyvtár ami létezik.
          </Text>
          
          <Group align="center" justify="center" mt="xl">
            <Button size="md" radius="md" onClick={() => navigate('/login')}>
              Bejelentkezés
            </Button>
            <Button size="md" radius="md" variant="outline" align="center" justify="center" onClick={() => navigate('/register')}>
              Regisztráció
            </Button>
          </Group>
        </Stack>
      </Paper>
      
      <Paper radius="md" p="xl" withBorder position="relative">
        <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />
        
        <Stack>
          <Title order={2} align="center" mb="md">Könyvek keresése</Title>
          
          <TextInput
            placeholder="Keresés könyvcím, szerző vagy kategória alapján..."
            icon={<IconSearch size={rem(18)} />}
            value={searchQuery}
            onChange={(e) => handleSearch(e.currentTarget.value)}
            mb="md"
          />
          
          {error ? (
            <Text c="red" ta="center">{error}</Text>
          ) : (
            <Table striped highlightOnHover>
              <Table.Thead>
                <Table.Tr>
                  <Table.Th>Cím</Table.Th>
                  <Table.Th>Szerző</Table.Th>
                  <Table.Th>Kategória</Table.Th>
                  <Table.Th>Minőség</Table.Th>
                  <Table.Th>Kiadás éve</Table.Th>
                  <Table.Th>Státusz</Table.Th>
                  <Table.Th></Table.Th>
                </Table.Tr>
              </Table.Thead>
              <Table.Tbody>
                {filteredBooks.length === 0 && !isLoading ? (
                  <Table.Tr>
                    <Table.Td colSpan={7} align="center">Nem található könyv a keresési feltételekkel</Table.Td>
                  </Table.Tr>
                ) : (
                  filteredBooks.map((book) => {
                    const status = getStatusText(book.status);
                    return (
                      <Table.Tr key={book.id}>
                        <Table.Td>{book.title}</Table.Td>
                        <Table.Td>{book.author}</Table.Td>
                        <Table.Td>{book.category}</Table.Td>
                        <Table.Td>{book.quality}</Table.Td>
                        <Table.Td>{formatDate(book.publishDate)}</Table.Td>
                        <Table.Td c={status.available ? "green" : "red"}>{status.text}</Table.Td>
                        <Table.Td>
                          <Button
                            size="sm"
                            onClick={() => handleReservation(book.id as number)}
                            disabled={!status.available}
                          >
                            Foglalás
                          </Button>
                        </Table.Td>
                      </Table.Tr>
                    );
                  })
                )}
              </Table.Tbody>
            </Table>
          )}
        </Stack>
      </Paper>
    </Container>
  );
};

export default Landing;