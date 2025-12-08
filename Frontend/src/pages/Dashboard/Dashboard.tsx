import {
  Container,
  Paper,
  Typography,
  Box,
  Card,
  CardContent,
  CircularProgress,
} from '@mui/material';
import {
  People as PeopleIcon,
  School as SchoolIcon,
  Event as EventIcon,
  Payment as PaymentIcon,
} from '@mui/icons-material';
import { useAuthStore } from '../../store/authStore';
import { useDashboard } from '../../hooks/useDashboard';

export const Dashboard = () => {
  const { user } = useAuthStore();
  const { stats, isLoading } = useDashboard();

  if (isLoading) {
    return (
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
          <CircularProgress />
        </Box>
    );
  }

  const statsData = [
    {
      title: 'Total Students',
      value: stats?.totalStudents.toString() || '0',
      icon: <PeopleIcon />,
      color: '#1976d2'
    },
    {
      title: 'Total Teachers',
      value: stats?.totalTeachers.toString() || '0',
      icon: <SchoolIcon />,
      color: '#2e7d32'
    },
    {
      title: "Today's Appointments",
      value: stats?.todayAppointments.toString() || '0',
      icon: <EventIcon />,
      color: '#ed6c02'
    },
    {
      title: 'Monthly Revenue',
      value: `$${stats?.monthlyRevenue.toLocaleString() || '0'}`,
      icon: <PaymentIcon />,
      color: '#9c27b0'
    },
  ];

  return (
      <Container maxWidth="lg">
        <Box sx={{ mb: 4 }}>
          <Typography variant="h4" gutterBottom>
            Welcome back, {user?.firstName}! 👋
          </Typography>
          <Typography variant="body1" color="text.secondary">
            Here's what's happening with your music school today.
          </Typography>
        </Box>

        {/* Stats Cards */}
        <Box sx={{ display: 'flex', gap: 3, mb: 3, flexWrap: 'wrap' }}>
          {statsData.map((stat, index) => (
              <Box key={index} sx={{ flex: '1 1 calc(25% - 18px)', minWidth: 200 }}>
                <Card sx={{ height: '100%' }}>
                  <CardContent>
                    <Box
                        sx={{
                          display: 'flex',
                          alignItems: 'center',
                          justifyContent: 'space-between',
                        }}
                    >
                      <Box>
                        <Typography color="text.secondary" gutterBottom variant="body2">
                          {stat.title}
                        </Typography>
                        <Typography variant="h4" component="div">
                          {stat.value}
                        </Typography>
                      </Box>
                      <Box
                          sx={{
                            backgroundColor: stat.color,
                            borderRadius: 2,
                            p: 1.5,
                            color: 'white',
                          }}
                      >
                        {stat.icon}
                      </Box>
                    </Box>
                  </CardContent>
                </Card>
              </Box>
          ))}
        </Box>

        {/* Content Area */}
        <Box sx={{ display: 'flex', gap: 3, flexWrap: 'wrap' }}>
          <Box sx={{ flex: '1 1 65%', minWidth: 300 }}>
            <Paper sx={{ p: 3, height: 400 }}>
              <Typography variant="h6" gutterBottom>
                Recent Appointments
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Loading appointments...
              </Typography>
            </Paper>
          </Box>

          <Box sx={{ flex: '1 1 30%', minWidth: 250 }}>
            <Paper sx={{ p: 3, height: 400 }}>
              <Typography variant="h6" gutterBottom>
                Quick Actions
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Manage your school operations
              </Typography>
            </Paper>
          </Box>
        </Box>
      </Container>
  );
};