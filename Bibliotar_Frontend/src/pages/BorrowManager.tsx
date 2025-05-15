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
    ActionIcon
} from "@mantine/core";
import { useState, useEffect } from "react";
import api from "../api/api";
import { type BorrowDto, BorrowStatus, type ExtendBorrowDto, type UpdateBorrowStatusDto } from "../interfaces/BorrowInterfaces";
import { type CreateFineDto } from "../interfaces/FineInterfaces";
import { IconSearch, IconX } from "@tabler/icons-react";

const BorrowManager = () => {
    const [borrows, setBorrows] = useState<BorrowDto[]>([]);
    const [filteredBorrows, setFilteredBorrows] = useState<BorrowDto[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState("");
    const [error, setError] = useState<string | null>(null);
    const [extendingBorrowId, setExtendingBorrowId] = useState<number | null>(null);
    const [returningBorrowId, setReturningBorrowId] = useState<number | null>(null);
    const [imposingFineId, setImposingFineId] = useState<number | null>(null);
    const [successMessage, setSuccessMessage] = useState<string | null>(null);

    useEffect(() => {
        fetchBorrows();
    }, []);

    useEffect(() => {
        if (searchTerm === "") {
            setFilteredBorrows(borrows);
        } else {
            setFilteredBorrows(
                borrows.filter((borrow) => {
                    const bookTitle = getProperty(borrow, 'bookTitle') || '';
                    return bookTitle.toLowerCase().includes(searchTerm.toLowerCase());
                })
            );
        }
    }, [searchTerm, borrows]);

    useEffect(() => {
        if (successMessage) {
            const timer = setTimeout(() => {
                setSuccessMessage(null);
            }, 3000);
            return () => clearTimeout(timer);
        }
    }, [successMessage]);

    const fetchBorrows = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await api.Borrow.getAllBorrows();
            console.log("Received borrows:", response.data);

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

            setBorrows(borrowsData);
            setFilteredBorrows(borrowsData);
        } catch (error) {
            console.error("Error fetching borrows:", error);
            setError("Nem sikerült betölteni a kölcsönzéseket. Kérjük, próbálja újra később.");
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
        setError(null);
        try {
            console.log(`Extending borrow ID: ${borrowId}`);

            const extendData: ExtendBorrowDto = {
                id: borrowId,
                borrowPeriodExtendInDays: 7
            };

            console.log("Extending borrow with data:", extendData);
            await api.Borrow.extendPeriod(extendData);

            setBorrows(prevBorrows =>
                prevBorrows.map(borrow =>
                    getProperty(borrow, 'id') === borrowId
                        ? {
                            ...borrow,
                            renewalsLeft: (getProperty(borrow, 'renewalsLeft') - 1),

                            dueDate: new Date(new Date(getProperty(borrow, 'dueDate')).getTime() + 7 * 24 * 60 * 60 * 1000).toISOString(),
                            DueDate: new Date(new Date(getProperty(borrow, 'dueDate')).getTime() + 7 * 24 * 60 * 60 * 1000).toISOString()
                        }
                        : borrow
                )
            );

            setSuccessMessage("Kölcsönzés hosszabbítva!");

        } catch (error) {
            console.error("Error extending borrow:", error);
            setError("Nem sikerült meghosszabbítani a kölcsönzést. Kérjük, próbálja újra később.");
        } finally {
            setExtendingBorrowId(null);
        }
    };

    const handleReturnBook = async (borrowId: number) => {
        setReturningBorrowId(borrowId);
        setError(null);
        try {
            console.log(`Returning borrow ID: ${borrowId}`);

            const updateData: UpdateBorrowStatusDto = {
                id: borrowId,
                statusModifyer: 1
            };

            console.log("Returning book with data:", updateData);
            await api.Borrow.updateBorrowStatus(updateData);

            setBorrows(prevBorrows =>
                prevBorrows.map(borrow =>
                    getProperty(borrow, 'id') === borrowId
                        ? {
                            ...borrow,
                            borrowStatus: BorrowStatus.Returned,
                        }
                        : borrow
                )
            );

            setSuccessMessage("Könyv sikeresen visszavéve!");

        } catch (error) {
            console.error("Error returning book:", error);
            setError("Nem sikerült visszavenni a könyvet. Kérjük, próbálja újra később.");
        } finally {
            setReturningBorrowId(null);
        }
    };

    const handleImposeFine = async (borrowId: number, userId: number) => {
        setImposingFineId(borrowId);
        setError(null);
        try {
            console.log(`Imposing fine for borrow ID: ${borrowId}, user ID: ${userId}`);

            const fineData: CreateFineDto = {
                userId: userId,
                borrowId: borrowId,
                amount: 5000
            };

            console.log("Imposing fine with data:", fineData);
            await api.Fine.create(fineData);

            setSuccessMessage("Bírság sikeresen kiszabva!");

        } catch (error) {
            console.error("Error imposing fine:", error);
            setError("Nem sikerült bírságot kiszabni. Kérjük, próbálja újra később.");
        } finally {
            setImposingFineId(null);
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
                    <Title order={2} align="center" mb="md">Kölcsönzések kezelése</Title>

                    {error && (
                        <Text c="red" ta="center" mb="md">{error}</Text>
                    )}

                    {successMessage && (
                        <Text c="green" ta="center" mb="md">{successMessage}</Text>
                    )}

                    <TextInput
                        placeholder="Könyv címe alapján keresés..."
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
                                <Table.Th>Azonosító</Table.Th>
                                <Table.Th>Könyv címe</Table.Th>
                                <Table.Th>Felhasználó azonosító</Table.Th>
                                <Table.Th>Kölcsönzés dátuma</Table.Th>
                                <Table.Th>Lejárat dátuma</Table.Th>
                                <Table.Th>Hátralévő hosszabbítások</Table.Th>
                                <Table.Th>Státusz</Table.Th>
                                <Table.Th>Műveletek</Table.Th>
                            </Table.Tr>
                        </Table.Thead>
                        <Table.Tbody>
                            {filteredBorrows.length === 0 && !isLoading ? (
                                <Table.Tr>
                                    <Table.Td colSpan={8} align="center">
                                        {searchTerm ? "Nincs a keresésnek megfelelő kölcsönzés" : "Nincs aktív kölcsönzés az adatbázisban"}
                                    </Table.Td>
                                </Table.Tr>
                            ) : (
                                Array.isArray(filteredBorrows) && filteredBorrows.map((borrow) => {
                                    if (!borrow) return null;

                                    const borrowId = getProperty(borrow, 'id');
                                    const userId = getProperty(borrow, 'userId');
                                    const borrowStatus = getProperty(borrow, 'borrowStatus');
                                    const renewalsLeft = getProperty(borrow, 'renewalsLeft') || 0;
                                    const bookTitle = getProperty(borrow, 'bookTitle');
                                    const borrowDate = getProperty(borrow, 'borrowDate');
                                    const dueDate = getProperty(borrow, 'dueDate');
                                    
                                    const status = getStatusText(borrowStatus);
                                    const isCurrentlyExtending = extendingBorrowId === borrowId;
                                    const isCurrentlyReturning = returningBorrowId === borrowId;
                                    const isCurrentlyImposingFine = imposingFineId === borrowId;
                                    
                                    const canExtend = borrowStatus === BorrowStatus.Active && renewalsLeft > 0;
                                    const canReturn = borrowStatus === BorrowStatus.Active || borrowStatus === BorrowStatus.Overdue;

                                    return (
                                        <Table.Tr key={borrowId}>
                                            <Table.Td>{borrowId}</Table.Td>
                                            <Table.Td>{bookTitle}</Table.Td>
                                            <Table.Td>{userId}</Table.Td>
                                            <Table.Td>{formatDate(borrowDate)}</Table.Td>
                                            <Table.Td>{formatDate(dueDate)}</Table.Td>
                                            <Table.Td>{renewalsLeft}</Table.Td>
                                            <Table.Td c={status.color}>{status.text}</Table.Td>
                                            <Table.Td>
                                                <Group>
                                                    <Button
                                                        size="xs"
                                                        color="blue"
                                                        onClick={() => handleExtendBorrow(borrowId)}
                                                        loading={isCurrentlyExtending}
                                                        disabled={!canExtend}
                                                        title={!canExtend ? "Nem hosszabbítható tovább" : "Kölcsönzés hosszabbítása 7 nappal"}
                                                    >
                                                        Hosszabbítás
                                                    </Button>
                                                    <Button
                                                        size="xs"
                                                        color="green"
                                                        onClick={() => handleReturnBook(borrowId)}
                                                        loading={isCurrentlyReturning}
                                                        disabled={!canReturn}
                                                        title={!canReturn ? "A könyv már vissza lett adva" : "Könyv visszavétele"}
                                                    >
                                                        Visszavétel
                                                    </Button>
                                                    <Button
                                                        size="xs"
                                                        color="red"
                                                        onClick={() => handleImposeFine(borrowId, userId)}
                                                        loading={isCurrentlyImposingFine}
                                                        title="Bírság kiszabása (5000 Ft)"
                                                    >
                                                        Bírság kiszabása
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
        </Container>
    );
};

export default BorrowManager;