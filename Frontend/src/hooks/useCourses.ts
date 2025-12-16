import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { courseApi } from '../api/courseApi';
import type { UpdateCourseDto } from '../types/course.types';

export const useCourses = () => {
    const queryClient = useQueryClient();

    const { data: courses, isLoading, error } = useQuery({
        queryKey: ['courses'],
        queryFn: courseApi.getAll,
    });

    const createMutation = useMutation({
        mutationFn: courseApi.create,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['courses'] });
        },
    });

    const updateMutation = useMutation({
        mutationFn: ({ id, data }: { id: number; data: UpdateCourseDto }) =>
            courseApi.update(id, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['courses'] });
        },
    });

    const deleteMutation = useMutation({
        mutationFn: courseApi.delete,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['courses'] });
        },
    });

    return {
        courses,
        isLoading,
        error,
        createCourse: createMutation.mutateAsync,
        updateCourse: updateMutation.mutateAsync,
        deleteCourse: deleteMutation.mutateAsync,
    };
};