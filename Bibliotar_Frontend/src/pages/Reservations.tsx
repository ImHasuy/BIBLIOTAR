import {
  Container,
  Title,
  Table,
  LoadingOverlay,
  Paper,
  Stack,
  Text
} from "@mantine/core";
import { useState, useEffect } from "react";
import api from "../api/api";
import {type ReservationDto, ReservationStatus } from "../interfaces/ReservationInterfaces";

const Reservations = () => {
  const [reservations, setReservations] = useState<ReservationDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchReservations();
  }, []);

  const fetchReservations = async () => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await api.Reservation.getMyReservations();
      setReservations(response.data);
    } catch (error) {
      console.error("Error fetching reservations:", error);
      setError("Nem sikerült betölteni a foglalásokat. Kérjük, próbálja újra később.");
    } finally {
      setIsLoading(false);
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('hu-HU');
  };

  const getStatusText = (status: ReservationStatus) => {
    switch (status) {
      case ReservationStatus.Active:
        return { text: 'Aktív', color: "green" };
      case ReservationStatus.Completed:
        return { text: 'Teljesítve', color: "blue" };
      case ReservationStatus.Canceled:
        return { text: 'Lemondva', color: "red" };
      default:
        return { text: 'Ismeretlen', color: "gray" };
    }
  };

  return (
    <Container size="lg">
      <Paper radius="md" p="xl" withBorder position="relative">
        <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />
        
        <Stack>
          <Title order={2} align="center" mb="md">Foglalásaim</Title>
          
          {error && (
            <Text c="red" ta="center" mb="md">{error}</Text>
          )}
          
          <Table striped highlightOnHover>
            <Table.Thead>
              <Table.Tr>
                <Table.Th>Foglalás azonosító</Table.Th>
                <Table.Th>Könyv címe</Table.Th>
                <Table.Th>Foglalás dátuma</Table.Th>
                <Table.Th>Státusz</Table.Th>
              </Table.Tr>
            </Table.Thead>
            <Table.Tbody>
              {reservations.length === 0 && !isLoading ? (
                <Table.Tr>
                  <Table.Td colSpan={4} align="center">Nincs aktív foglalásod</Table.Td>
                </Table.Tr>
              ) : (
                reservations.map((reservation) => {
                  const status = getStatusText(reservation.status);
                  
                  return (
                    <Table.Tr key={reservation.id}>
                      <Table.Td>{reservation.id}</Table.Td>
                      <Table.Td>{reservation.bookTitle}</Table.Td>
                      <Table.Td>{formatDate(reservation.reservationDate)}</Table.Td>
                      <Table.Td c={status.color}>{status.text}</Table.Td>
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

export default Reservations;