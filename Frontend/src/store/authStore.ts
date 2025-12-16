import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import type { LoginResponse, User } from '../types/auth.types';

interface AuthState {
    user: User | null;
    token: string | null;
    isAuthenticated: boolean;
    passwordChangeRequired: boolean;
    login: (data: LoginResponse) => void;
    logout: () => void;
    updateUser: (user: User) => void;
}

export const useAuthStore = create<AuthState>()(
    persist(
        (set) => ({
            user: null,
            token: null,
            isAuthenticated: false,
            passwordChangeRequired: false,

            login: (data: LoginResponse) => {
                localStorage.setItem('token', data.token);
                set({
                    user: {
                        id: data.id,
                        firstName: data.firstName,
                        lastName: data.lastName,
                        email: data.email,
                        role: data.role,
                        isActive: true,
                    },
                    token: data.token,
                    isAuthenticated: true,
                    passwordChangeRequired: data.passwordChangeRequired || false,
                });
            },

            logout: () => {
                localStorage.removeItem('token');
                set({
                    user: null,
                    token: null,
                    isAuthenticated: false,
                    passwordChangeRequired: false,
                });
            },

            updateUser: (user: User) => {
                set({
                    user,
                    passwordChangeRequired: false
                });
            },
        }),
        {
            name: 'auth-storage',
        }
    )
);