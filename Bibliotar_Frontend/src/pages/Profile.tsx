import { useState } from 'react';
import { Box, Button, Container, TextInput, Title, Grid, Paper } from '@mantine/core';
import { useForm } from '@mantine/form';
import api from '../api/api';

interface UserProfileFormValues {
  phoneNumber: string;
  address: {
    zipCode: string;
    city: string;
    street: string;
    houseNumber: string;
    country: string;
  }
}

function Profile() {
  const [isSubmitting, setIsSubmitting] = useState(false);

  const form = useForm<UserProfileFormValues>({
    initialValues: {
      phoneNumber: '',
      address: {
        zipCode: '',
        city: '',
        street: '',
        houseNumber: '',
        country: ''
      }
    }
  });

  const handleSubmit = async (values: UserProfileFormValues) => {
    setIsSubmitting(true);
    try {
      await api.User.updateInformation({
        phoneNumber: values.phoneNumber || '',
        address: {
          zipCode: values.address.zipCode || '',
          city: values.address.city || '',
          street: values.address.street || '',
          houseNumber: values.address.houseNumber || '',
          country: values.address.country || ''
        }
      });
    } catch (error) {
      console.error('Failed to update profile:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Container size="md" py="xl">
      <Paper shadow="xs" p="xl" withBorder>
        <Title order={2} mb="md" align="center">Személyes adatok frissítése</Title>
        
        <form onSubmit={form.onSubmit(handleSubmit)}>
          <Box mb="md">
            <TextInput
              label="Telefonszám"
              placeholder="írd be a telefonszámod"
              {...form.getInputProps('phoneNumber')}
            />
          </Box>

          <Title order={4} mb="sm">Lakcím adatok</Title>
          <Grid>
            <Grid.Col span={{ base: 12, sm: 6 }}>
              <TextInput 
                label="Ország"
                placeholder="Ország"
                mb="md"
                {...form.getInputProps('address.country')}
              />
            </Grid.Col>
            <Grid.Col span={{ base: 12, sm: 6 }}>
              <TextInput 
                label="Irányítószám"
                placeholder="Irányítószám"
                mb="md"
                {...form.getInputProps('address.zipCode')}
              />
            </Grid.Col>
            <Grid.Col span={{ base: 12, sm: 6 }}>
              <TextInput 
                label="Város"
                placeholder="Város"
                mb="md"
                {...form.getInputProps('address.city')}
              />
            </Grid.Col>
            <Grid.Col span={{ base: 12, sm: 6 }}>
              <TextInput 
                label="Utca"
                placeholder="Utca"
                mb="md"
                {...form.getInputProps('address.street')}
              />
            </Grid.Col>
            <Grid.Col span={{ base: 12, sm: 6 }}>
              <TextInput 
                label="Házszám"
                placeholder="Házszám"
                mb="md"
                {...form.getInputProps('address.houseNumber')}
              />
            </Grid.Col>
          </Grid>

          <Button 
            type="submit"
            fullWidth 
            mt="md" 
            loading={isSubmitting}
          >
            Adatok frissitése
          </Button>
        </form>
      </Paper>
    </Container>
  );
}

export default Profile;