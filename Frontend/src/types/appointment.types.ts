import { AppointmentStatus } from './common.types';

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
    status: AppointmentStatus;
    isRecurring: boolean;
    notes?: string;
    cancellationReason?: string;
}

export interface CreateAppointmentDto {
    studentId: number;
    teacherId: number;
    courseId: number;
    classroomId?: number;
    studentPackageId?: number;
    appointmentDate: string;
    startTime: string;
    endTime: string;
    notes?: string;
}

export interface CreateRecurringAppointmentDto extends CreateAppointmentDto {
    endDate: string;
    recurringPattern: 'Weekly' | 'Biweekly';
}