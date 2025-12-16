import { useEffect, useState } from 'react';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    TextField,
    Box,
    MenuItem,
    FormControl,
    InputLabel,
    Select,
} from '@mui/material';
import { useForm, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { useSnackbar } from 'notistack';
import type { Appointment } from '../../types/appointment.types';
import { useAppointments } from '../../hooks/useAppointments';
import axiosInstance from '../../api/axios.config';

const schema = yup.object({
    studentId: yup.number().required('Student is required').min(1, 'Please select a student'),
    teacherId: yup.number().required('Teacher is required').min(1, 'Please select a teacher'),
    courseId: yup.number().required('Course is required').min(1, 'Please select a course'),
    classroomId: yup.number().nullable(),
    appointmentDate: yup.string().required('Date is required'),
    startTime: yup.string().required('Start time is required'),
    endTime: yup.string().required('End time is required'),
    status: yup.string(),
    notes: yup.string(),
});

interface AppointmentFormDialogProps {
    open: boolean;
    onClose: () => void;
    appointment: Appointment | null;
    onSuccess: () => void;
}

interface SelectOption {
    id: number;
    name: string;
}

export const AppointmentFormDialog = ({ open, onClose, appointment, onSuccess }: AppointmentFormDialogProps) => {
    const { enqueueSnackbar } = useSnackbar();
    const { createAppointment, updateAppointment } = useAppointments();
    const isEdit = !!appointment;

    const [students, setStudents] = useState<SelectOption[]>([]);
    const [teachers, setTeachers] = useState<SelectOption[]>([]);
    const [courses, setCourses] = useState<SelectOption[]>([]);
    const [classrooms, setClassrooms] = useState<SelectOption[]>([]);
    const [loading, setLoading] = useState(false);

    const {
        control,
        handleSubmit,
        reset,
        formState: { errors, isSubmitting },
    } = useForm({
        resolver: yupResolver(schema),
        defaultValues: {
            studentId: 0,
            teacherId: 0,
            courseId: 0,
            classroomId: null as number | null,
            appointmentDate: new Date().toISOString().split('T')[0],
            startTime: '09:00',
            endTime: '10:00',
            status: 'Scheduled',
            notes: '',
        },
    });

    // Load dropdown data
    useEffect(() => {
        const loadData = async () => {
            setLoading(true);
            try {
                const [studentsRes, teachersRes, coursesRes, classroomsRes] = await Promise.all([
                    axiosInstance.get('/Students'),
                    axiosInstance.get('/Teachers'),
                    axiosInstance.get('/Courses'),
                    axiosInstance.get('/Classrooms'),
                ]);

                setStudents(
                    studentsRes.data.data.map((s: any) => ({
                        id: s.id,
                        name: `${s.firstName} ${s.lastName}`,
                    }))
                );

                setTeachers(
                    teachersRes.data.data.map((t: any) => ({
                        id: t.id,
                        name: t.user ? `${t.user.firstName} ${t.user.lastName}` : 'N/A',
                    }))
                );

                setCourses(
                    coursesRes.data.data.map((c: any) => ({
                        id: c.id,
                        name: c.name,
                    }))
                );

                setClassrooms(
                    classroomsRes.data.data.map((cl: any) => ({
                        id: cl.id,
                        name: cl.name,
                    }))
                );
            } catch (error) {
                console.error('Failed to load data:', error);
                enqueueSnackbar('Failed to load form data', { variant: 'error' });
            } finally {
                setLoading(false);
            }
        };

        if (open) {
            loadData();
        }
    }, [open, enqueueSnackbar]);

    useEffect(() => {
        if (appointment) {
            reset({
                studentId: appointment.studentId,
                teacherId: appointment.teacherId,
                courseId: appointment.courseId,
                classroomId: appointment.classroomId || null,
                appointmentDate: appointment.appointmentDate.split('T')[0],
                startTime: appointment.startTime,
                endTime: appointment.endTime,
                status: appointment.status,
                notes: appointment.notes || '',
            });
        } else {
            reset({
                studentId: 0,
                teacherId: 0,
                courseId: 0,
                classroomId: null,
                appointmentDate: new Date().toISOString().split('T')[0],
                startTime: '09:00',
                endTime: '10:00',
                status: 'Scheduled',
                notes: '',
            });
        }
    }, [appointment, reset]);

    const onSubmit = async (data: any) => {
        try {
            if (isEdit) {
                await updateAppointment({
                    id: appointment.id,
                    data: {
                        appointmentDate: data.appointmentDate,
                        startTime: data.startTime,
                        endTime: data.endTime,
                        classroomId: data.classroomId,
                        status: data.status,
                        notes: data.notes,
                    }
                });
                enqueueSnackbar('Appointment updated successfully!', { variant: 'success' });
            } else {
                await createAppointment({
                    studentId: data.studentId,
                    teacherId: data.teacherId,
                    courseId: data.courseId,
                    classroomId: data.classroomId,
                    appointmentDate: data.appointmentDate,
                    startTime: data.startTime,
                    endTime: data.endTime,
                    notes: data.notes,
                });
                enqueueSnackbar('Appointment created successfully!', { variant: 'success' });
            }
            onSuccess();
            onClose();
        } catch (error: any) {
            enqueueSnackbar(error.response?.data?.message || 'Operation failed', { variant: 'error' });
        }
    };

    return (
        <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
            <DialogTitle>{isEdit ? 'Edit Appointment' : 'Add New Appointment'}</DialogTitle>
            <form onSubmit={handleSubmit(onSubmit)}>
                <DialogContent>
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                        {/* Student Select */}
                        <Controller
                            name="studentId"
                            control={control}
                            render={({ field }) => (
                                <FormControl fullWidth error={!!errors.studentId} disabled={isEdit}>
                                    <InputLabel>Student</InputLabel>
                                    <Select {...field} label="Student" disabled={loading || isEdit}>
                                        <MenuItem value={0}>Select Student</MenuItem>
                                        {students.map((student) => (
                                            <MenuItem key={student.id} value={student.id}>
                                                {student.name}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                    {errors.studentId && (
                                        <Box sx={{ color: 'error.main', fontSize: '0.75rem', mt: 0.5 }}>
                                            {errors.studentId.message}
                                        </Box>
                                    )}
                                </FormControl>
                            )}
                        />

                        {/* Teacher Select */}
                        <Controller
                            name="teacherId"
                            control={control}
                            render={({ field }) => (
                                <FormControl fullWidth error={!!errors.teacherId} disabled={isEdit}>
                                    <InputLabel>Teacher</InputLabel>
                                    <Select {...field} label="Teacher" disabled={loading || isEdit}>
                                        <MenuItem value={0}>Select Teacher</MenuItem>
                                        {teachers.map((teacher) => (
                                            <MenuItem key={teacher.id} value={teacher.id}>
                                                {teacher.name}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                    {errors.teacherId && (
                                        <Box sx={{ color: 'error.main', fontSize: '0.75rem', mt: 0.5 }}>
                                            {errors.teacherId.message}
                                        </Box>
                                    )}
                                </FormControl>
                            )}
                        />

                        {/* Course Select */}
                        <Controller
                            name="courseId"
                            control={control}
                            render={({ field }) => (
                                <FormControl fullWidth error={!!errors.courseId} disabled={isEdit}>
                                    <InputLabel>Course</InputLabel>
                                    <Select {...field} label="Course" disabled={loading || isEdit}>
                                        <MenuItem value={0}>Select Course</MenuItem>
                                        {courses.map((course) => (
                                            <MenuItem key={course.id} value={course.id}>
                                                {course.name}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                    {errors.courseId && (
                                        <Box sx={{ color: 'error.main', fontSize: '0.75rem', mt: 0.5 }}>
                                            {errors.courseId.message}
                                        </Box>
                                    )}
                                </FormControl>
                            )}
                        />

                        {/* Classroom Select (Optional) */}
                        <Controller
                            name="classroomId"
                            control={control}
                            render={({ field }) => (
                                <FormControl fullWidth>
                                    <InputLabel>Classroom (Optional)</InputLabel>
                                    <Select {...field} label="Classroom (Optional)" disabled={loading}>
                                        <MenuItem value="">None</MenuItem>
                                        {classrooms.map((classroom) => (
                                            <MenuItem key={classroom.id} value={classroom.id}>
                                                {classroom.name}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                            )}
                        />

                        {/* Date */}
                        <Controller
                            name="appointmentDate"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Date"
                                    type="date"
                                    fullWidth
                                    InputLabelProps={{ shrink: true }}
                                    error={!!errors.appointmentDate}
                                    helperText={errors.appointmentDate?.message}
                                />
                            )}
                        />

                        {/* Time Range */}
                        <Box sx={{ display: 'flex', gap: 2 }}>
                            <Controller
                                name="startTime"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label="Start Time"
                                        type="time"
                                        fullWidth
                                        InputLabelProps={{ shrink: true }}
                                        error={!!errors.startTime}
                                        helperText={errors.startTime?.message}
                                    />
                                )}
                            />
                            <Controller
                                name="endTime"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label="End Time"
                                        type="time"
                                        fullWidth
                                        InputLabelProps={{ shrink: true }}
                                        error={!!errors.endTime}
                                        helperText={errors.endTime?.message}
                                    />
                                )}
                            />
                        </Box>

                        {/* Status (Edit only) */}
                        {isEdit && (
                            <Controller
                                name="status"
                                control={control}
                                render={({ field }) => (
                                    <FormControl fullWidth>
                                        <InputLabel>Status</InputLabel>
                                        <Select {...field} label="Status">
                                            <MenuItem value="Scheduled">Scheduled</MenuItem>
                                            <MenuItem value="Completed">Completed</MenuItem>
                                            <MenuItem value="Cancelled">Cancelled</MenuItem>
                                            <MenuItem value="Rescheduled">Rescheduled</MenuItem>
                                        </Select>
                                    </FormControl>
                                )}
                            />
                        )}

                        {/* Notes */}
                        <Controller
                            name="notes"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Notes"
                                    fullWidth
                                    multiline
                                    rows={3}
                                    placeholder="Additional notes..."
                                    error={!!errors.notes}
                                    helperText={errors.notes?.message}
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