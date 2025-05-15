import {
    Container,
    Title,
    Paper,
    Stack,
    Button,
    Text,
    Group,
    TextInput,
    Select,
    LoadingOverlay
} from "@mantine/core";
import { useState, useEffect, useRef } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useForm } from '@mantine/form';
import api from "../api/api";
import { type BookGetDto } from "../interfaces/BookInterfaces";
import { IconChevronLeft } from "@tabler/icons-react";
import axiosInstance from "../api/axios.config";

const BookEdit = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(true);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [successMessage, setSuccessMessage] = useState<string | null>(null);
    const [book, setBook] = useState<BookGetDto | null>(null);
    const dataFetched = useRef(false);

    const form = useForm({
        initialValues: {
            id: 0,
            title: '',
            author: '',
            quality: '',
            isbn: '',
            category: '',
            publishDate: new Date().toISOString().split('T')[0],
            status: 0 // Default to Available
        },
        validate: {
            title: (value) => (value.trim().length > 0 ? null : 'A cím megadása kötelező'),
            author: (value) => (value.trim().length > 0 ? null : 'A szerző megadása kötelező'),
            isbn: (value) => (/^[0-9-]{10,17}$/.test(value) ? null : 'Érvénytelen ISBN formátum'),
            category: (value) => (value.trim().length > 0 ? null : 'A kategória megadása kötelező'),
            quality: (value) => (value.trim().length > 0 ? null : 'A minőség megadása kötelező'),
            publishDate: (value) => (value ? null : 'A kiadás dátumának megadása kötelező')
        }
    });

    useEffect(() => {
        if (!dataFetched.current && id) {
            const fetchBookDetails = async () => {
                setIsLoading(true);
                setError(null);
                try {
                    const response = await api.Book.listBooks();
                    const books = response.data;
                    const bookId = parseInt(id);
                    const foundBook = books.find(b => b.id === bookId);
                    
                    if (foundBook) {
                        setBook(foundBook);
                        form.setValues({
                            id: foundBook.id || 0,
                            title: foundBook.title,
                            author: foundBook.author,
                            quality: foundBook.quality,
                            isbn: foundBook.isbn,
                            category: foundBook.category,
                            publishDate: new Date(foundBook.publishDate).toISOString().split('T')[0],
                            status: foundBook.status
                        });
                    } else {
                        setError("A könyv nem található.");
                        setTimeout(() => navigate('/app/BooksManager'), 2000);
                    }
                } catch (error) {
                    console.error("Error fetching book details:", error);
                    setError("Nem sikerült betölteni a könyv adatait. Kérjük, próbálja újra később.");
                } finally {
                    setIsLoading(false);
                    dataFetched.current = true;
                }
            };

            fetchBookDetails();
        }
    }, [id]);

    const handleSubmit = async (values: typeof form.values) => {
        setIsSubmitting(true);
        setError(null);
        setSuccessMessage(null);
        
        try {
            const bookData: BookGetDto = {
                ...values,
                publishDate: new Date(values.publishDate).toISOString(),
                status: Number(values.status)
            };
            
            console.log("Sending update request with data:", bookData);

            await axiosInstance.put('/api/Book/UpdateBook', bookData, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            
            setSuccessMessage("Könyv sikeresen módosítva!");

            setTimeout(() => {
                navigate('/app/BooksManager');
            }, 1500);
        } catch (error) {
            console.error("Error updating book:", error);
            setError("Nem sikerült frissíteni a könyvet. Kérjük, próbálja újra később.");
        } finally {
            setIsSubmitting(false);
        }
    };

    const qualityOptions = [
        { value: 'Új', label: 'Új' },
        { value: 'Jó', label: 'Jó' },
        { value: 'Használt', label: 'Használt' },
        { value: 'Elhasznált', label: 'Elhasznált' }
    ];

    const categoryOptions = [
        { value: 'Regény', label: 'Regény' },
        { value: 'Sci-Fi', label: 'Sci-Fi' },
        { value: 'Fantasy', label: 'Fantasy' },
        { value: 'Thriller', label: 'Thriller' },
        { value: 'Krimi', label: 'Krimi' },
        { value: 'Romantikus', label: 'Romantikus' },
        { value: 'Történelmi', label: 'Történelmi' },
        { value: 'Ismeretterjesztő', label: 'Ismeretterjesztő' }
    ];

    const statusOptions = [
        { value: '0', label: 'Elérhető' },
        { value: '1', label: 'Kikölcsönözve' },
        { value: '2', label: 'Foglalt' }
    ];

    return (
        <Container size="md" py="xl">
            <Button
                mb="xl"
                leftSection={<IconChevronLeft size="1rem" />}
                onClick={() => navigate('/app/BooksManager')}
                variant="subtle"
            >
                Vissza a könyvek kezeléséhez
            </Button>
            
            <Paper radius="md" p="xl" withBorder position="relative">
                <LoadingOverlay visible={isLoading || isSubmitting} overlayProps={{ blur: 2 }} />
                
                {(!isLoading || book) && (
                    <form onSubmit={form.onSubmit(handleSubmit)}>
                        <Stack>
                            <Title order={2} align="center">Könyv módosítása</Title>
                            
                            {error && (
                                <Text c="red" ta="center" mb="md">{error}</Text>
                            )}
                            
                            {successMessage && (
                                <Text c="green" ta="center" mb="md">{successMessage}</Text>
                            )}
                            
                            <TextInput
                                label="Cím"
                                placeholder="A könyv címe"
                                required
                                {...form.getInputProps('title')}
                            />
                            
                            <TextInput
                                label="Szerző"
                                placeholder="A könyv szerzője"
                                required
                                {...form.getInputProps('author')}
                            />
                            
                            <TextInput
                                label="ISBN"
                                placeholder="Pl. 978-3-16-148410-0"
                                required
                                {...form.getInputProps('isbn')}
                            />
                            
                            <Select
                                label="Kategória"
                                placeholder="Válasszon kategóriát"
                                data={categoryOptions}
                                required
                                searchable
                                creatable
                                getCreateLabel={(query) => `+ Új kategória: ${query}`}
                                onCreate={(query) => {
                                    const item = { value: query, label: query };
                                    categoryOptions.push(item);
                                    return item;
                                }}
                                {...form.getInputProps('category')}
                            />
                            
                            <Select
                                label="Minőség"
                                placeholder="Válasszon minőséget"
                                data={qualityOptions}
                                required
                                searchable
                                creatable
                                getCreateLabel={(query) => `+ Új minőség: ${query}`}
                                onCreate={(query) => {
                                    const item = { value: query, label: query };
                                    qualityOptions.push(item);
                                    return item;
                                }}
                                {...form.getInputProps('quality')}
                            />
                            
                            <TextInput
                                label="Kiadás dátuma"
                                placeholder="ÉÉÉÉ-HH-NN"
                                type="date"
                                required
                                {...form.getInputProps('publishDate')}
                            />
                            
                            <Select
                                label="Státusz"
                                placeholder="Válasszon státuszt"
                                data={statusOptions}
                                required
                                {...form.getInputProps('status')}
                            />
                            
                            <Group position="right" mt="xl">
                                <Button
                                    variant="outline"
                                    onClick={() => navigate('/app/BooksManager')}
                                >
                                    Mégsem
                                </Button>
                                <Button 
                                    type="submit" 
                                    loading={isSubmitting}
                                    color="blue"
                                >
                                    Változtatások mentése
                                </Button>
                            </Group>
                        </Stack>
                    </form>
                )}
            </Paper>
        </Container>
    );
};

export default BookEdit;