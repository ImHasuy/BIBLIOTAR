import axiosInstance from "./axios.config.ts";
import {type BookGetDto } from "../interfaces/BookInterfaces.ts";
import {type UserRegistrationDto } from "../interfaces/BookInterfaces.ts";
import {type UserUpdateDto } from "../interfaces/UserInterfaces.ts";
import {type ReservationDto } from "../interfaces/ReservationInterfaces.ts";
import {type BorrowDto, type ExtendBorrowDto, type UpdateBorrowStatusDto } from "../interfaces/BorrowInterfaces.ts";
import {type CreateFineDto} from "../interfaces/FineInterfaces.ts";
import {type BookCreateDto } from "../interfaces/BookInterfaces.ts";
import {type BookDeleteDto } from "../interfaces/BookInterfaces.ts";

// @ts-ignore
const api = {
    Auth: {
        login: (email: string, password: string) =>
            axiosInstance.post<{token: string}>(`/api/User/login`, {email, password})
    },
    Book: {
        listBooks: () =>
            axiosInstance.get<BookGetDto[]>(`/api/Book/getall`),
        createBook: (bookData: BookCreateDto) =>
            axiosInstance.post('/api/Book/create', bookData),
        updateBook: (bookData: BookGetDto) =>
            axiosInstance.put('/api/Book/update', bookData),
        removeBook: (bookId: number) => {

            console.log("Sending delete request with data:", bookId);
            return axiosInstance.delete('/api/Book/remove/'+ bookId ,{
                headers: {
                    'Content-Type': 'application/json'
                }
            });
        }
    },
    Reservation: {
        createAsGuest: (email: string, bookId: number) =>
            axiosInstance.post<number>('/api/Reservation/createAsGuest', { Email: email, BookId: bookId }),
        create: (bookId: number) =>
            axiosInstance.post<number>('/api/Reservation/create', { bookId }),
        getMyReservations: () =>
            axiosInstance.get<ReservationDto[]>('/api/Reservation/MyReservations')


    },
    User: {
        createUser: (userData: UserRegistrationDto) =>
            axiosInstance.post('/api/User/create', userData),

        updateInformation: (userInfo: UserUpdateDto) =>
            axiosInstance.put('/api/User/UpdateInformations', userInfo)

    },
    Borrow: {
        getAllBorrows: () =>
            axiosInstance.get<BorrowDto[]>('/api/Borrow/GetAllBorrow'),
        extendPeriod: (extendData: ExtendBorrowDto) =>
            axiosInstance.put('/api/Borrow/ExtendPeriod', extendData),
        updateBorrowStatus: (updateData: UpdateBorrowStatusDto) =>
            axiosInstance.post('/api/Borrow/UpdateBorrow', updateData),
        create: (borrowData: { userId: number; bookId: number; borrowPeriodInDays: number }) =>
            axiosInstance.post('/api/Borrow/create', borrowData),
        getUserBorrows: () =>
            axiosInstance.get<BorrowDto[]>('/api/Borrow/GetAllBorrowForUser')
    },
    Fine: {
        create: (fineData: CreateFineDto) =>
            axiosInstance.post('/api/Fine/Create', fineData)
    }
};

export default api;