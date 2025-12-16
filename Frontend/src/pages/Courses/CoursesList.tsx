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
import { useCourses } from '../../hooks/useCourses';
import { CourseFormDialog } from '../../components/Forms/CourseFormDialog';
import type { Course } from '../../types/course.types';
import { useQueryClient } from '@tanstack/react-query';

export const CoursesList = () => {
    const { courses, isLoading, error, deleteCourse } = useCourses();
    const queryClient = useQueryClient();
    const [openDialog, setOpenDialog] = useState(false);
    const [selectedCourse, setSelectedCourse] = useState<Course | null>(null);

    const handleAdd = () => {
        setSelectedCourse(null);
        setOpenDialog(true);
    };

    const handleEdit = (course: Course) => {
        setSelectedCourse(course);
        setOpenDialog(true);
    };

    const handleDelete = async (id: number) => {
        if (window.confirm('Are you sure you want to delete this course?')) {
            try {
                await deleteCourse(id);
            } catch (err) {
                console.error('Delete failed:', err);
            }
        }
    };

    const handleCloseDialog = () => {
        setOpenDialog(false);
        setSelectedCourse(null);
    };

    const handleSuccess = () => {
        queryClient.invalidateQueries({ queryKey: ['courses'] });
    };

    if (isLoading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
                <CircularProgress />
            </Box>
        );
    }

    if (error) {
        return <Alert severity="error">Failed to load courses</Alert>;
    }

    return (
        <Box sx={{ width: '100%', height: '100%' }}>
            <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Typography variant="h4" fontWeight={600}>
                    Courses
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={handleAdd}
                    size="large"
                >
                    Add Course
                </Button>
            </Box>

            <TableContainer component={Paper} sx={{ boxShadow: 3, width: '100%', overflow: 'auto' }}>
                <Table sx={{ tableLayout: 'fixed', width: '100%' }}>
                    <TableHead>
                        <TableRow sx={{ backgroundColor: '#f5f5f5' }}>
                            <TableCell sx={{ fontWeight: 600, width: '5%' }}>ID</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '20%' }}>Name</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '30%' }}>Description</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '12%' }}>Instrument</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Duration</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Price</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '8%' }}>Status</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }} align="center">Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {courses && courses.length > 0 ? (
                            courses.map((course) => (
                                <TableRow
                                    key={course.id}
                                    sx={{ '&:hover': { backgroundColor: '#fafafa' } }}
                                >
                                    <TableCell>{course.id}</TableCell>
                                    <TableCell sx={{ fontWeight: 500 }}>{course.name}</TableCell>
                                    <TableCell
                                        sx={{
                                            overflow: 'hidden',
                                            textOverflow: 'ellipsis',
                                            whiteSpace: 'nowrap',
                                            maxWidth: 300
                                        }}
                                        title={course.description || ''}
                                    >
                                        {course.description || '-'}
                                    </TableCell>
                                    <TableCell>{course.instrumentName || '-'}</TableCell>
                                    <TableCell>{course.duration} min</TableCell>
                                    <TableCell>₺{course.price.toLocaleString()}</TableCell>
                                    <TableCell>
                                        <Chip
                                            label={course.isActive ? 'Active' : 'Inactive'}
                                            color={course.isActive ? 'success' : 'default'}
                                            size="small"
                                            sx={{ fontWeight: 500 }}
                                        />
                                    </TableCell>
                                    <TableCell align="center">
                                        <Box sx={{ display: 'flex', gap: 0.5, justifyContent: 'center' }}>
                                            <IconButton
                                                color="primary"
                                                size="small"
                                                onClick={() => handleEdit(course)}
                                            >
                                                <EditIcon fontSize="small" />
                                            </IconButton>
                                            <IconButton
                                                color="error"
                                                size="small"
                                                onClick={() => handleDelete(course.id)}
                                            >
                                                <DeleteIcon fontSize="small" />
                                            </IconButton>
                                        </Box>
                                    </TableCell>
                                </TableRow>
                            ))
                        ) : (
                            <TableRow>
                                <TableCell colSpan={8} align="center" sx={{ py: 8 }}>
                                    <Typography variant="h6" color="text.secondary" gutterBottom>
                                        No courses found
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                                        Add your first course to get started!
                                    </Typography>
                                    <Button
                                        variant="contained"
                                        startIcon={<AddIcon />}
                                        onClick={handleAdd}
                                    >
                                        Add Course
                                    </Button>
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </TableContainer>

            <CourseFormDialog
                open={openDialog}
                onClose={handleCloseDialog}
                course={selectedCourse}
                onSuccess={handleSuccess}
            />
        </Box>
    );
};