import { useState } from 'react';
import {
    Box,
    Paper,
    TextField,
    Button,
    Typography,
    Alert,
} from '@mui/material';
import { useForm, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { useNavigate } from 'react-router-dom';
import { useSnackbar } from 'notistack';
import axiosInstance from '../../api/axios.config';
import { useAuthStore } from '../../store/authStore';

const schema = yup.object({
    oldPassword: yup.string().required('Current password is required'),
    newPassword: yup.string()
        .required('New password is required')
        .min(8, 'Password must be at least 8 characters')
        .matches(/[A-Z]/, 'Password must contain at least one uppercase letter')
        .matches(/[a-z]/, 'Password must contain at least one lowercase letter')
        .matches(/[0-9]/, 'Password must contain at least one number')
        .matches(/[!@#$%^&*._-]/, 'Password must contain at least one special character (!@#$%^&*._-)'),
    confirmPassword: yup.string()
        .required('Please confirm your password')
        .oneOf([yup.ref('newPassword')], 'Passwords must match'),
});

export const ChangePassword = () => {
    const navigate = useNavigate();
    const { enqueueSnackbar } = useSnackbar();
    const { user, updateUser } = useAuthStore();
    const [error, setError] = useState('');

    const {
        control,
        handleSubmit,
        formState: { errors, isSubmitting },
    } = useForm({
        resolver: yupResolver(schema),
        defaultValues: {
            oldPassword: '',
            newPassword: '',
            confirmPassword: '',
        },
    });

    const onSubmit = async (data: any) => {
        try {
            setError('');
            await axiosInstance.post('/Auth/change-password', {
                oldPassword: data.oldPassword,
                newPassword: data.newPassword,
            });

            enqueueSnackbar('Password changed successfully!', { variant: 'success' });

            // Update user state (remove passwordChangeRequired flag)
            if (user) {
                updateUser({ ...user, passwordChangeRequired: false });
            }

            navigate('/dashboard');
        } catch (err: any) {
            setError(err.response?.data?.message || 'Failed to change password');
        }
    };

    return (
        <Box
            sx={{
                minHeight: '100vh',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                backgroundColor: '#f5f5f5',
            }}
        >
            <Paper
                elevation={3}
                sx={{
                    p: 4,
                    maxWidth: 450,
                    width: '100%',
                }}
            >
                <Typography variant="h4" align="center" gutterBottom fontWeight={600}>
                    Change Password
                </Typography>

                <Alert severity="warning" sx={{ mb: 3 }}>
                    You must change your temporary password before continuing.
                </Alert>

                {error && (
                    <Alert severity="error" sx={{ mb: 2 }}>
                        {error}
                    </Alert>
                )}

                <form onSubmit={handleSubmit(onSubmit)}>
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                        <Controller
                            name="oldPassword"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Current Password"
                                    type="password"
                                    fullWidth
                                    error={!!errors.oldPassword}
                                    helperText={errors.oldPassword?.message}
                                />
                            )}
                        />

                        <Controller
                            name="newPassword"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="New Password"
                                    type="password"
                                    fullWidth
                                    error={!!errors.newPassword}
                                    helperText={errors.newPassword?.message}
                                />
                            )}
                        />

                        <Controller
                            name="confirmPassword"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Confirm New Password"
                                    type="password"
                                    fullWidth
                                    error={!!errors.confirmPassword}
                                    helperText={errors.confirmPassword?.message}
                                />
                            )}
                        />

                        <Button
                            type="submit"
                            variant="contained"
                            fullWidth
                            size="large"
                            disabled={isSubmitting}
                        >
                            {isSubmitting ? 'Changing...' : 'Change Password'}
                        </Button>
                    </Box>
                </form>
            </Paper>
        </Box>
    );
};