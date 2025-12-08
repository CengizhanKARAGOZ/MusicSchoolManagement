import axiosInstance from './axios.config';
import type { ApiResponse } from '../types/common.types';
import type { DashboardStats, RecentAppointment } from '../types/dashboard.types';

export const dashboardApi = {
    getStats: async (): Promise<DashboardStats> => {
        // const response = await axiosInstance.get<ApiResponse<DashboardStats>>('/Dashboard/stats');
        // return response.data.data;

        // Temporary hardcoded data until backend is ready
        return {
            totalStudents: 156,
            totalTeachers: 24,
            todayAppointments: 12,
            monthlyRevenue: 24500,
        };
    },

    getRecentAppointments: async (): Promise<RecentAppointment[]> => {
        const response = await axiosInstance.get<ApiResponse<RecentAppointment[]>>('/Appointments/upcoming?count=5');
        return response.data.data;
    },
};