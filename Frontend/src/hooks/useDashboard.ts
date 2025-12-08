import { useQuery } from '@tanstack/react-query';
import { dashboardApi } from '../api/dashboardApi';

export const useDashboard = () => {
    const { data: stats, isLoading: statsLoading } = useQuery({
        queryKey: ['dashboard', 'stats'],
        queryFn: dashboardApi.getStats,
    });

    const { data: recentAppointments, isLoading: appointmentsLoading } = useQuery({
        queryKey: ['dashboard', 'appointments'],
        queryFn: dashboardApi.getRecentAppointments,
    });

    return {
        stats,
        recentAppointments,
        isLoading: statsLoading || appointmentsLoading,
    };
};