import axiosInstance from './axios.config';
import type { ApiResponse } from '../types/common.types';
import type { LoginRequest, LoginResponse, RegisterRequest } from '../types/auth.types';

export const authApi = {
    login: async (data: LoginRequest): Promise<LoginResponse> => {
        const response = await axiosInstance.post<ApiResponse<LoginResponse>>('/Auth/login', data);
        return response.data.data;
    },

    register: async (data: RegisterRequest): Promise<LoginResponse> => {
        const response = await axiosInstance.post<ApiResponse<LoginResponse>>('/Auth/register', data);
        return response.data.data;
    },
};