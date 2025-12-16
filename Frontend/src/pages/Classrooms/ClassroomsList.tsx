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
import { useClassrooms } from '../../hooks/useClassrooms';
import { ClassroomFormDialog } from '../../components/Forms/ClassroomFormDialog';
import type { Classroom } from '../../types/classroom.types';
import { useQueryClient } from '@tanstack/react-query';

export const ClassroomsList = () => {
    const { classrooms, isLoading, error, deleteClassroom } = useClassrooms();
    const queryClient = useQueryClient();
    const [openDialog, setOpenDialog] = useState(false);
    const [selectedClassroom, setSelectedClassroom] = useState<Classroom | null>(null);

    const handleAdd = () => {
        setSelectedClassroom(null);
        setOpenDialog(true);
    };

    const handleEdit = (classroom: Classroom) => {
        setSelectedClassroom(classroom);
        setOpenDialog(true);
    };

    const handleDelete = async (id: number) => {
        if (window.confirm('Are you sure you want to delete this classroom?')) {
            try {
                await deleteClassroom(id);
            } catch (err) {
                console.error('Delete failed:', err);
            }
        }
    };

    const handleCloseDialog = () => {
        setOpenDialog(false);
        setSelectedClassroom(null);
    };

    const handleSuccess = () => {
        queryClient.invalidateQueries({ queryKey: ['classrooms'] });
    };

    if (isLoading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
                <CircularProgress />
            </Box>
        );
    }

    if (error) {
        return <Alert severity="error">Failed to load classrooms</Alert>;
    }

    return (
        <Box sx={{ width: '100%', height: '100%' }}>
            <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Typography variant="h4" fontWeight={600}>
                    Classrooms
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={handleAdd}
                    size="large"
                >
                    Add Classroom
                </Button>
            </Box>

            <TableContainer component={Paper} sx={{ boxShadow: 3, width: '100%', overflow: 'auto' }}>
                <Table sx={{ tableLayout: 'fixed', width: '100%' }}>
                    <TableHead>
                        <TableRow sx={{ backgroundColor: '#f5f5f5' }}>
                            <TableCell sx={{ fontWeight: 600, width: '5%' }}>ID</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '20%' }}>Name</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Capacity</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '20%' }}>Location</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '30%' }}>Equipment</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '8%' }}>Status</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }} align="center">Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {classrooms && classrooms.length > 0 ? (
                            classrooms.map((classroom) => (
                                <TableRow
                                    key={classroom.id}
                                    sx={{ '&:hover': { backgroundColor: '#fafafa' } }}
                                >
                                    <TableCell>{classroom.id}</TableCell>
                                    <TableCell sx={{ fontWeight: 500 }}>{classroom.name}</TableCell>
                                    <TableCell>{classroom.capacity} people</TableCell>
                                    <TableCell>{classroom.location || '-'}</TableCell>
                                    <TableCell
                                        sx={{
                                            overflow: 'hidden',
                                            textOverflow: 'ellipsis',
                                            whiteSpace: 'nowrap',
                                            maxWidth: 200
                                        }}
                                        title={classroom.equipment || ''}
                                    >
                                        {classroom.equipment || '-'}
                                    </TableCell>
                                    <TableCell>
                                        <Chip
                                            label={classroom.isAvailable ? 'Available' : 'Unavailable'}
                                            color={classroom.isAvailable ? 'success' : 'default'}
                                            size="small"
                                            sx={{ fontWeight: 500 }}
                                        />
                                    </TableCell>
                                    <TableCell align="center">
                                        <Box sx={{ display: 'flex', gap: 0.5, justifyContent: 'center' }}>
                                            <IconButton
                                                color="primary"
                                                size="small"
                                                onClick={() => handleEdit(classroom)}
                                            >
                                                <EditIcon fontSize="small" />
                                            </IconButton>
                                            <IconButton
                                                color="error"
                                                size="small"
                                                onClick={() => handleDelete(classroom.id)}
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
                                        No classrooms found
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                                        Add your first classroom to get started!
                                    </Typography>
                                    <Button
                                        variant="contained"
                                        startIcon={<AddIcon />}
                                        onClick={handleAdd}
                                    >
                                        Add Classroom
                                    </Button>
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </TableContainer>

            <ClassroomFormDialog
                open={openDialog}
                onClose={handleCloseDialog}
                classroom={selectedClassroom}
                onSuccess={handleSuccess}
            />
        </Box>
    );
};