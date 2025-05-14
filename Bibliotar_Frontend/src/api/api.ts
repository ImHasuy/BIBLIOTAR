
// src/api/api.ts
import axiosInstance from "./axios.config.ts";
import {type BookGetDto } from "../interfaces/BookInterfaces.ts";
import {type UserRegistrationDto } from "../interfaces/BookInterfaces.ts";
import {type UserUpdateDto } from "../interfaces/UserInterfaces.ts";
import {type ReservationDto } from "../interfaces/ReservationInterfaces.ts";
import {type BorrowDto, type ExtendBorrowDto, type UpdateBorrowStatusDto } from "../interfaces/BorrowInterfaces.ts";
import {type CreateFineDto} from "../interfaces/FineInterfaces.ts";

const api = {
    Auth: {
        login: (email: string, password: string) =>
            axiosInstance.post<{token: string}>(`/api/User/login`, {email, password})
    },
    Book: {
        listBooks: () =>
            axiosInstance.get<BookGetDto[]>(`/api/Book/getall`)
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
            axiosInstance.post('/api/Borrow/UpdateBorrow', updateData) // Changed from PUT to POST

    },
    Fine: {
        create: (fineData: CreateFineDto) =>
            axiosInstance.post('/api/Fine/Create', fineData)
    }



};

export default api;