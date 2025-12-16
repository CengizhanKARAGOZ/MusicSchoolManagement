import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { classroomApi } from '../api/classroomApi';
import type { UpdateClassroomDto } from '../types/classroom.types';

export const useClassrooms = () => {
    const queryClient = useQueryClient();

    const { data: classrooms, isLoading, error } = useQuery({
        queryKey: ['classrooms'],
        queryFn: classroomApi.getAll,
    });

    const createMutation = useMutation({
        mutationFn: classroomApi.create,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['classrooms'] });
        },
    });

    const updateMutation = useMutation({
        mutationFn: ({ id, data }: { id: number; data: UpdateClassroomDto }) =>
            classroomApi.update(id, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['classrooms'] });
        },
    });

    const deleteMutation = useMutation({
        mutationFn: classroomApi.delete,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['classrooms'] });
        },
    });

    return {
        classrooms,
        isLoading,
        error,
        createClassroom: createMutation.mutateAsync,
        updateClassroom: updateMutation.mutateAsync,
        deleteClassroom: deleteMutation.mutateAsync,
    };
};