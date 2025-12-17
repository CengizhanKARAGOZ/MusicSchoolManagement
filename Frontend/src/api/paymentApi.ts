import axiosInstance from './axios.config';
import type { ApiResponse } from '../types/common.types';
import type { Payment, CreatePaymentDto, UpdatePaymentDto } from '../types/payment.types';

export const paymentApi = {
    getAll: async (): Promise<Payment[]> => {
        const response = await axiosInstance.get<ApiResponse<Payment[]>>('/Payments');
        return response.data.data;
    },

    getByStudent: async (studentId: number): Promise<Payment[]> => {
        const response = await axiosInstance.get<ApiResponse<Payment[]>>(`/Payments/student/${studentId}`);
        return response.data.data;
    },

    getById: async (id: number): Promise<Payment> => {
        const response = await axiosInstance.get<ApiResponse<Payment>>(`/Payments/${id}`);
        return response.data.data;
    },

    create: async (data: CreatePaymentDto): Promise<Payment> => {
        const response = await axiosInstance.post<ApiResponse<Payment>>('/Payments', data);
        return response.data.data;
    },

    update: async (id: number, data: UpdatePaymentDto): Promise<Payment> => {
        const response = await axiosInstance.put<ApiResponse<Payment>>(`/Payments/${id}`, data);
        return response.data.data;
    },

    delete: async (id: number): Promise<void> => {
        await axiosInstance.delete(`/Payments/${id}`);
    },
};