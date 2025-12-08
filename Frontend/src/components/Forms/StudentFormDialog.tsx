import { useEffect } from 'react';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    TextField,
    Box,
} from '@mui/material';
import { useForm, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { useStudents } from '../../hooks/useStudents';
import type { Student, CreateStudentDto, UpdateStudentDto } from '../../types/student.types';
import { useSnackbar } from 'notistack';

interface StudentFormDialogProps {
    open: boolean;
    onClose: () => void;
    student: Student | null;
}

const schema = yup.object({
    firstName: yup.string().required('First name is required').max(100),
    lastName: yup.string().required('Last name is required').max(100),
    email: yup.string().required('Email is required').email('Invalid email format'),
    phoneNumber: yup.string().required('Phone number is required'),
    dateOfBirth: yup.string().required('Date of birth is required'),
    parentName: yup.string().max(200),
    parentPhone: yup.string(),
    address: yup.string().max(500),
    registrationDate: yup.string().required('Registration date is required'),
    isActive: yup.boolean(),
});

export const StudentFormDialog = ({ open, onClose, student }: StudentFormDialogProps) => {
    const { createStudent, updateStudent } = useStudents();
    const { enqueueSnackbar } = useSnackbar();
    const isEdit = !!student;

    const {
        control,
        handleSubmit,
        reset,
        formState: { errors, isSubmitting },
    } = useForm({
        resolver: yupResolver(schema),
        defaultValues: {
            firstName: '',
            lastName: '',
            email: '',
            phoneNumber: '',
            dateOfBirth: '',
            parentName: '',
            parentPhone: '',
            address: '',
            registrationDate: new Date().toISOString().split('T')[0],
            isActive: true,
        },
    });

    useEffect(() => {
        if (student) {
            reset({
                firstName: student.firstName,
                lastName: student.lastName,
                email: student.email,
                phoneNumber: student.phoneNumber,
                dateOfBirth: student.dateOfBirth.split('T')[0],
                parentName: student.parentName || '',
                parentPhone: student.parentPhone || '',
                address: student.address || '',
                registrationDate: student.registrationDate.split('T')[0],
                isActive: student.isActive,
            });
        } else {
            reset({
                firstName: '',
                lastName: '',
                email: '',
                phoneNumber: '',
                dateOfBirth: '',
                parentName: '',
                parentPhone: '',
                address: '',
                registrationDate: new Date().toISOString().split('T')[0],
                isActive: true,
            });
        }
    }, [student, reset]);

    const onSubmit = async (data: any) => {
        try {
            if (isEdit) {
                await updateStudent({ id: student.id, data: data as UpdateStudentDto });
                enqueueSnackbar('Student updated successfully', { variant: 'success' });
            } else {
                await createStudent(data as CreateStudentDto);
                enqueueSnackbar('Student created successfully', { variant: 'success' });
            }
            onClose();
        } catch (error: any) {
            enqueueSnackbar(error.message || 'Operation failed', { variant: 'error' });
        }
    };

    return (
        <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
            <DialogTitle>{isEdit ? 'Edit Student' : 'Add New Student'}</DialogTitle>
            <form onSubmit={handleSubmit(onSubmit)}>
                <DialogContent>
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                        <Box sx={{ display: 'flex', gap: 2 }}>
                            <Controller
                                name="firstName"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label="First Name"
                                        fullWidth
                                        error={!!errors.firstName}
                                        helperText={errors.firstName?.message}
                                    />
                                )}
                            />
                            <Controller
                                name="lastName"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label="Last Name"
                                        fullWidth
                                        error={!!errors.lastName}
                                        helperText={errors.lastName?.message}
                                    />
                                )}
                            />
                        </Box>

                        <Controller
                            name="email"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Email"
                                    type="email"
                                    fullWidth
                                    error={!!errors.email}
                                    helperText={errors.email?.message}
                                />
                            )}
                        />

                        <Box sx={{ display: 'flex', gap: 2 }}>
                            <Controller
                                name="phoneNumber"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label="Phone Number"
                                        fullWidth
                                        error={!!errors.phoneNumber}
                                        helperText={errors.phoneNumber?.message}
                                    />
                                )}
                            />
                            <Controller
                                name="dateOfBirth"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label="Date of Birth"
                                        type="date"
                                        fullWidth
                                        InputLabelProps={{ shrink: true }}
                                        error={!!errors.dateOfBirth}
                                        helperText={errors.dateOfBirth?.message}
                                    />
                                )}
                            />
                        </Box>

                        <Box sx={{ display: 'flex', gap: 2 }}>
                            <Controller
                                name="parentName"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label="Parent Name"
                                        fullWidth
                                        error={!!errors.parentName}
                                        helperText={errors.parentName?.message}
                                    />
                                )}
                            />
                            <Controller
                                name="parentPhone"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label="Parent Phone"
                                        fullWidth
                                        error={!!errors.parentPhone}
                                        helperText={errors.parentPhone?.message}
                                    />
                                )}
                            />
                        </Box>

                        <Controller
                            name="address"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Address"
                                    fullWidth
                                    multiline
                                    rows={2}
                                    error={!!errors.address}
                                    helperText={errors.address?.message}
                                />
                            )}
                        />

                        <Controller
                            name="registrationDate"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Registration Date"
                                    type="date"
                                    fullWidth
                                    InputLabelProps={{ shrink: true }}
                                    error={!!errors.registrationDate}
                                    helperText={errors.registrationDate?.message}
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