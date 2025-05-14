export interface UserUpdateDto {
    phoneNumber: string;
    address: {
        zipCode: string;
        city: string;
        street: string;
        houseNumber: string;
        country: string;
    }
}
