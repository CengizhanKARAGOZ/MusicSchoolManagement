import { Box } from '@mui/material';
import { Outlet } from 'react-router-dom';
import { Navbar } from './Navbar';
import { Sidebar } from './Sidebar';

export const Layout = () => {
    return (
        <Box sx={{ display: 'flex', minHeight: '100vh' }}>
            <Navbar />
            <Sidebar />
            <Box
                component="main"
                sx={{
                    flexGrow: 1,
                    p: 2,
                    pt: 10,
                    backgroundColor: '#f5f5f5',
                    width: '100%',
                }}
            >
                <Outlet />
            </Box>
        </Box>
    );
};