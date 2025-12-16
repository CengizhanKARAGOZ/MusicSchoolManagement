export interface Appointment {
    id: number;
    studentId: number;
    studentName: string;
    teacherId: number;
    teacherName: string;
    courseId: number;
    courseName: string;
    classroomId?: number;
    classroomName?: string;
    appointmentDate: string;
    startTime: string;
    endTime: string;
    status: 'Scheduled' | 'Completed' | 'Cancelled' | 'Rescheduled';
    notes?: string;
    createdAt: string;
}

export interface CreateAppointmentDto {
    studentId: number;
    teacherId: number;
    courseId: number;
    classroomId?: number;
    appointmentDate: string;
    startTime: string;
    endTime: string;
    notes?: string;
}

export interface UpdateAppointmentDto {
    appointmentDate: string;
    startTime: string;
    endTime: string;
    classroomId?: number;
    status: string;
    notes?: string;
}