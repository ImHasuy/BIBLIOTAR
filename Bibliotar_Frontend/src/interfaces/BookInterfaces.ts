export interface BookGetDto {
    title: string;
    author: string;
    quality: string;
    isbn: string;
    category: string;
    publishDate: string;
    status: number;
    id?: number;
}
export interface UserRegistrationDto {
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

export interface BookCreateDto {
    title: string;
    author: string;
    quality: string;
    isbn: string;
    category: string;
    publishDate: string;
    status: number;
}

export interface BookDeleteDto {
    bookId: number;
}
