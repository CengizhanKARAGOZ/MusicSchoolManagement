import { useEffect } from 'react';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    TextField,
    Box,
    Typography,
} from '@mui/material';
import { useForm, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { useSnackbar } from 'notistack';
import type { Teacher } from '../../types/teacher.types';
import { useTeachers } from '../../hooks/useTeachers';

const schema = yup.object({
    specializations: yup.string().max(200),
    hourlyRate: yup.number().min(0),
    biography: yup.string(),
    availabilityNotes: yup.string(),
});

interface TeacherEditDialogProps {
    open: boolean;
    onClose: () => void;
    teacher: Teacher | null;
    onSuccess: () => void;
}

export const TeacherEditDialog = ({ open, onClose, teacher, onSuccess }: TeacherEditDialogProps) => {
    const { enqueueSnackbar } = useSnackbar();
    const { updateTeacher } = useTeachers();

    const {
        control,
        handleSubmit,
        reset,
        formState: { errors, isSubmitting },
    } = useForm({
        resolver: yupResolver(schema),
        defaultValues: {
            specializations: '',
            hourlyRate: 0,
            biography: '',
            availabilityNotes: '',
        },
    });

    useEffect(() => {
        if (teacher) {
            reset({
                specializations: teacher.specializations || '',
                hourlyRate: teacher.hourlyRate || 0,
                biography: teacher.biography || '',
                availabilityNotes: teacher.availabilityNotes || '',
            });
        }
    }, [teacher, reset]);

    const onSubmit = async (data: any) => {
        if (!teacher) return;

        try {
            await updateTeacher({ id: teacher.id, data });
            enqueueSnackbar('Teacher updated successfully!', { variant: 'success' });
            onSuccess();
            onClose();
        } catch (error: any) {
            enqueueSnackbar(error.response?.data?.message || 'Failed to update teacher', { variant: 'error' });
        }
    };

    if (!teacher) return null;

    return (
        <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
            <DialogTitle>Edit Teacher</DialogTitle>
            <form onSubmit={handleSubmit(onSubmit)}>
                <DialogContent>
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                        <Box sx={{ p: 2, backgroundColor: '#f5f5f5', borderRadius: 1 }}>
                            <Typography variant="body2" color="text.secondary">
                                Teacher: {teacher.user?.firstName} {teacher.user?.lastName}
                            </Typography>
                            <Typography variant="body2" color="text.secondary">
                                Email: {teacher.user?.email}
                            </Typography>
                        </Box>

                        <Controller
                            name="specializations"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Specializations"
                                    fullWidth
                                    placeholder="e.g., Piano, Guitar, Violin"
                                    error={!!errors.specializations}
                                    helperText={errors.specializations?.message}
                                />
                            )}
                        />

                        <Controller
                            name="hourlyRate"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Hourly Rate (₺)"
                                    type="number"
                                    fullWidth
                                    error={!!errors.hourlyRate}
                                    helperText={errors.hourlyRate?.message}
                                />
                            )}
                        />

                        <Controller
                            name="biography"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Biography"
                                    fullWidth
                                    multiline
                                    rows={4}
                                    error={!!errors.biography}
                                    helperText={errors.biography?.message}
                                />
                            )}
                        />

                        <Controller
                            name="availabilityNotes"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Availability Notes"
                                    fullWidth
                                    multiline
                                    rows={2}
                                    placeholder="e.g., Available Mon-Fri, 9am-5pm"
                                    error={!!errors.availabilityNotes}
                                    helperText={errors.availabilityNotes?.message}
                                />
                            )}
                        />
                    </Box>
                </DialogContent>
                <DialogActions>
                    <Button onClick={onClose}>Cancel</Button>
                    <Button type="submit" variant="contained" disabled={isSubmitting}>
                        {isSubmitting ? 'Saving...' : 'Update'}
                    </Button>
                </DialogActions>
            </form>
        </Dialog>
    );
};