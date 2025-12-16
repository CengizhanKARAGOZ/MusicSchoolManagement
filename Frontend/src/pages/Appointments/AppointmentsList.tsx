import { useState } from 'react';
import {
    Box,
    Button,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    IconButton,
    Chip,
    CircularProgress,
    Alert,
    Typography,
} from '@mui/material';
import {
    Add as AddIcon,
    Edit as EditIcon,
    Delete as DeleteIcon,
    Cancel as CancelIcon,
} from '@mui/icons-material';
import { useAppointments } from '../../hooks/useAppointments';
import { AppointmentFormDialog } from '../../components/Forms/AppointmentFormDialog';
import type { Appointment } from '../../types/appointment.types';
import { useQueryClient } from '@tanstack/react-query';

export const AppointmentsList = () => {
    const { appointments, isLoading, error, cancelAppointment, deleteAppointment } = useAppointments();
    const queryClient = useQueryClient();
    const [openDialog, setOpenDialog] = useState(false);
    const [selectedAppointment, setSelectedAppointment] = useState<Appointment | null>(null);

    const handleAdd = () => {
        setSelectedAppointment(null);
        setOpenDialog(true);
    };

    const handleEdit = (appointment: Appointment) => {
        setSelectedAppointment(appointment);
        setOpenDialog(true);
    };

    const handleCancel = async (id: number) => {
        if (window.confirm('Are you sure you want to cancel this appointment?')) {
            try {
                await cancelAppointment(id);
            } catch (err) {
                console.error('Cancel failed:', err);
            }
        }
    };

    const handleDelete = async (id: number) => {
        if (window.confirm('Are you sure you want to delete this appointment?')) {
            try {
                await deleteAppointment(id);
            } catch (err) {
                console.error('Delete failed:', err);
            }
        }
    };

    const handleCloseDialog = () => {
        setOpenDialog(false);
        setSelectedAppointment(null);
    };

    const handleSuccess = () => {
        queryClient.invalidateQueries({ queryKey: ['appointments'] });
    };

    const getStatusColor = (status: string) => {
        switch (status) {
            case 'Scheduled':
                return 'primary';
            case 'Completed':
                return 'success';
            case 'Cancelled':
                return 'error';
            case 'Rescheduled':
                return 'warning';
            default:
                return 'default';
        }
    };

    if (isLoading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
                <CircularProgress />
            </Box>
        );
    }

    if (error) {
        return <Alert severity="error">Failed to load appointments</Alert>;
    }

    return (
        <Box sx={{ width: '100%', height: '100%' }}>
            <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Typography variant="h4" fontWeight={600}>
                    Appointments
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={handleAdd}
                    size="large"
                >
                    Add Appointment
                </Button>
            </Box>

            <TableContainer component={Paper} sx={{ boxShadow: 3, width: '100%', overflow: 'auto' }}>
                <Table sx={{ tableLayout: 'fixed', width: '100%' }}>
                    <TableHead>
                        <TableRow sx={{ backgroundColor: '#f5f5f5' }}>
                            <TableCell sx={{ fontWeight: 600, width: '5%' }}>ID</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '15%' }}>Student</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '15%' }}>Teacher</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '12%' }}>Course</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Date</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Time</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Classroom</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '8%' }}>Status</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '15%' }} align="center">Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {appointments && appointments.length > 0 ? (
                            appointments.map((appointment) => (
                                <TableRow
                                    key={appointment.id}
                                    sx={{ '&:hover': { backgroundColor: '#fafafa' } }}
                                >
                                    <TableCell>{appointment.id}</TableCell>
                                    <TableCell sx={{ fontWeight: 500 }}>{appointment.studentName}</TableCell>
                                    <TableCell>{appointment.teacherName}</TableCell>
                                    <TableCell>{appointment.courseName}</TableCell>
                                    <TableCell>{new Date(appointment.appointmentDate).toLocaleDateString('tr-TR')}</TableCell>
                                    <TableCell>
                                        {appointment.startTime} - {appointment.endTime}
                                    </TableCell>
                                    <TableCell>{appointment.classroomName || '-'}</TableCell>
                                    <TableCell>
                                        <Chip
                                            label={appointment.status}
                                            color={getStatusColor(appointment.status)}
                                            size="small"
                                            sx={{ fontWeight: 500 }}
                                        />
                                    </TableCell>
                                    <TableCell align="center">
                                        <Box sx={{ display: 'flex', gap: 0.5, justifyContent: 'center' }}>
                                            {appointment.status === 'Scheduled' && (
                                                <>
                                                    <IconButton
                                                        color="primary"
                                                        size="small"
                                                        onClick={() => handleEdit(appointment)}
                                                        title="Edit"
                                                    >
                                                        <EditIcon fontSize="small" />
                                                    </IconButton>
                                                    <IconButton
                                                        color="warning"
                                                        size="small"
                                                        onClick={() => handleCancel(appointment.id)}
                                                        title="Cancel"
                                                    >
                                                        <CancelIcon fontSize="small" />
                                                    </IconButton>
                                                </>
                                            )}
                                            <IconButton
                                                color="error"
                                                size="small"
                                                onClick={() => handleDelete(appointment.id)}
                                                title="Delete"
                                            >
                                                <DeleteIcon fontSize="small" />
                                            </IconButton>
                                        </Box>
                                    </TableCell>
                                </TableRow>
                            ))
                        ) : (
                            <TableRow>
                                <TableCell colSpan={9} align="center" sx={{ py: 8 }}>
                                    <Typography variant="h6" color="text.secondary" gutterBottom>
                                        No appointments found
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                                        Schedule your first appointment!
                                    </Typography>
                                    <Button
                                        variant="contained"
                                        startIcon={<AddIcon />}
                                        onClick={handleAdd}
                                    >
                                        Add Appointment
                                    </Button>
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </TableContainer>

            <AppointmentFormDialog
                open={openDialog}
                onClose={handleCloseDialog}
                appointment={selectedAppointment}
                onSuccess={handleSuccess}
            />
        </Box>
    );
};