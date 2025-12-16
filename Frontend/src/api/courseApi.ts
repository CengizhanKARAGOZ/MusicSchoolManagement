import axiosInstance from './axios.config';
import type { ApiResponse } from '../types/common.types';
import type { Course, CreateCourseDto, UpdateCourseDto } from '../types/course.types';

export const courseApi = {
    getAll: async (): Promise<Course[]> => {
        const response = await axiosInstance.get<ApiResponse<Course[]>>('/Courses');
        return response.data.data;
    },

    getById: async (id: number): Promise<Course> => {
        const response = await axiosInstance.get<ApiResponse<Course>>(`/Courses/${id}`);
        return response.data.data;
    },

    create: async (data: CreateCourseDto): Promise<Course> => {
        const response = await axiosInstance.post<ApiResponse<Course>>('/Courses', data);
        return response.data.data;
    },

    update: async (id: number, data: UpdateCourseDto): Promise<Course> => {
        const response = await axiosInstance.put<ApiResponse<Course>>(`/Courses/${id}`, data);
        return response.data.data;
    },

    delete: async (id: number): Promise<void> => {
        await axiosInstance.delete(`/Courses/${id}`);
    },
};