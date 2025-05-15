import { useEffect, useState } from 'react';
import { Table, Button, Container, Title, Text, Paper, LoadingOverlay, TextInput, ActionIcon } from '@mantine/core';
import { useNavigate } from 'react-router-dom';
import { IconSearch, IconX } from '@tabler/icons-react';
import api from '../api/api';
import type {BookGetDto} from '../interfaces/BookInterfaces';

function BorrowCreate() {
  const [books, setBooks] = useState<BookGetDto[]>([]);
  const [filteredBooks, setFilteredBooks] = useState<BookGetDto[]>([]);
  const [searchQuery, setSearchQuery] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [borrowingBookId, setBorrowingBookId] = useState<number | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchBooks = async () => {
      try {
        const response = await api.Book.listBooks();
        setBooks(response.data);
        setFilteredBooks(response.data);
      } catch (error) {
        console.error('Error fetching books:', error);
        setError('Nem sikerült betölteni a könyveket. Kérjük, próbálja újra később.');
      } finally {
        setLoading(false);
      }
    };

    fetchBooks();
  }, []);

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
        book.category.toLowerCase().includes(lowerCaseQuery) ||
        book.isbn.toLowerCase().includes(lowerCaseQuery)
    );
    
    setFilteredBooks(filtered);
  };

  const handleClearSearch = () => {
    setSearchQuery("");
    setFilteredBooks(books);
  };

  const handleBorrowClick = (bookId: number) => {
    setBorrowingBookId(bookId);
    navigate(`/app/BorrowForm/${bookId}`);
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('hu-HU');
  };

  const getStatusText = (status: number) => {
    switch (status) {
      case 0:
        return { text: 'Elérhető', available: true, color: "green" };
      case 1:
        return { text: 'Kikölcsönözve', available: false, color: "blue" };
      case 2:
        return { text: 'Foglalt', available: false, color: "red" };
      default:
        return { text: 'Ismeretlen', available: false, color: "gray" };
    }
  };

  const isBookBorrowed = (status: number) => {
    return status === 1;
  };

  return (
    <Container size="lg">
      <Paper radius="md" p="xl" withBorder position="relative">
        <LoadingOverlay visible={loading} overlayProps={{ blur: 2 }} />
        
        <Title order={2} align="center" mb="md">Kölcsönözhető könyvek</Title>
        
        <TextInput
          placeholder="Keresés könyvcím, szerző, ISBN vagy kategória alapján..."
          leftSection={<IconSearch size={16} />}
          rightSection={
            searchQuery ? (
              <ActionIcon onClick={handleClearSearch}>
                <IconX size={16} />
              </ActionIcon>
            ) : null
          }
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
              <Table.Th>ISBN</Table.Th>
              <Table.Th>Kategória</Table.Th>
              <Table.Th>Állapot</Table.Th>
              <Table.Th>Kiadás dátuma</Table.Th>
              <Table.Th>Státusz</Table.Th>
              <Table.Th>Művelet</Table.Th>
            </Table.Tr>
          </Table.Thead>
          <Table.Tbody>
            {filteredBooks.length === 0 && !loading ? (
              <Table.Tr>
                <Table.Td colSpan={8} align="center">
                  {searchQuery ? "Nincs a keresésnek megfelelő könyv" : "Nincsenek könyvek az adatbázisban"}
                </Table.Td>
              </Table.Tr>
            ) : (
              filteredBooks.map((book) => {
                const status = getStatusText(book.status);
                const isCurrentlyBorrowing = borrowingBookId === book.id;
                const isBorrowed = isBookBorrowed(book.status);
                
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
                      <Button
                        size="xs"
                        onClick={() => handleBorrowClick(book.id)}
                        loading={isCurrentlyBorrowing}
                        disabled={isBorrowed || isCurrentlyBorrowing}
                      >
                        {isCurrentlyBorrowing ? '' : 'Kölcsönzés'}
                      </Button>
                    </Table.Td>
                  </Table.Tr>
                );
              })
            )}
          </Table.Tbody>
        </Table>
      </Paper>
    </Container>
  );
}

export default BorrowCreate;