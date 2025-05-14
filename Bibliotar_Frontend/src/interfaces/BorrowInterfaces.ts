export enum BorrowStatus {
    Active = 0,
    Returned = 1,
    Overdue = 2
}

export interface BorrowDto {
    id: number;
    bookTitle: string;
    BookTitle: string; // Alternate casing to handle API response
    borrowDate: string;
    BorrowDate: string; // Alternate casing
    dueDate: string;
    DueDate: string; // Alternate casing
    renewalsLeft: number;
    RenewalsLeft: number; // Alternate casing
    borrowStatus: BorrowStatus;
}

export interface ExtendBorrowDto {
    id: number;
    borrowPeriodExtendInDays: number;
}