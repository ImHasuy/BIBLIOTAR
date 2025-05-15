export interface UpdateBorrowStatusDto {
    borrowId: number;
    newStatus: number;
}

export interface BorrowDto {
    id: number;
    userId: number;
    bookId: number;
    borrowDate: string;
    returnDate: string;
    status: number;
}

export interface ExtendBorrowDto {
    borrowId: number;
    extensionPeriodInDays: number;
}

export interface CreateBorrowDto {
    userId: number;
    bookId: number;
    borrowPeriodInDays: number;
}

export enum BorrowStatus {
    Active = 0,
    Returned = 1,
    Overdue = 2,
    Lost = 3
}