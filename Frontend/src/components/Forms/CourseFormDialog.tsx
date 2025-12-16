import { useEffect, useState } from 'react';
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
    MenuItem,
    FormControl,
    InputLabel,
    Select,
} from '@mui/material';
import { useForm, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { useSnackbar } from 'notistack';
import type { Course } from '../../types/course.types';
import { useCourses } from '../../hooks/useCourses';
import axiosInstance from '../../api/axios.config';

const schema = yup.object({
    name: yup.string().required('Name is required').max(100),
    description: yup.string().max(500),
    instrumentId: yup.number().nullable(),
    duration: yup.number().required('Duration is required').min(15, 'Minimum 15 minutes').max(240, 'Maximum 240 minutes'),
    price: yup.number().required('Price is required').min(0, 'Price must be positive'),
    isActive: yup.boolean(),
});

interface CourseFormDialogProps {
    open: boolean;
    onClose: () => void;
    course: Course | null;
    onSuccess: () => void;
}

interface Instrument {
    id: number;
    name: string;
}

export const CourseFormDialog = ({ open, onClose, course, onSuccess }: CourseFormDialogProps) => {
    const { enqueueSnackbar } = useSnackbar();
    const { createCourse, updateCourse } = useCourses();
    const isEdit = !!course;

    const [instruments, setInstruments] = useState<Instrument[]>([]);
    const [loading, setLoading] = useState(false);

    const {
        control,
        handleSubmit,
        reset,
        formState: { errors, isSubmitting },
    } = useForm({
        resolver: yupResolver(schema),
        defaultValues: {
            name: '',
            description: '',
            instrumentId: null as number | null,
            duration: 60,
            price: 0,
            isActive: true,
        },
    });

    // Load instruments
    useEffect(() => {
        const loadInstruments = async () => {
            setLoading(true);
            try {
                const response = await axiosInstance.get('/Instruments');
                setInstruments(response.data.data);
            } catch (error) {
                console.error('Failed to load instruments:', error);
            } finally {
                setLoading(false);
            }
        };

        if (open) {
            loadInstruments();
        }
    }, [open]);

    useEffect(() => {
        if (course) {
            reset({
                name: course.name,
                description: course.description || '',
                instrumentId: course.instrumentId || null,
                duration: course.duration,
                price: course.price,
                isActive: course.isActive,
            });
        } else {
            reset({
                name: '',
                description: '',
                instrumentId: null,
                duration: 60,
                price: 0,
                isActive: true,
            });
        }
    }, [course, reset]);

    const onSubmit = async (data: any) => {
        try {
            if (isEdit) {
                await updateCourse({ id: course.id, data });
                enqueueSnackbar('Course updated successfully!', { variant: 'success' });
            } else {
                await createCourse(data);
                enqueueSnackbar('Course created successfully!', { variant: 'success' });
            }
            onSuccess();
            onClose();
        } catch (error: any) {
            enqueueSnackbar(error.response?.data?.message || 'Operation failed', { variant: 'error' });
        }
    };

    return (
        <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
            <DialogTitle>{isEdit ? 'Edit Course' : 'Add New Course'}</DialogTitle>
            <form onSubmit={handleSubmit(onSubmit)}>
                <DialogContent>
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                        <Controller
                            name="name"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Course Name"
                                    fullWidth
                                    error={!!errors.name}
                                    helperText={errors.name?.message}
                                    placeholder="e.g., Piano Beginner Class"
                                />
                            )}
                        />

                        <Controller
                            name="description"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Description"
                                    fullWidth
                                    multiline
                                    rows={3}
                                    error={!!errors.description}
                                    helperText={errors.description?.message}
                                    placeholder="Course description..."
                                />
                            )}
                        />

                        <Controller
                            name="instrumentId"
                            control={control}
                            render={({ field }) => (
                                <FormControl fullWidth>
                                    <InputLabel>Instrument (Optional)</InputLabel>
                                    <Select {...field} label="Instrument (Optional)" disabled={loading}>
                                        <MenuItem value="">None</MenuItem>
                                        {instruments.map((instrument) => (
                                            <MenuItem key={instrument.id} value={instrument.id}>
                                                {instrument.name}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                            )}
                        />

                        <Box sx={{ display: 'flex', gap: 2 }}>
                            <Controller
                                name="duration"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label="Duration (minutes)"
                                        type="number"
                                        fullWidth
                                        error={!!errors.duration}
                                        helperText={errors.duration?.message}
                                    />
                                )}
                            />

                            <Controller
                                name="price"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label="Price (₺)"
                                        type="number"
                                        fullWidth
                                        error={!!errors.price}
                                        helperText={errors.price?.message}
                                    />
                                )}
                            />
                        </Box>

                        <Controller
                            name="isActive"
                            control={control}
                            render={({ field }) => (
                                <FormControlLabel
                                    control={<Switch {...field} checked={field.value} />}
                                    label="Active"
                                />
                            )}
                        />
                    </Box>
                </DialogContent>
                <DialogActions>
                    <Button onClick={onClose}>Cancel</Button>
                    <Button type="submit" variant="contained" disabled={isSubmitting || loading}>
                        {isSubmitting ? 'Saving...' : isEdit ? 'Update' : 'Create'}
                    </Button>
                </DialogActions>
            </form>
        </Dialog>
    );
};