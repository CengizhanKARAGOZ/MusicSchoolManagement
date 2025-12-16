import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { teacherApi } from '../api/teacherApi';
import type { UpdateTeacherDto } from '../types/teacher.types';

export const useTeachers = () => {
    const queryClient = useQueryClient();

    const { data: teachers, isLoading, error } = useQuery({
        queryKey: ['teachers'],
        queryFn: teacherApi.getAll,
    });
    
    const createMutation = useMutation({
        mutationFn: teacherApi.create,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['teachers'] });
        },
    });

    const updateMutation = useMutation({
        mutationFn: ({ id, data }: { id: number; data: UpdateTeacherDto }) =>
            teacherApi.update(id, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['teachers'] });
        },
    });

    const deleteMutation = useMutation({
        mutationFn: teacherApi.delete,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['teachers'] });
        },
    });

    return {
        teachers,
        isLoading,
        error,
        createTeacher: createMutation.mutateAsync,
        updateTeacher: updateMutation.mutateAsync,
        deleteTeacher: deleteMutation.mutateAsync,
    };
};