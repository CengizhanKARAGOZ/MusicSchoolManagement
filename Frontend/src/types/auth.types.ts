import { UserRole } from './common.types';

export interface LoginRequest {
    email: string;
    password: string;
}

export interface RegisterRequest {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    phoneNumber?: string;
    role: UserRole;
}

export interface LoginResponse {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    role: UserRole;
    token: string;
    refreshToken: string;
    expiresAt: string;
    passwordChangeRequired?: boolean;
}

export interface User {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    role: UserRole;
    phoneNumber?: string;
    isActive: boolean;
    passwordChangeRequired?: boolean;
}