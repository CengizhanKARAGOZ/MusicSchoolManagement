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
} from '@mui/icons-material';
import { useTeachers } from '../../hooks/useTeachers';
import { TeacherFormDialog } from '../../components/Forms/TeacherFormDialog';
import { useAuthStore } from '../../store/authStore';
import { useQueryClient } from '@tanstack/react-query';
import type {Teacher} from "../../types/teacher.types.ts";
import {TeacherEditDialog} from "../../components/Forms/TeacherEditDialog.tsx";

export const TeachersList = () => {
    const { teachers, isLoading, error, deleteTeacher } = useTeachers();
    const { user } = useAuthStore();
    const queryClient = useQueryClient();
    const [openDialog, setOpenDialog] = useState(false);
    const [openEditDialog, setOpenEditDialog] = useState(false);
    const [selectedTeacher, setSelectedTeacher] = useState<Teacher | null>(null);

    const handleAdd = () => {
        setOpenDialog(true);
    };
    
    const handleEdit = (teacher: Teacher) => {
        setSelectedTeacher(teacher);
        setOpenEditDialog(true);
    }

    const handleDelete = async (id: number) => {
        if (window.confirm('Are you sure you want to delete this teacher?')) {
            try {
                await deleteTeacher(id);
            } catch (err) {
                console.error('Delete failed:', err);
            }
        }
    };

    const handleCloseDialog = () => {
        setOpenDialog(false);
    };
    
    const handleCloseEditDialog = () => {
        setOpenEditDialog(false);
        setSelectedTeacher(null);
    }

    const handleSuccess = () => {
        queryClient.invalidateQueries({ queryKey: ['teachers'] });
    };

    if (isLoading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
                <CircularProgress />
            </Box>
        );
    }

    if (error) {
        return <Alert severity="error">Failed to load teachers</Alert>;
    }

    return (
        <Box sx={{ width: '100%', height: '100%' }}>
            <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Box>
                    <Typography variant="h4" fontWeight={600}>
                        Teachers
                    </Typography>
                    <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                        Admin can create teacher accounts with temporary passwords
                    </Typography>
                </Box>
                {user?.role === 'Admin' && (
                    <Button
                        variant="contained"
                        startIcon={<AddIcon />}
                        onClick={handleAdd}
                        size="large"
                    >
                        Add Teacher
                    </Button>
                )}
            </Box>

            <TableContainer component={Paper} sx={{ boxShadow: 3, width: '100%', overflow: 'auto' }}>
                <Table sx={{ tableLayout: 'fixed', width: '100%' }}>
                    <TableHead>
                        <TableRow sx={{ backgroundColor: '#f5f5f5' }}>
                            <TableCell sx={{ fontWeight: 600, width: '5%' }}>ID</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '20%' }}>Name</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '20%' }}>Specialization</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '12%' }}>Hourly Rate</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '25%' }}>Biography</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '8%' }}>Status</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }} align="center">Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {teachers && teachers.length > 0 ? (
                            teachers.map((teacher) => (
                                <TableRow
                                    key={teacher.id}
                                    sx={{ '&:hover': { backgroundColor: '#fafafa' } }}
                                >
                                    <TableCell>{teacher.id}</TableCell>
                                    <TableCell sx={{ fontWeight: 500 }}>
                                        {teacher.user ? `${teacher.user.firstName} ${teacher.user.lastName}` : 'N/A'}
                                    </TableCell>
                                    <TableCell>{teacher.specializations || '-'}</TableCell>
                                    <TableCell>{teacher.hourlyRate ? `₺${teacher.hourlyRate.toLocaleString()}` : '-'}</TableCell>
                                    <TableCell
                                        sx={{
                                            overflow: 'hidden',
                                            textOverflow: 'ellipsis',
                                            whiteSpace: 'nowrap',
                                            maxWidth: 200
                                        }}
                                        title={teacher.biography || ''}
                                    >
                                        {teacher.biography || '-'}
                                    </TableCell>
                                    <TableCell>
                                        <Chip
                                            label={teacher.user?.isActive ? 'Active' : 'Inactive'}
                                            color={teacher.user?.isActive ? 'success' : 'default'}
                                            size="small"
                                            sx={{ fontWeight: 500 }}
                                        />
                                    </TableCell>
                                    <TableCell align="center">
                                        <Box sx={{ display: 'flex', gap: 0.5, justifyContent: 'center' }}>
                                            <IconButton
                                                color="primary"
                                                size="small"
                                                onClick={() => handleEdit(teacher)}
                                            >
                                                <EditIcon fontSize="small" />
                                            </IconButton>
                                        <IconButton
                                            color="error"
                                            size="small"
                                            onClick={() => handleDelete(teacher.id)}
                                        >
                                            <DeleteIcon fontSize="small" />
                                        </IconButton>
                                            </Box>
                                    </TableCell>
                                </TableRow>
                            ))
                        ) : (
                            <TableRow>
                                <TableCell colSpan={7} align="center" sx={{ py: 8 }}>
                                    <Typography variant="h6" color="text.secondary" gutterBottom>
                                        No teachers found
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                                        Add your first teacher to get started!
                                    </Typography>
                                    {user?.role === 'Admin' && (
                                        <Button
                                            variant="contained"
                                            startIcon={<AddIcon />}
                                            onClick={handleAdd}
                                        >
                                            Add Teacher
                                        </Button>
                                    )}
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </TableContainer>

            <TeacherFormDialog
                open={openDialog}
                onClose={handleCloseDialog}
                onSuccess={handleSuccess}
            />
            <TeacherEditDialog
                open={openEditDialog}
                onClose={handleCloseEditDialog}
                teacher={selectedTeacher}
                onSuccess={handleSuccess}
            />
            
        </Box>
    );
};