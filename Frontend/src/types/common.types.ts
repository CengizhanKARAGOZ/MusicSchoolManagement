export interface ApiResponse<T> {
    success: boolean;
    message: string;
    data: T;
    errors: string[] | null;
}

export interface PaginatedResponse<T> {
    items: T[];
    totalCount: number;
    pageNumber: number;
    pageSize: number;
}

export enum UserRole {
    Admin = 'Admin',
    Teacher = 'Teacher',
    Student = 'Student'
}

export enum AppointmentStatus {
    Scheduled = 'Scheduled',
    Completed = 'Completed',
    Cancelled = 'Cancelled',
    NoShow = 'NoShow'
}

export enum PaymentStatus {
    Pending = 'Pending',
    Completed = 'Completed',
    Failed = 'Failed',
    Refunded = 'Refunded'
}

export enum PaymentMethod {
    Cash = 'Cash',
    CreditCard = 'CreditCard',
    BankTransfer = 'BankTransfer',
    Other = 'Other'
}

export enum StudentPackageStatus {
    Active = 'Active',
    Completed = 'Completed',
    Cancelled = 'Cancelled',
    Expired = 'Expired'
}