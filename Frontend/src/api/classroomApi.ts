import axiosInstance from './axios.config';
import type { ApiResponse } from '../types/common.types';
import type { Classroom, CreateClassroomDto, UpdateClassroomDto } from '../types/classroom.types';

export const classroomApi = {
    getAll: async (): Promise<Classroom[]> => {
        const response = await axiosInstance.get<ApiResponse<Classroom[]>>('/Classrooms');
        return response.data.data;
    },

    getById: async (id: number): Promise<Classroom> => {
        const response = await axiosInstance.get<ApiResponse<Classroom>>(`/Classrooms/${id}`);
        return response.data.data;
    },

    create: async (data: CreateClassroomDto): Promise<Classroom> => {
        const response = await axiosInstance.post<ApiResponse<Classroom>>('/Classrooms', data);
        return response.data.data;
    },

    update: async (id: number, data: UpdateClassroomDto): Promise<Classroom> => {
        const response = await axiosInstance.put<ApiResponse<Classroom>>(`/Classrooms/${id}`, data);
        return response.data.data;
    },

    delete: async (id: number): Promise<void> => {
        await axiosInstance.delete(`/Classrooms/${id}`);
    },
};