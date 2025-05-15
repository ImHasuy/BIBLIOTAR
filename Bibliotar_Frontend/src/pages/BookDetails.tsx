import { 
  Container, 
  Title, 
  Text, 
  Button, 
  Group, 
  Stack, 
  Paper, 
  Center, 
  TextInput,
  Divider,
  Alert,
  LoadingOverlay
} from "@mantine/core";
import { IconAlertCircle, IconBook, IconChevronLeft } from "@tabler/icons-react";
import { useNavigate, useParams } from "react-router-dom";
import { useState, useEffect } from "react";
import { useForm } from "@mantine/form";
import api from "../api/api";
import type {BookGetDto} from "../interfaces/BookInterfaces";
import axiosInstance from "../api/axios.config";

const BookDetails = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [book, setBook] = useState<BookGetDto | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isReserving, setIsReserving] = useState(false);
  const [reservationSuccess, setReservationSuccess] = useState(false);

  const form = useForm({
    initialValues: {
      email: ''
    },
    validate: {
      email: (value) => (/^\S+@\S+$/.test(value) ? null : 'Érvénytelen email cím')
    }
  });

  useEffect(() => {
    const fetchBookDetails = async () => {
      setIsLoading(true);
      setError(null);
      try {
        const response = await api.Book.listBooks();
        const books = response.data.map((book, index) => ({
          ...book,
          id: book.id || index + 1
        }));
        
        const foundBook = books.find(b => b.id === Number(id));
        if (foundBook) {
          setBook(foundBook);
        } else {
          setError("A keresett könyv nem található.");
        }
      } catch (error) {
        console.error("Error fetching book details:", error);
        setError("Nem sikerült betölteni a könyv részleteit. Kérjük, próbálja újra később.");
      } finally {
        setIsLoading(false);
      }
    };

    if (id) {
      fetchBookDetails();
    }
  }, [id]);

const handleReservation = async (values: typeof form.values) => {
  setIsReserving(true);
  setError(null);
  
  try {
    // Create the DTO as specified
    const reservationData = {
      Email: values.email,
      BookId: Number(id)
    };

    const response = await axiosInstance.post('/api/Reservation/createAsGuest', reservationData);

    console.log('Reservation successful, ID:', response.data);
    setReservationSuccess(true);

    setTimeout(() => {
      navigate('/');
    }, 2000);
  } catch (err) {
    console.error('Error during reservation:', err);
    setError("A foglalás sikertelen. Kérjük, próbálja újra később.");
  } finally {
    setIsReserving(false);
  }
};


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

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('hu-HU');
  };

  return (
    <Container size="md" py="xl">
      <Button
        mb="xl"
        leftSection={<IconChevronLeft size="1rem" />}
        onClick={() => navigate('/')}
        variant="subtle"
      >
        Vissza a főoldalra
      </Button>
      
      <Paper radius="md" p="xl" withBorder mb="xl" position="relative">
        <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />
        
        {error ? (
          <Alert icon={<IconAlertCircle size="1rem" />} title="Hiba!" color="red">
            {error}
          </Alert>
        ) : book ? (
          <Stack>
            <Group position="apart" align="center">
              <IconBook size="2rem" />
              <Text fw={700} c="dimmed">ISBN: {book.isbn}</Text>
            </Group>
            
            <Title order={1}>{book.title}</Title>
            <Group>
              <Text fw={500}>{book.author}</Text>
              <Text c="dimmed">({formatDate(book.publishDate)})</Text>
            </Group>
            
            <Divider my="sm" />
            
            <Group>
              <Text fw={500}>Kategória:</Text>
              <Text>{book.category}</Text>
            </Group>
            
            <Group>
              <Text fw={500}>Minőség:</Text>
              <Text>{book.quality}</Text>
            </Group>
            
            <Text fw={700} c={getStatusText(book.status).available ? "green" : "red"}>
              {getStatusText(book.status).text}
            </Text>
          </Stack>
        ) : null}
      </Paper>
      
      {!isLoading && !error && book && getStatusText(book.status).available && !reservationSuccess ? (
        <Paper radius="md" p="xl" withBorder>
          <form onSubmit={form.onSubmit(handleReservation)}>
            <Stack>
              <Title order={2}>Könyv foglalása</Title>
              <Text size="sm" c="dimmed">
                Foglalja le a könyvet az alábbi űrlap kitöltésével. A foglalásról értesítést küldünk e-mailben.
              </Text>
              
              <TextInput
                label="E-mail cím"
                placeholder="pelda@email.hu"
                required
                {...form.getInputProps('email')}
              />
              
              <Button type="submit" loading={isReserving}>
                Foglalás
              </Button>
            </Stack>
          </form>
        </Paper>
      ) : reservationSuccess ? (
        <Alert icon={<IconAlertCircle size="1rem" />} title="Sikeres foglalás!" color="green">
          A könyvet sikeresen lefoglalta. A foglalás részleteit elküldtük az e-mail címére.
          <Button
            mt="md"
            onClick={() => navigate('/')}
            variant="outline"
            fullWidth
          >
            Vissza a főoldalra
          </Button>
        </Alert>
      ) : book && !getStatusText(book.status).available ? (
        <Alert icon={<IconAlertCircle size="1rem" />} title="Nem foglalható" color="orange">
          Ez a könyv jelenleg nem foglalható. Kérjük próbálja meg később.
        </Alert>
      ) : null}
    </Container>
  );
};

export default BookDetails;