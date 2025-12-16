import { useEffect } from 'react';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    TextField,
    Box,
    FormControlLabel,
    Switch,
} from '@mui/material';
import { useForm, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { useSnackbar } from 'notistack';
import type { Classroom } from '../../types/classroom.types';
import { useClassrooms } from '../../hooks/useClassrooms';

const schema = yup.object({
    name: yup.string().required('Name is required').max(100),
    capacity: yup.number().required('Capacity is required').min(1, 'Minimum 1 person').max(100, 'Maximum 100 people'),
    location: yup.string().max(200),
    equipment: yup.string().max(500),
    isAvailable: yup.boolean(),
});

interface ClassroomFormDialogProps {
    open: boolean;
    onClose: () => void;
    classroom: Classroom | null;
    onSuccess: () => void;
}

export const ClassroomFormDialog = ({ open, onClose, classroom, onSuccess }: ClassroomFormDialogProps) => {
    const { enqueueSnackbar } = useSnackbar();
    const { createClassroom, updateClassroom } = useClassrooms();
    const isEdit = !!classroom;

    const {
        control,
        handleSubmit,
        reset,
        formState: { errors, isSubmitting },
    } = useForm({
        resolver: yupResolver(schema),
        defaultValues: {
            name: '',
            capacity: 10,
            location: '',
            equipment: '',
            isAvailable: true,
        },
    });

    useEffect(() => {
        if (classroom) {
            reset({
                name: classroom.name,
                capacity: classroom.capacity,
                location: classroom.location || '',
                equipment: classroom.equipment || '',
                isAvailable: classroom.isAvailable,
            });
        } else {
            reset({
                name: '',
                capacity: 10,
                location: '',
                equipment: '',
                isAvailable: true,
            });
        }
    }, [classroom, reset]);

    const onSubmit = async (data: any) => {
        try {
            if (isEdit) {
                await updateClassroom({ id: classroom.id, data });
                enqueueSnackbar('Classroom updated successfully!', { variant: 'success' });
            } else {
                await createClassroom(data);
                enqueueSnackbar('Classroom created successfully!', { variant: 'success' });
            }
            onSuccess();
            onClose();
        } catch (error: any) {
            enqueueSnackbar(error.response?.data?.message || 'Operation failed', { variant: 'error' });
        }
    };

    return (
        <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
            <DialogTitle>{isEdit ? 'Edit Classroom' : 'Add New Classroom'}</DialogTitle>
            <form onSubmit={handleSubmit(onSubmit)}>
                <DialogContent>
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                        <Controller
                            name="name"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Classroom Name"
                                    fullWidth
                                    error={!!errors.name}
                                    helperText={errors.name?.message}
                                    placeholder="e.g., Room A1, Studio 2"
                                />
                            )}
                        />

                        <Controller
                            name="capacity"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Capacity (people)"
                                    type="number"
                                    fullWidth
                                    error={!!errors.capacity}
                                    helperText={errors.capacity?.message}
                                />
                            )}
                        />

                        <Controller
                            name="location"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Location"
                                    fullWidth
                                    error={!!errors.location}
                                    helperText={errors.location?.message}
                                    placeholder="e.g., 2nd Floor, Building A"
                                />
                            )}
                        />

                        <Controller
                            name="equipment"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Equipment"
                                    fullWidth
                                    multiline
                                    rows={3}
                                    error={!!errors.equipment}
                                    helperText={errors.equipment?.message}
                                    placeholder="e.g., Piano, Whiteboard, Sound System"
                                />
                            )}
                        />

                        <Controller
                            name="isAvailable"
                            control={control}
                            render={({ field }) => (
                                <FormControlLabel
                                    control={<Switch {...field} checked={field.value} />}
                                    label="Available"
                                />
                            )}
                        />
                    </Box>
                </DialogContent>
                <DialogActions>
                    <Button onClick={onClose}>Cancel</Button>
                    <Button type="submit" variant="contained" disabled={isSubmitting}>
                        {isSubmitting ? 'Saving...' : isEdit ? 'Update' : 'Create'}
                    </Button>
                </DialogActions>
            </form>
        </Dialog>
    );
};