import { useAuthStore } from '../store/authStore';
import { authApi } from '../api/authApi';
import type { LoginRequest, RegisterRequest } from '../types/auth.types';
import { useState } from 'react';

export const useAuth = () => {
    const { user, token, isAuthenticated, login: setLogin, logout: setLogout } = useAuthStore();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const login = async (credentials: LoginRequest) => {
        try {
            setLoading(true);
            setError(null);
            const response = await authApi.login(credentials);
            setLogin(response);
            return response;
        } catch (err: any) {
            const errorMessage = err.response?.data?.message || 'Login failed';
            setError(errorMessage);
            throw new Error(errorMessage);
        } finally {
            setLoading(false);
        }
    };

    const register = async (data: RegisterRequest) => {
        try {
            setLoading(true);
            setError(null);
            const response = await authApi.register(data);
            setLogin(response);
            return response;
        } catch (err: any) {
            const errorMessage = err.response?.data?.message || 'Registration failed';
            setError(errorMessage);
            throw new Error(errorMessage);
        } finally {
            setLoading(false);
        }
    };

    const logout = () => {
        setLogout();
    };

    return {
        user,
        token,
        isAuthenticated,
        loading,
        error,
        login,
        register,
        logout,
    };
};