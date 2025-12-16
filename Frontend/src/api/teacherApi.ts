import axiosInstance from './axios.config';
import type { ApiResponse } from '../types/common.types';
import type { Teacher, CreateTeacherDto, UpdateTeacherDto } from '../types/teacher.types';

export const teacherApi = {
    getAll: async (): Promise<Teacher[]> => {
        const response = await axiosInstance.get<ApiResponse<Teacher[]>>('/Teachers');
        return response.data.data;
    },

    getById: async (id: number): Promise<Teacher> => {
        const response = await axiosInstance.get<ApiResponse<Teacher>>(`/Teachers/${id}`);
        return response.data.data;
    },

    create: async (data: CreateTeacherDto): Promise<Teacher> => {
        const response = await axiosInstance.post<ApiResponse<Teacher>>('/Teachers', data);
        return response.data.data;
    },

    update: async (id: number, data: UpdateTeacherDto): Promise<Teacher> => {
        const response = await axiosInstance.put<ApiResponse<Teacher>>(`/Teachers/${id}`, data);
        return response.data.data;
    },

    delete: async (id: number): Promise<void> => {
        await axiosInstance.delete(`/Teachers/${id}`);
    },
};