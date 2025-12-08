import axiosInstance from './axios.config';
import type { ApiResponse } from '../types/common.types';
import type { Student, CreateStudentDto, UpdateStudentDto } from '../types/student.types';

export const studentApi = {
    getAll: async (): Promise<Student[]> => {
        const response = await axiosInstance.get<ApiResponse<Student[]>>('/Students');
        return response.data.data;
    },

    getActive: async (): Promise<Student[]> => {
        const response = await axiosInstance.get<ApiResponse<Student[]>>('/Students/active');
        return response.data.data;
    },

    getById: async (id: number): Promise<Student> => {
        const response = await axiosInstance.get<ApiResponse<Student>>(`/Students/${id}`);
        return response.data.data;
    },

    create: async (data: CreateStudentDto): Promise<Student> => {
        const response = await axiosInstance.post<ApiResponse<Student>>('/Students', data);
        return response.data.data;
    },

    update: async (id: number, data: UpdateStudentDto): Promise<Student> => {
        const response = await axiosInstance.put<ApiResponse<Student>>(`/Students/${id}`, data);
        return response.data.data;
    },

    delete: async (id: number): Promise<void> => {
        await axiosInstance.delete(`/Students/${id}`);
    },
};