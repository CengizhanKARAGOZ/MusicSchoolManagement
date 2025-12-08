export interface Student {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    dateOfBirth: string;
    parentName?: string;
    parentPhone?: string;
    address?: string;
    registrationDate: string;
    isActive: boolean;
}

export interface CreateStudentDto {
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    dateOfBirth: string;
    parentName?: string;
    parentPhone?: string;
    address?: string;
    registrationDate: string;
}

export interface UpdateStudentDto extends CreateStudentDto {
    isActive: boolean;
}