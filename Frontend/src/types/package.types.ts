export interface Package {
    id: number;
    name: string;
    description?: string;
    lessonCount: number;
    price: number;
    validityDays: number;
    isActive: boolean;
    createdAt: string;
    updatedAt?: string;
}

export interface CreatePackageDto {
    name: string;
    description?: string;
    lessonCount: number;
    price: number;
    validityDays: number;
    isActive: boolean;
}

export interface UpdatePackageDto {
    name: string;
    description?: string;
    lessonCount: number;
    price: number;
    validityDays: number;
    isActive: boolean;
}