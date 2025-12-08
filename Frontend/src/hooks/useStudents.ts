import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { studentApi } from '../api/studentApi';
import type { UpdateStudentDto } from '../types/student.types';

export const useStudents = () => {
    const queryClient = useQueryClient();

    const { data: students, isLoading, error } = useQuery({
        queryKey: ['students'],
        queryFn: studentApi.getAll,
    });

    const { data: activeStudents } = useQuery({
        queryKey: ['students', 'active'],
        queryFn: studentApi.getActive,
    });

    const createMutation = useMutation({
        mutationFn: studentApi.create,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['students'] });
        },
    });

    const updateMutation = useMutation({
        mutationFn: ({ id, data }: { id: number; data: UpdateStudentDto }) =>
            studentApi.update(id, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['students'] });
        },
    });

    const deleteMutation = useMutation({
        mutationFn: studentApi.delete,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['students'] });
        },
    });

    return {
        students,
        activeStudents,
        isLoading,
        error,
        createStudent: createMutation.mutateAsync,
        updateStudent: updateMutation.mutateAsync,
        deleteStudent: deleteMutation.mutateAsync,
    };
};