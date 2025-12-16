import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { appointmentApi } from '../api/appointmentApi';
import type { UpdateAppointmentDto } from '../types/appointment.types';

export const useAppointments = () => {
    const queryClient = useQueryClient();

    const { data: appointments, isLoading, error } = useQuery({
        queryKey: ['appointments'],
        queryFn: appointmentApi.getAll,
    });

    const createMutation = useMutation({
        mutationFn: appointmentApi.create,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['appointments'] });
        },
    });

    const updateMutation = useMutation({
        mutationFn: ({ id, data }: { id: number; data: UpdateAppointmentDto }) =>
            appointmentApi.update(id, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['appointments'] });
        },
    });

    const cancelMutation = useMutation({
        mutationFn: appointmentApi.cancel,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['appointments'] });
        },
    });

    const deleteMutation = useMutation({
        mutationFn: appointmentApi.delete,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['appointments'] });
        },
    });

    return {
        appointments,
        isLoading,
        error,
        createAppointment: createMutation.mutateAsync,
        updateAppointment: updateMutation.mutateAsync,
        cancelAppointment: cancelMutation.mutateAsync,
        deleteAppointment: deleteMutation.mutateAsync,
    };
};