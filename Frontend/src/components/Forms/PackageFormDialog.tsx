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
import type { Package } from '../../types/package.types';
import { usePackages } from '../../hooks/usePackages';

const schema = yup.object({
    name: yup.string().required('Name is required').max(100),
    description: yup.string().max(500),
    lessonCount: yup.number().required('Lesson count is required').min(1, 'Minimum 1 lesson').max(100, 'Maximum 100 lessons'),
    price: yup.number().required('Price is required').min(0, 'Price must be positive'),
    validityDays: yup.number().required('Validity is required').min(1, 'Minimum 1 day').max(365, 'Maximum 365 days'),
    isActive: yup.boolean(),
});

interface PackageFormDialogProps {
    open: boolean;
    onClose: () => void;
    package: Package | null;
    onSuccess: () => void;
}

export const PackageFormDialog = ({ open, onClose, package: pkg, onSuccess }: PackageFormDialogProps) => {
    const { enqueueSnackbar } = useSnackbar();
    const { createPackage, updatePackage } = usePackages();
    const isEdit = !!pkg;

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
            lessonCount: 8,
            price: 0,
            validityDays: 30,
            isActive: true,
        },
    });

    useEffect(() => {
        if (pkg) {
            reset({
                name: pkg.name,
                description: pkg.description || '',
                lessonCount: pkg.lessonCount,
                price: pkg.price,
                validityDays: pkg.validityDays,
                isActive: pkg.isActive,
            });
        } else {
            reset({
                name: '',
                description: '',
                lessonCount: 8,
                price: 0,
                validityDays: 30,
                isActive: true,
            });
        }
    }, [pkg, reset]);

    const onSubmit = async (data: any) => {
        try {
            if (isEdit) {
                await updatePackage({ id: pkg.id, data });
                enqueueSnackbar('Package updated successfully!', { variant: 'success' });
            } else {
                await createPackage(data);
                enqueueSnackbar('Package created successfully!', { variant: 'success' });
            }
            onSuccess();
            onClose();
        } catch (error: any) {
            enqueueSnackbar(error.response?.data?.message || 'Operation failed', { variant: 'error' });
        }
    };

    return (
        <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
            <DialogTitle>{isEdit ? 'Edit Package' : 'Add New Package'}</DialogTitle>
            <form onSubmit={handleSubmit(onSubmit)}>
                <DialogContent>
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                        <Controller
                            name="name"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Package Name"
                                    fullWidth
                                    error={!!errors.name}
                                    helperText={errors.name?.message}
                                    placeholder="e.g., Basic Package, Premium Package"
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
                                    placeholder="Package description..."
                                />
                            )}
                        />

                        <Box sx={{ display: 'flex', gap: 2 }}>
                            <Controller
                                name="lessonCount"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label="Lesson Count"
                                        type="number"
                                        fullWidth
                                        error={!!errors.lessonCount}
                                        helperText={errors.lessonCount?.message}
                                    />
                                )}
                            />

                            <Controller
                                name="validityDays"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label="Validity (days)"
                                        type="number"
                                        fullWidth
                                        error={!!errors.validityDays}
                                        helperText={errors.validityDays?.message}
                                    />
                                )}
                            />
                        </Box>

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
                    <Button type="submit" variant="contained" disabled={isSubmitting}>
                        {isSubmitting ? 'Saving...' : isEdit ? 'Update' : 'Create'}
                    </Button>
                </DialogActions>
            </form>
        </Dialog>
    );
};