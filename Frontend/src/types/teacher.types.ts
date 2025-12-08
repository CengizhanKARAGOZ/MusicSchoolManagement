export interface Teacher {
    id: number;
    userId: number;
    user?: {
        id: number;
        firstName: string;
        lastName: string;
        email: string;
        phoneNumber?: string;
        isActive: boolean;
    };
    specializations?: string;
    hourlyRate?: number;
    biography?: string;
    availabilityNotes?: string;
    createdAt: string;
    updatedAt?: string;
}

export interface CreateTeacherDto {
    specializations?: string;
    hourlyRate?: number;
    biography?: string;
    availabilityNotes?: string;
}

export interface UpdateTeacherDto {
    specializations?: string;
    hourlyRate?: number;
    biography?: string;
    availabilityNotes?: string;
}