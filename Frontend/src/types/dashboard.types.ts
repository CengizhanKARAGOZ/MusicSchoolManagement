export interface DashboardStats {
    totalStudents: number;
    totalTeachers: number;
    todayAppointments: number;
    monthlyRevenue: number;
}

export interface RecentAppointment {
    id: number;
    studentName: string;
    teacherName: string;
    courseName: string;
    appointmentDate: string;
    startTime: string;
    status: string;
}