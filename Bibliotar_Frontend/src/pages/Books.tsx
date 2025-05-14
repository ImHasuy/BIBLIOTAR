// src/pages/Books.tsx
import { 
  Container, 
  Title, 
  TextInput,
  Table,
  rem,
  LoadingOverlay,
  Paper,
  Stack,
  Button,
  Text
} from "@mantine/core";
import { IconSearch } from "@tabler/icons-react";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import api from "../api/api";
import type { BookGetDto } from "../interfaces/BookInterfaces";

const Books = () => {
  const navigate = useNavigate();
  const [books, setBooks] = useState<BookGetDto[]>([]);
  const [filteredBooks, setFilteredBooks] = useState<BookGetDto[]>([]);
  const [searchQuery, setSearchQuery] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [reservingBookId, setReservingBookId] = useState<number | null>(null);

  // Fetch books from API
  useEffect(() => {
    fetchBooks();
  }, []);

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

  // Handle reservation
  const handleReservation = async (bookId: number) => {
    setReservingBookId(bookId);
    try {
      await api.Reservation.create(bookId);
      
      // Update the book's status locally
      const updatedBooks = books.map(book => 
        book.id === bookId 
          ? { ...book, status: 2 } // Set to "Foglalt" status
          : book
      );
      
      setBooks(updatedBooks);
      setFilteredBooks(updatedBooks.filter(book => 
        book.title.toLowerCase().includes(searchQuery.toLowerCase()) || 
        book.author.toLowerCase().includes(searchQuery.toLowerCase()) ||
        book.category.toLowerCase().includes(searchQuery.toLowerCase())
      ));
      
    } catch (error) {
      console.error("Error reserving book:", error);
      setError("Nem sikerült lefoglalni a könyvet. Kérjük, próbálja újra később.");
    } finally {
      setReservingBookId(null);
    }
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
    <Container size="lg">
      <Paper radius="md" p="xl" withBorder position="relative">
        <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />
        
        <Stack>
          <Title order={2} align="center" mb="md">Könyvek</Title>
          
          <TextInput
            placeholder="Keresés könyvcím, szerző vagy kategória alapján..."
            icon={<IconSearch size={rem(18)} />}
            value={searchQuery}
            onChange={(e) => handleSearch(e.currentTarget.value)}
            mb="md"
          />
          
          {error && (
            <Text c="red" ta="center" mb="md">{error}</Text>
          )}
          
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
                  const isCurrentlyReserving = reservingBookId === book.id;
                  
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
                          loading={isCurrentlyReserving}
                          disabled={!status.available || isCurrentlyReserving}
                        >
                          {isCurrentlyReserving ? '' : 'Foglalás'}
                        </Button>
                      </Table.Td>
                    </Table.Tr>
                  );
                })
              )}
            </Table.Tbody>
          </Table>
        </Stack>
      </Paper>
    </Container>
  );
};

export default Books;