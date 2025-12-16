export interface Course {
    id: number;
    name: string;
    description?: string;
    instrumentId?: number;
    instrumentName?: string;
    duration: number;
    price: number;
    isActive: boolean;
    createdAt: string;
    updatedAt?: string;
}

export interface CreateCourseDto {
    name: string;
    description?: string;
    instrumentId?: number;
    duration: number;
    price: number;
    isActive: boolean;
}

export interface UpdateCourseDto {
    name: string;
    description?: string;
    instrumentId?: number;
    duration: number;
    price: number;
    isActive: boolean;
}