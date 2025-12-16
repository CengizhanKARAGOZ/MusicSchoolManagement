import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { packageApi } from '../api/packageApi';
import type { UpdatePackageDto } from '../types/package.types';

export const usePackages = () => {
    const queryClient = useQueryClient();

    const { data: packages, isLoading, error } = useQuery({
        queryKey: ['packages'],
        queryFn: packageApi.getAll,
    });
    
    const createMutation = useMutation({
        mutationFn: packageApi.create,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['packages'] });
        },
    });
    
    const updateMutation = useMutation({
        mutationFn: ({ id, data }: { id: number; data: UpdatePackageDto }) =>
            packageApi.update(id, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['packages'] });
        },
    });
    
    const deleteMutation = useMutation({
        mutationFn: packageApi.delete,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['packages'] });
        },
    });
    
    return {
        packages,
        isLoading,
        error,
        createPackage: createMutation.mutateAsync,
        updatePackage: updateMutation.mutateAsync,
        deletePackage: deleteMutation.mutateAsync,
    };
};