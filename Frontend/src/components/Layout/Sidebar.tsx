import {
    Drawer,
    List,
    ListItem,
    ListItemButton,
    ListItemIcon,
    ListItemText,
    Box,
} from '@mui/material';
import {
    Dashboard as DashboardIcon,
    People as PeopleIcon,
    School as SchoolIcon,
    Event as EventIcon,
    Payment as PaymentIcon,
    CardGiftcard as PackageIcon,
    MusicNote as MusicIcon,
    ClassOutlined as ClassIcon,
} from '@mui/icons-material';
import { useNavigate, useLocation } from 'react-router-dom';
import { useUiStore } from '../../store/uiStore';
import { useAuthStore } from '../../store/authStore';
import { UserRole } from '../../types/common.types';

export const Sidebar = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const { sidebarOpen } = useUiStore();
    const { user } = useAuthStore();

    const menuItems = [
        { text: 'Dashboard', icon: <DashboardIcon />, path: '/dashboard', roles: [UserRole.Admin, UserRole.Teacher, UserRole.Student] },
        { text: 'Students', icon: <PeopleIcon />, path: '/students', roles: [UserRole.Admin, UserRole.Teacher] },
        { text: 'Teachers', icon: <SchoolIcon />, path: '/teachers', roles: [UserRole.Admin] },
        { text: 'Appointments', icon: <EventIcon />, path: '/appointments', roles: [UserRole.Admin, UserRole.Teacher] },
        { text: 'Packages', icon: <PackageIcon />, path: '/packages', roles: [UserRole.Admin] },
        { text: 'Payments', icon: <PaymentIcon />, path: '/payments', roles: [UserRole.Admin] },
        { text: 'Courses', icon: <MusicIcon />, path: '/courses', roles: [UserRole.Admin] },
        { text: 'Classrooms', icon: <ClassIcon />, path: '/classrooms', roles: [UserRole.Admin] },
    ];

    const filteredMenuItems = menuItems.filter((item) =>
        user && item.roles.includes(user.role)
    );

    return (
        <Drawer
            variant="permanent"
            open={sidebarOpen}
            sx={{
                width: sidebarOpen ? 240 : 64,
                flexShrink: 0,
                '& .MuiDrawer-paper': {
                    width: sidebarOpen ? 240 : 64,
                    boxSizing: 'border-box',
                    top: 64,
                    height: 'calc(100% - 64px)',
                    transition: 'width 0.3s',
                    overflowX: 'hidden',
                    position: 'fixed',
                },
            }}
        >
            <Box sx={{ overflow: 'auto' }}>
                <List>
                    {filteredMenuItems.map((item) => (
                        <ListItem key={item.text} disablePadding>
                            <ListItemButton
                                selected={location.pathname === item.path}
                                onClick={() => navigate(item.path)}
                            >
                                <ListItemIcon>{item.icon}</ListItemIcon>
                                {sidebarOpen && <ListItemText primary={item.text} />}
                            </ListItemButton>
                        </ListItem>
                    ))}
                </List>
            </Box>
        </Drawer>
    );
};