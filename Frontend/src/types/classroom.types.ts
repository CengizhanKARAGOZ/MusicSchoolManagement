export interface Classroom {
    id: number;
    name: string;
    capacity: number;
    location?: string;
    equipment?: string;
    isAvailable: boolean;
    createdAt: string;
    updatedAt?: string;
}

export interface CreateClassroomDto {
    name: string;
    capacity: number;
    location?: string;
    equipment?: string;
    isAvailable: boolean;
}

export interface UpdateClassroomDto {
    name: string;
    capacity: number;
    location?: string;
    equipment?: string;
    isAvailable: boolean;
}