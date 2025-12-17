import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { paymentApi } from '../api/paymentApi';
import type { UpdatePaymentDto } from '../types/payment.types';

export const usePayments = () => {
    const queryClient = useQueryClient();

    const { data: payments, isLoading, error } = useQuery({
        queryKey: ['payments'],
        queryFn: paymentApi.getAll,
    });

    const createMutation = useMutation({
        mutationFn: paymentApi.create,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['payments'] });
        },
    });

    const updateMutation = useMutation({
        mutationFn: ({ id, data }: { id: number; data: UpdatePaymentDto }) =>
            paymentApi.update(id, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['payments'] });
        },
    });

    const deleteMutation = useMutation({
        mutationFn: paymentApi.delete,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['payments'] });
        },
    });

    return {
        payments,
        isLoading,
        error,
        createPayment: createMutation.mutateAsync,
        updatePayment: updateMutation.mutateAsync,
        deletePayment: deleteMutation.mutateAsync,
    };
};