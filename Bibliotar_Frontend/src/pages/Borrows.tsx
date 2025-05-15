import {
    Container,
    Title,
    Table,
    LoadingOverlay,
    Paper,
    Stack,
    Button,
    Text
} from "@mantine/core";
import { useState, useEffect } from "react";
import api from "../api/api";
import axiosInstance from "../api/axios.config";
import {type BorrowDto, BorrowStatus } from "../interfaces/BorrowInterfaces";

const Borrows = () => {
    const [borrows, setBorrows] = useState<BorrowDto[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [extendingBorrowId, setExtendingBorrowId] = useState<number | null>(null);

    useEffect(() => {
        fetchBorrows();
    }, []);

    const fetchBorrows = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await api.Borrow.getUserBorrows();
            console.log("Received user borrows raw:", response.data);

            let borrowsData: BorrowDto[] = [];
            if (Array.isArray(response.data)) {
                borrowsData = response.data;
            } else if (response.data && typeof response.data === 'object') {
                const possibleArrays = Object.values(response.data).filter(value => Array.isArray(value));
                if (possibleArrays.length > 0) {
                    borrowsData = possibleArrays[0] as BorrowDto[];
                } else {
                    borrowsData = [response.data as BorrowDto];
                }
            }
            
            console.log("Processed borrows data:", borrowsData);
            setBorrows(borrowsData);
        } catch (error) {
            console.error("Error fetching borrows:", error);
            setError("Nem sikerült betölteni a kölcsönzéseket. Kérjük, próbálja újra később.");
        } finally {
            setIsLoading(false);
        }
    };

    // Format date
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

    // Get status text
    const getStatusText = (status: BorrowStatus | undefined) => {
        switch (status) {
            case BorrowStatus.Active:
                return { text: 'Aktív', color: "green" };
            case BorrowStatus.Returned:
                return { text: 'Visszaadva', color: "blue" };
            case BorrowStatus.Overdue:
                return { text: 'Késedelmes', color: "red" };
            default:
                return { text: 'Ismeretlen', color: "gray" };
        }
    };

    const getProperty = (obj: any, key: string): any => {
        if (!obj) return undefined;

        if (obj[key] !== undefined) return obj[key];

        const camelCaseKey = key.charAt(0).toLowerCase() + key.slice(1);
        if (obj[camelCaseKey] !== undefined) return obj[camelCaseKey];

        const pascalCaseKey = key.charAt(0).toUpperCase() + key.slice(1);
        if (obj[pascalCaseKey] !== undefined) return obj[pascalCaseKey];
        
        return undefined;
    };

    const handleExtendBorrow = async (borrowId: number) => {
        setExtendingBorrowId(borrowId);
        try {
            const extendData = {
                id: borrowId,
                borrowPeriodExtendInDays: 7
            };

            await api.Borrow.extendPeriod(extendData);

            setBorrows(prevBorrows =>
                prevBorrows.map(borrow =>
                    borrow.id === borrowId
                        ? {
                            ...borrow,
                            renewalsLeft: (getProperty(borrow, 'renewalsLeft') - 1),

                            dueDate: new Date(new Date(getProperty(borrow, 'dueDate')).getTime() + 7 * 24 * 60 * 60 * 1000).toISOString(),
                            DueDate: new Date(new Date(getProperty(borrow, 'dueDate')).getTime() + 7 * 24 * 60 * 60 * 1000).toISOString()
                        }
                        : borrow
                )
            );

        } catch (error) {
            console.error("Error extending borrow:", error);
            setError("Nem sikerült meghosszabbítani a kölcsönzést. Kérjük, próbálja újra később.");
        } finally {
            setExtendingBorrowId(null);
        }
    };

    return (
        <Container size="lg">
            <Paper radius="md" p="xl" withBorder position="relative">
                <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />

                <Stack>
                    <Title order={2} align="center" mb="md">Kölcsönzéseim</Title>

                    {error && (
                        <Text c="red" ta="center" mb="md">{error}</Text>
                    )}

                    <Table striped highlightOnHover>
                        <Table.Thead>
                            <Table.Tr>
                                <Table.Th>Kölcsönzés azonosító</Table.Th>
                                <Table.Th>Könyv címe</Table.Th>
                                <Table.Th>Kölcsönzés dátuma</Table.Th>
                                <Table.Th>Lejárat dátuma</Table.Th>
                                <Table.Th>Hátralévő hosszabbítások</Table.Th>
                                <Table.Th>Státusz</Table.Th>
                                <Table.Th></Table.Th>
                            </Table.Tr>
                        </Table.Thead>
                        <Table.Tbody>
                            {borrows.length === 0 && !isLoading ? (
                                <Table.Tr>
                                    <Table.Td colSpan={7} align="center">Nincs aktív kölcsönzésed</Table.Td>
                                </Table.Tr>
                            ) : (
                                Array.isArray(borrows) && borrows.map((borrow) => {
                                    if (!borrow) return null;

                                    const status = getStatusText(getProperty(borrow, 'borrowStatus'));
                                    const isCurrentlyExtending = extendingBorrowId === borrow.id;
                                    const renewalsLeft = getProperty(borrow, 'renewalsLeft') || 0;
                                    const canExtend = getProperty(borrow, 'borrowStatus') === BorrowStatus.Active && renewalsLeft > 0;

                                    return (
                                        <Table.Tr key={borrow.id}>
                                            <Table.Td>{borrow.id}</Table.Td>
                                            <Table.Td>{getProperty(borrow, 'bookTitle')}</Table.Td>
                                            <Table.Td>{formatDate(getProperty(borrow, 'borrowDate'))}</Table.Td>
                                            <Table.Td>{formatDate(getProperty(borrow, 'dueDate'))}</Table.Td>
                                            <Table.Td>{renewalsLeft}</Table.Td>
                                            <Table.Td c={status.color}>{status.text}</Table.Td>
                                            <Table.Td>
                                                {canExtend && (
                                                    <Button
                                                        size="xs"
                                                        color="blue"
                                                        onClick={() => handleExtendBorrow(borrow.id)}
                                                        loading={isCurrentlyExtending}
                                                        disabled={isCurrentlyExtending}
                                                    >
                                                        Hosszabbítás
                                                    </Button>
                                                )}
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

export default Borrows;