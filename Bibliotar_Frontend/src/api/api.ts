// src/api/api.ts
import axiosInstance from "./axios.config.ts";
import {type BookGetDto } from "../interfaces/BookInterfaces.ts";
import {type UserRegistrationDto } from "../interfaces/BookInterfaces.ts";
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
            axiosInstance.post<number>('/api/Reservation/createAsGuest', { Email: email, BookId: bookId })
    },
    User: {
        createUser: (userData: UserRegistrationDto) =>
            axiosInstance.post('/api/User/create', userData),
        // Add other user endpoints here (login, update profile, etc.)
    }

};

export default api;