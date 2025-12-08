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
import { useStudents } from '../../hooks/useStudents';
import { StudentFormDialog } from '../../components/Forms/StudentFormDialog';
import type { Student } from '../../types/student.types';

export const StudentList = () => {
    const { students, isLoading, error, deleteStudent } = useStudents();
    const [openDialog, setOpenDialog] = useState(false);
    const [selectedStudent, setSelectedStudent] = useState<Student | null>(null);

    const handleAdd = () => {
        setSelectedStudent(null);
        setOpenDialog(true);
    };

    const handleEdit = (student: Student) => {
        setSelectedStudent(student);
        setOpenDialog(true);
    };

    const handleDelete = async (id: number) => {
        if (window.confirm('Are you sure you want to delete this student?')) {
            try {
                await deleteStudent(id);
            } catch (err) {
                console.error('Delete failed:', err);
            }
        }
    };

    const handleCloseDialog = () => {
        setOpenDialog(false);
        setSelectedStudent(null);
    };

    if (isLoading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
                <CircularProgress />
            </Box>
        );
    }

    if (error) {
        return <Alert severity="error">Failed to load students</Alert>;
    }

    return (
        <Box sx={{ width: '100%', height: '100%' }}>
            <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Typography variant="h4" fontWeight={600}>
                    Students
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={handleAdd}
                    size="large"
                >
                    Add Student
                </Button>
            </Box>

            <TableContainer component={Paper} sx={{ boxShadow: 3, width: '100%', overflow: 'auto' }}>
                <Table sx={{ tableLayout: 'fixed', width: '100%' }}>
                    <TableHead>
                        <TableRow sx={{ backgroundColor: '#f5f5f5' }}>
                            <TableCell sx={{ fontWeight: 600, width: '5%' }}>ID</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '12%' }}>Name</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '15%' }}>Email</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Phone</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Date of Birth</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Parent Phone</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Registration</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '8%' }}>Status</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }} align="center">Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {students && students.length > 0 ? (
                            students.map((student) => (
                                <TableRow
                                    key={student.id}
                                    sx={{
                                        '&:hover': { backgroundColor: '#fafafa' },
                                    }}
                                >
                                    <TableCell>{student.id}</TableCell>
                                    <TableCell sx={{ fontWeight: 500, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
                                        {`${student.firstName} ${student.lastName}`}
                                    </TableCell>
                                    <TableCell sx={{ overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
                                        {student.email}
                                    </TableCell>
                                    <TableCell>{student.phoneNumber}</TableCell>
                                    <TableCell>{new Date(student.dateOfBirth).toLocaleDateString('tr-TR')}</TableCell>
                                    <TableCell>{student.parentPhone || '-'}</TableCell>
                                    <TableCell>{new Date(student.registrationDate).toLocaleDateString('tr-TR')}</TableCell>
                                    <TableCell>
                                        <Chip
                                            label={student.isActive ? 'Active' : 'Inactive'}
                                            color={student.isActive ? 'success' : 'default'}
                                            size="small"
                                            sx={{ fontWeight: 500 }}
                                        />
                                    </TableCell>
                                    <TableCell align="center">
                                        <Box sx={{ display: 'flex', gap: 0.5, justifyContent: 'center' }}>
                                            <IconButton
                                                color="primary"
                                                size="small"
                                                onClick={() => handleEdit(student)}
                                            >
                                                <EditIcon fontSize="small" />
                                            </IconButton>
                                            <IconButton
                                                color="error"
                                                size="small"
                                                onClick={() => handleDelete(student.id)}
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
                                        No students found
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary">
                                        Add your first student to get started!
                                    </Typography>
                                    <Button
                                        variant="contained"
                                        startIcon={<AddIcon />}
                                        onClick={handleAdd}
                                        sx={{ mt: 2 }}
                                    >
                                        Add Student
                                    </Button>
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </TableContainer>

            <StudentFormDialog
                open={openDialog}
                onClose={handleCloseDialog}
                student={selectedStudent}
            />
        </Box>
    );
};