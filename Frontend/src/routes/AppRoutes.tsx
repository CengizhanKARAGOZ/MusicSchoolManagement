import { Routes, Route, Navigate } from 'react-router-dom';
import { ProtectedRoute } from './ProtectedRoute';
import { Login } from '../pages/Auth/Login';
import { Dashboard } from '../pages/Dashboard/Dashboard';
import { StudentList } from '../pages/Students/StudentList';
import { TeachersList } from '../pages/Teachers/TeachersList';
import { AppointmentsList } from '../pages/Appointments/AppointmentsList';
import { PackagesList } from '../pages/Packages/PackagesList';
import { PaymentsList } from '../pages/Payments/PaymentsList';
import { CoursesList } from '../pages/Courses/CoursesList';
import { ClassroomsList } from '../pages/Classrooms/ClassroomsList';
import { Layout } from '../components/Layout/Layout';
import { ChangePassword } from '../pages/Auth/ChangePassword';

export const AppRoutes = () => {
    return (
        <Routes>
            {/* Public Routes */}
            <Route path="/login" element={<Login />} />

            {/* Protected Routes */}
            <Route
                path="/"
                element={
                    <ProtectedRoute>
                        <Layout />
                    </ProtectedRoute>
                }
            >
                <Route index element={<Navigate to="/dashboard" replace />} />
                <Route path="dashboard" element={<Dashboard />} />
                <Route path="students" element={<StudentList />} />
                <Route path="teachers" element={<TeachersList />} />
                <Route path="appointments" element={<AppointmentsList />} />
                <Route path="packages" element={<PackagesList />} />
                <Route path="payments" element={<PaymentsList />} />
                <Route path="courses" element={<CoursesList />} />
                <Route path="classrooms" element={<ClassroomsList />} />
                <Route path="/change-password" element={<ChangePassword />} />

                <Route path="*" element={<Navigate to="/dashboard" replace />} />
            </Route>
        </Routes>
    );
};