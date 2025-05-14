// src/interfaces/BookInterfaces.ts
export interface BookGetDto {
    title: string;
    author: string;
    quality: string;
    isbn: string;
    category: string;
    publishDate: string; // Date will come as string from API
    status: number;
    id?: number; // Adding ID for navigation purposes
}
interface UserRegistrationDto {
    email: string;
    phoneNumber: string;
    name: string;
    password: string;
    zipCode: string;
    city: string;
    street: string;
    houseNumber: string;
    country: string;
}

