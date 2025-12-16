import axiosInstance from './axios.config';
import type { ApiResponse } from '../types/common.types';
import type { Appointment, CreateAppointmentDto, UpdateAppointmentDto } from '../types/appointment.types';

export const appointmentApi = {
    getAll: async (): Promise<Appointment[]> => {
        const response = await axiosInstance.get<ApiResponse<Appointment[]>>('/Appointments');
        return response.data.data;
    },

    getByStudent: async (studentId: number): Promise<Appointment[]> => {
        const response = await axiosInstance.get<ApiResponse<Appointment[]>>(`/Appointments/student/${studentId}`);
        return response.data.data;
    },

    getByTeacher: async (teacherId: number): Promise<Appointment[]> => {
        const response = await axiosInstance.get<ApiResponse<Appointment[]>>(`/Appointments/teacher/${teacherId}`);
        return response.data.data;
    },

    getByDate: async (date: string): Promise<Appointment[]> => {
        const response = await axiosInstance.get<ApiResponse<Appointment[]>>(`/Appointments/date/${date}`);
        return response.data.data;
    },

    getById: async (id: number): Promise<Appointment> => {
        const response = await axiosInstance.get<ApiResponse<Appointment>>(`/Appointments/${id}`);
        return response.data.data;
    },

    create: async (data: CreateAppointmentDto): Promise<Appointment> => {
        const response = await axiosInstance.post<ApiResponse<Appointment>>('/Appointments', data);
        return response.data.data;
    },

    update: async (id: number, data: UpdateAppointmentDto): Promise<Appointment> => {
        const response = await axiosInstance.put<ApiResponse<Appointment>>(`/Appointments/${id}`, data);
        return response.data.data;
    },

    cancel: async (id: number): Promise<Appointment> => {
        const response = await axiosInstance.put<ApiResponse<Appointment>>(`/Appointments/${id}/cancel`, {});
        return response.data.data;
    },

    delete: async (id: number): Promise<void> => {
        await axiosInstance.delete(`/Appointments/${id}`);
    },
};