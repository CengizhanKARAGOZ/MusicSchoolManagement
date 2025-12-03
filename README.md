# 🎵 Music School Management System

A comprehensive RESTful API for managing music school operations including students, teachers, courses, appointments, packages, and payments.

## 📋 Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
- [API Documentation](#api-documentation)
- [Database Schema](#database-schema)
- [Authentication](#authentication)
- [Project Structure](#project-structure)
- [Contributing](#contributing)

## ✨ Features

### Core Modules
- **Student Management** - Register, update, and manage student information
- **Teacher Management** - Teacher profiles with specializations and availability
- **Course Management** - Course catalog with instruments, levels, and pricing
- **Appointment Scheduling** - Single and recurring appointments with conflict detection
- **Package System** - Lesson packages with automatic usage tracking
- **Payment Processing** - Payment records, revenue tracking, and refunds
- **Classroom Management** - Resource allocation and availability tracking

### Advanced Features
- 🔐 JWT Authentication & Role-based Authorization (Admin, Teacher, Student)
- 🔄 Recurring Appointments (Weekly/Biweekly patterns)
- ⚠️ Conflict Detection (Teacher/Student/Classroom scheduling)
- 📦 Package Tracking (Automatic lesson count management)
- 💰 Revenue Reports (Date-based financial analytics)
- ✅ FluentValidation (Comprehensive input validation)
- 🗺️ AutoMapper (Clean DTO mappings)
- 🚨 Global Exception Handling (Consistent error responses)
- 📝 Serilog Logging (Structured logging)
- 📚 Swagger/OpenAPI Documentation

## 🛠️ Tech Stack

### Backend
- **.NET 8.0** - Latest LTS version
- **ASP.NET Core Web API** - RESTful API framework
- **Entity Framework Core 8.0** - ORM with Code First approach
- **MySQL 8.0** - Primary database

### Architecture & Patterns
- **Clean Architecture** - Separation of concerns
- **Repository Pattern** - Data access abstraction
- **Unit of Work Pattern** - Transaction management
- **CQRS** - Command Query Responsibility Segregation

### Libraries & Tools
- **AutoMapper 13.0** - Object-to-object mapping
- **FluentValidation 11.9** - Input validation
- **Serilog** - Structured logging
- **Pomelo.EntityFrameworkCore.MySql** - MySQL provider
- **Swashbuckle (Swagger)** - API documentation
- **BCrypt.Net** - Password hashing

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────┐
│                   Presentation Layer                 │
│              (MusicSchoolManagement.API)            │
│                    Controllers                       │
└────────────────────┬────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────┐
│                  Business Layer                      │
│           (MusicSchoolManagement.Business)          │
│              Services + AutoMapper                   │
└────────────────────┬────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────┐
│                    Core Layer                        │
│             (MusicSchoolManagement.Core)            │
│        Entities + DTOs + Interfaces + Enums         │
└────────────────────┬────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────┐
│               Infrastructure Layer                   │
│        (MusicSchoolManagement.Infrastructure)       │
│        Repositories + DbContext + Migrations        │
└─────────────────────────────────────────────────────┘
```

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL 8.0+](https://dev.mysql.com/downloads/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [JetBrains Rider](https://www.jetbrains.com/rider/) (recommended)

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/CengizhanKARAGOZ/MusicSchoolManagement.git
cd MusicSchoolManagement
```

2. **Update Database Connection String**

Edit `appsettings.json` in `MusicSchoolManagement.API`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=musicschool_db;User=root;Password=your_password;"
  }
}
```

3. **Apply Database Migrations**
```bash
cd Backend/MusicSchoolManagement.API
dotnet ef database update
```

4. **Run the Application**
```bash
dotnet run
```

5. **Access Swagger UI**
```
https://localhost:5001
```

### Default Admin Credentials
```
Email: admin@musicschool.com
Password: Admin123!
```

## 📚 API Documentation

### Authentication Endpoints
```http
POST   /api/Auth/login       # User login
POST   /api/Auth/register    # User registration
```

### Student Endpoints
```http
GET    /api/Students              # Get all students
GET    /api/Students/active       # Get active students
GET    /api/Students/{id}         # Get student by ID
POST   /api/Students              # Create student (Admin)
PUT    /api/Students/{id}         # Update student (Admin)
DELETE /api/Students/{id}         # Delete student (Admin)
```

### Teacher Endpoints
```http
GET    /api/Teachers              # Get all teachers
GET    /api/Teachers/{id}         # Get teacher by ID
GET    /api/Teachers/user/{userId}# Get teacher by user ID
POST   /api/Teachers              # Create teacher (Admin)
PUT    /api/Teachers/{id}         # Update teacher (Admin)
DELETE /api/Teachers/{id}         # Delete teacher (Admin)
```

### Appointment Endpoints
```http
GET    /api/Appointments                    # Get all appointments
GET    /api/Appointments/range              # Get by date range
GET    /api/Appointments/teacher/{id}       # Get teacher's appointments
GET    /api/Appointments/student/{id}       # Get student's appointments
GET    /api/Appointments/upcoming           # Get upcoming appointments
GET    /api/Appointments/{id}               # Get appointment by ID
POST   /api/Appointments                    # Create appointment (Admin/Teacher)
POST   /api/Appointments/recurring          # Create recurring (Admin/Teacher)
PUT    /api/Appointments/{id}               # Update appointment (Admin/Teacher)
POST   /api/Appointments/{id}/cancel        # Cancel appointment (Admin/Teacher)
DELETE /api/Appointments/{id}               # Delete appointment (Admin)
```

### Package Endpoints
```http
GET    /api/Packages                        # Get all packages
GET    /api/Packages/active                 # Get active packages
GET    /api/Packages/{id}                   # Get package by ID
GET    /api/Packages/student/{studentId}    # Get student packages
POST   /api/Packages                        # Create package (Admin)
POST   /api/Packages/assign                 # Assign to student (Admin)
PUT    /api/Packages/{id}                   # Update package (Admin)
DELETE /api/Packages/{id}                   # Delete package (Admin)
POST   /api/Packages/student-package/{id}/cancel # Cancel package (Admin)
```

### Payment Endpoints
```http
GET    /api/Payments                        # Get all payments (Admin)
GET    /api/Payments/student/{studentId}    # Get student payments
GET    /api/Payments/range                  # Get by date range (Admin)
GET    /api/Payments/pending                # Get pending payments (Admin)
GET    /api/Payments/{id}                   # Get payment by ID
POST   /api/Payments                        # Create payment (Admin)
GET    /api/Payments/revenue                # Get revenue (Admin)
POST   /api/Payments/{id}/refund            # Refund payment (Admin)
```

### Course, Instrument & Classroom Endpoints
Similar CRUD operations available for:
- `/api/Courses`
- `/api/Instruments`
- `/api/Classrooms`

Full API documentation available at `/swagger` when running the application.

## 🗄️ Database Schema

### Core Entities
- **Users** - Authentication and user information
- **Students** - Student profiles and registration data
- **Teachers** - Teacher profiles with specializations
- **Instruments** - Musical instruments catalog
- **Courses** - Course offerings with pricing
- **Classrooms** - Physical classroom resources
- **Packages** - Lesson package definitions
- **StudentPackages** - Assigned packages with usage tracking
- **Appointments** - Scheduled lessons (single and recurring)
- **Payments** - Payment records and transactions
- **AttendanceLogs** - Lesson attendance tracking

### Key Relationships
- User → Teacher (One-to-One)
- Teacher → Appointments (One-to-Many)
- Student → Appointments (One-to-Many)
- Student → StudentPackages (One-to-Many)
- Package → StudentPackages (One-to-Many)
- Course → Appointments (One-to-Many)
- Classroom → Appointments (One-to-Many)

## 🔐 Authentication

The API uses **JWT (JSON Web Tokens)** for authentication.

### How to Authenticate

1. **Login/Register** to get a JWT token:
```json
POST /api/Auth/login
{
  "email": "admin@musicschool.com",
  "password": "Admin123!"
}

Response:
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresAt": "2025-11-29T10:00:00Z"
  }
}
```

2. **Add token to requests**:
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### User Roles
- **Admin** - Full system access
- **Teacher** - Create/manage appointments, view students
- **Student** - View own appointments and packages

## 📁 Project Structure

```
MusicSchoolManagement/
├── Backend/
│   ├── MusicSchoolManagement.API/           # Web API (Controllers, Middleware)
│   │   ├── Controllers/                     # API endpoints
│   │   ├── Middleware/                      # Custom middleware
│   │   ├── Validators/                      # FluentValidation rules
│   │   └── Program.cs                       # App configuration
│   │
│   ├── MusicSchoolManagement.Business/      # Business Logic
│   │   ├── Services/                        # Service implementations
│   │   ├── Mappings/                        # AutoMapper profiles
│   │   └── Helpers/                         # Utility classes
│   │
│   ├── MusicSchoolManagement.Core/          # Core Domain
│   │   ├── Entities/                        # Domain models
│   │   ├── DTOs/                            # Data transfer objects
│   │   ├── Enums/                           # Enumerations
│   │   ├── Interfaces/                      # Contracts
│   │   └── Exceptions/                      # Custom exceptions
│   │
│   └── MusicSchoolManagement.Infrastructure/# Data Access
│       ├── Data/                            # DbContext
│       ├── Repositories/                    # Repository implementations
│       └── Migrations/                      # EF Core migrations
│
├── README.md
└── .gitignore
```

## 🧪 Testing

### Manual Testing with Swagger
1. Run the application
2. Navigate to `https://localhost:5001`
3. Click "Authorize" and enter JWT token
4. Test endpoints directly from Swagger UI

### Postman Collection
Import the provided Postman collection for comprehensive API testing.
[Download Collection](./docs/MusicSchool-API.postman_collection.json)

## 🐳 Docker Support (Optional)

### Build Docker Image
```bash
docker build -t musicschool-api .
```

### Run with Docker Compose
```bash
docker-compose up -d
```

## 🚀 Deployment

### Prerequisites
- Linux server or cloud platform (Azure, AWS, DigitalOcean)
- MySQL 8.0+ database
- HTTPS certificate (Let's Encrypt recommended)

### Deployment Steps

1. **Publish the application**
```bash
dotnet publish -c Release -o ./publish
```

2. **Transfer files to server**
```bash
scp -r ./publish/* user@server:/var/www/musicschool-api/
```

3. **Configure systemd service** (Linux)
```bash
sudo nano /etc/systemd/system/musicschool-api.service
```

4. **Start the service**
```bash
sudo systemctl start musicschool-api
sudo systemctl enable musicschool-api
```

5. **Setup Nginx reverse proxy**
```bash
sudo nano /etc/nginx/sites-available/musicschool-api
```

## 📝 Environment Variables

```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Server=...;Database=...;
JwtSettings__SecretKey=your-256-bit-secret
JwtSettings__Issuer=MusicSchoolAPI
JwtSettings__Audience=MusicSchoolClient
```

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Cengizhan**
- GitHub: [@CengizhanKARAGOZ](https://github.com/CengizhanKARAGOZ)
- LinkedIn: [Cengizhan Karagöz](https://linkedin.com/in/cengizhankaragoz)

## 🙏 Acknowledgments

- Clean Architecture principles by Robert C. Martin
- ASP.NET Core documentation by Microsoft
- Entity Framework Core best practices

---

⭐ If you find this project useful, please consider giving it a star!