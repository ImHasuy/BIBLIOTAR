export enum ReservationStatus {
    Active = 0,
    Completed = 1,
    Canceled = 2
}

export interface ReservationDto {
    id: number;
    email: string | null;
    userId: number | null;
    bookId: number;
    bookTitle: string;
    reservationDate: string;
    status: ReservationStatus;
}