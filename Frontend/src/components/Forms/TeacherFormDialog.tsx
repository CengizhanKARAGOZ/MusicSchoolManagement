import { useState } from 'react';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    TextField,
    Box,
    Alert,
    IconButton,
    InputAdornment,
} from '@mui/material';
import { ContentCopy as CopyIcon } from '@mui/icons-material';
import { useForm, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { useSnackbar } from 'notistack';
import axios from '../api/axios.config';

const schema = yup.object({
    firstName: yup.string().required('First name is required').max(100),
    lastName: yup.string().required('Last name is required').max(100),
    email: yup.string().required('Email is required').email(),
    phoneNumber: yup.string().required('Phone number is required'),
    specializations: yup.string().max(200),
    hourlyRate: yup.number().min(0),
    biography: yup.string(),
    availabilityNotes: yup.string(),
});

interface CreateTeacherDialogProps {
    open: boolean;
    onClose: () => void;
    onSuccess: () => void;
}

export const CreateTeacherDialog = ({ open, onClose, onSuccess }: CreateTeacherDialogProps) => {
    const { enqueueSnackbar } = useSnackbar();
    const [generatedPassword, setGeneratedPassword] = useState<string | null>(null);

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
            specializations: '',
            hourlyRate: 0,
            biography: '',
            availabilityNotes: '',
        },
    });

    const onSubmit = async (data: any) => {
        try {
            const response = await axios.post('/Teachers', data);
            const password = response.data.data.temporaryPassword;

            setGeneratedPassword(password);
            enqueueSnackbar('Teacher created successfully!', { variant: 'success' });
            onSuccess();
        } catch (error: any) {
            enqueueSnackbar(error.response?.data?.message || 'Failed to create teacher', { variant: 'error' });
        }
    };

    const handleCopyPassword = () => {
        if (generatedPassword) {
            navigator.clipboard.writeText(generatedPassword);
            enqueueSnackbar('Password copied to clipboard!', { variant: 'success' });
        }
    };

    const handleClose = () => {
        reset();
        setGeneratedPassword(null);
        onClose();
    };

    if (generatedPassword) {
        return (
            <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
                <DialogTitle>Teacher Created Successfully! 🎉</DialogTitle>
                <DialogContent>
                    <Alert severity="warning" sx={{ mb: 2 }}>
                        <strong>Important:</strong> Save this password! It will only be shown once.
                    </Alert>

                    <TextField
                        fullWidth
                        label="Temporary Password"
                        value={generatedPassword}
                        InputProps={{
                            readOnly: true,
                            endAdornment: (
                                <InputAdornment position="end">
                                    <IconButton onClick={handleCopyPassword} edge="end">
                                        <CopyIcon />
                                    </IconButton>
                                </InputAdornment>
                            ),
                        }}
                        sx={{ mb: 2 }}
                    />

                    <Alert severity="info">
                        The teacher must change this password on their first login.
                    </Alert>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose} variant="contained">
                        Done
                    </Button>
                </DialogActions>
            </Dialog>
        );
    }

    return (
        <Dialog open={open} onClose={handleClose} maxWidth="md" fullWidth>
            <DialogTitle>Add New Teacher</DialogTitle>
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

                        <Box sx={{ display: 'flex', gap: 2 }}>
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
                        </Box>

                        <Controller
                            name="biography"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Biography"
                                    fullWidth
                                    multiline
                                    rows={3}
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
                                    error={!!errors.availabilityNotes}
                                    helperText={errors.availabilityNotes?.message}
                                />
                            )}
                        />
                    </Box>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Cancel</Button>
                    <Button type="submit" variant="contained" disabled={isSubmitting}>
                        {isSubmitting ? 'Creating...' : 'Create Teacher'}
                    </Button>
                </DialogActions>
            </form>
        </Dialog>
    );
};