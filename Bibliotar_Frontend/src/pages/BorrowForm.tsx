import { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Container, Title, Paper, NumberInput, Button, Group, Box, Text, LoadingOverlay, Stack } from '@mantine/core';
import { useForm } from '@mantine/form';
import api from '../api/api';
import { IconChevronLeft } from '@tabler/icons-react';

interface BorrowFormValues {
  userId: number;
  borrowPeriodInDays: number;
}

function BorrowForm() {
  const { bookId } = useParams<{ bookId: string }>();
  const navigate = useNavigate();
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const form = useForm<BorrowFormValues>({
    initialValues: {
      userId: 0,
      borrowPeriodInDays: 14,
    },
    validate: {
      userId: (value) => (value <= 0 ? 'Érvényes felhasználói azonosítót adjon meg' : null),
      borrowPeriodInDays: (value) => (value <= 0 ? 'A kölcsönzési időnek nagyobbnak kell lennie 0-nál' : null),
    },
  });

  const handleSubmit = async (values: BorrowFormValues) => {
    if (!bookId) return;
    
    setSubmitting(true);
    setError(null);
    
    try {
      await api.Borrow.create({
        userId: values.userId,
        bookId: parseInt(bookId),
        borrowPeriodInDays: values.borrowPeriodInDays
      });
      
      navigate('/app/BorrowManager');
    } catch (error) {
      console.error('Error creating borrow:', error);
      setError('Hiba történt a kölcsönzés létrehozása során. Kérjük, próbálja újra később.');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Container size="md" py="xl">
      <Button
        mb="xl"
        leftSection={<IconChevronLeft size="1rem" />}
        onClick={() => navigate('/app/BorrowCreate')}
        variant="subtle"
      >
        Vissza a könyvek kezeléséhez
      </Button>
      
      <Paper radius="md" p="xl" withBorder position="relative">
        <LoadingOverlay visible={submitting} overlayProps={{ blur: 2 }} />
        
        <form onSubmit={form.onSubmit(handleSubmit)}>
          <Stack>
            <Title order={2} align="center">Kölcsönzés létrehozása</Title>
            
            {error && (
              <Text c="red" ta="center" mb="md">{error}</Text>
            )}
            
            <NumberInput
              label="Felhasználó azonosítója"
              placeholder="Adja meg a felhasználó azonosítóját"
              required
              min={1}
              {...form.getInputProps('userId')}
            />
            
            <NumberInput
              label="Kölcsönzési idő (napokban)"
              placeholder="Kölcsönzési idő napokban"
              required
              min={1}
              max={90}
              {...form.getInputProps('borrowPeriodInDays')}
            />
            
            <Group position="right" mt="xl">
              <Button
                variant="outline"
                onClick={() => navigate('/app/BorrowCreate')}
              >
                Mégsem
              </Button>
              <Button 
                type="submit"
                loading={submitting}
              >
                Kölcsönzés létrehozása
              </Button>
            </Group>
          </Stack>
        </form>
      </Paper>
    </Container>
  );
}

export default BorrowForm;