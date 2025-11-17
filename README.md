# Music School Management System

A comprehensive management panel system for music schools.

## 🎯 Project Overview

This system is a **management panel** developed to manage students, teachers, lessons, appointments, and payment processes for music schools.

**⚠️ Important:** This is an admin-only system. Students do not have access. All operations are performed by Admin and Teachers.

## 🚀 Technology Stack

### Backend
- ASP.NET Core 8.0 Web API
- Entity Framework Core 8.0
- MySQL 8.0
- JWT Authentication
- Clean Architecture (4-layer)

### Frontend (Planned)
- React 18
- TypeScript
- Axios
- React Router DOM
- TailwindCSS
- React Query

## 📁 Project Structure
```
MusicSchoolManagement/
├── Backend/
│   ├── MusicSchoolManagement.API/          # REST API endpoints
│   ├── MusicSchoolManagement.Core/         # Domain entities & interfaces
│   ├── MusicSchoolManagement.Infrastructure/ # Data access & external services
│   └── MusicSchoolManagement.Business/     # Business logic & services
└── Frontend/
    └── music-school-frontend/              # React application (coming soon)
```

## 🎯 Features

### Core Features
- ✅ User Management (Admin, Teacher roles)
- ✅ Student Management (CRUD, profiles, parent info)
- ✅ Teacher Management (profiles, specializations, hourly rates)
- ✅ Instrument & Course Management
- ✅ Classroom Management

### Scheduling
- ✅ Appointment System
- ✅ Recurring Appointments (weekly, biweekly)
- ✅ Teacher Availability Management
- ✅ Conflict Detection (teacher, classroom, student)

### Financial
- ✅ Package System (1, 3, 6 month packages)
- ✅ Payment Tracking
- ✅ Remaining Lessons Counter

### Communication
- ✅ Email Notifications
- ✅ SMS Notifications (when available)
- ✅ Appointment Reminders

### Reporting
- ✅ Attendance Tracking
- ✅ Revenue Reports
- ✅ Teacher Performance Reports
- ✅ Dashboard with KPIs

## 🏁 Getting Started

### Prerequisites

- .NET 8.0 SDK
- MySQL 8.0+
- Node.js 18+ (for frontend)
- Visual Studio 2022 / Rider / VS Code

### Backend Setup

1. **Clone the repository**
```bash
   git clone https://github.com/CengizhanKARAGOZ/MusicSchoolManagement.git
   cd MusicSchoolManagement/Backend
```

2. **Configure database connection**
```bash
   cp MusicSchoolManagement.API/appsettings.Example.json MusicSchoolManagement.API/appsettings.json
```
Edit `appsettings.json` with your MySQL credentials.

3. **Restore packages**
```bash
   dotnet restore
```

4. **Apply migrations**
```bash
   cd MusicSchoolManagement.API
   dotnet ef database update
```

5. **Run the application**
```bash
   dotnet run
```

6. **Access Swagger UI**
```
   https://localhost:5001/swagger
```

### Frontend Setup (Coming Soon)
```bash
cd Frontend/music-school-frontend
npm install
npm start
```

## 📊 Database Schema

The system uses 13 main tables:
- **Users** - Admin and Teacher accounts
- **Students** - Student information (no login)
- **Teachers** - Teacher profiles linked to Users
- **Instruments** - Musical instruments
- **Courses** - Course definitions
- **Classrooms** - Room management
- **Packages** - Subscription packages
- **StudentPackages** - Student-package assignments
- **Appointments** - Lesson scheduling
- **TeacherAvailabilities** - Teacher schedules
- **Payments** - Financial records
- **Notifications** - Email/SMS logs
- **AttendanceLogs** - Attendance records

## 🔐 Security

- JWT-based authentication
- Role-based authorization (Admin, Teacher)
- Password hashing with BCrypt
- Sensitive data excluded from version control

## 📝 API Documentation

Full API documentation is available via Swagger UI when running the application.

### Main Endpoints
- `/api/auth` - Authentication
- `/api/users` - User management
- `/api/students` - Student CRUD
- `/api/teachers` - Teacher management
- `/api/courses` - Course management
- `/api/appointments` - Scheduling
- `/api/payments` - Financial operations
- `/api/reports` - Reporting

## 🛠️ Development

### Running Tests
```bash
dotnet test
```

### Creating Migrations
```bash
cd MusicSchoolManagement.API
dotnet ef migrations add MigrationName --project ../MusicSchoolManagement.Infrastructure
```

## 📋 Project Status

- [x] Project structure setup
- [x] Domain entities
- [x] Database configurations
- [ ] Repository pattern implementation
- [ ] Business services
- [ ] API controllers
- [ ] Authentication & Authorization
- [ ] Frontend development
- [ ] Testing
- [ ] Deployment

## 🤝 Contributing

This is a private project. Contact the developer for contribution guidelines.

## 📄 License

Proprietary License - All rights reserved.

## 👨‍💻 Developer

**Cengizhan KARAGÖZ**

---

**Version:** 1.0.0  
**Last Updated:** 2025  
**Status:** In Development