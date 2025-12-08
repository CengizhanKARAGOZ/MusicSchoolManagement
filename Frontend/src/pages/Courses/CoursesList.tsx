import { Container, Typography, Paper, Box } from '@mui/material';

export const CoursesList = () => {
    return (
        <Container maxWidth="lg">
            <Box sx={{ mb: 4 }}>
                <Typography variant="h4" gutterBottom>
                    Courses
                </Typography>
            </Box>

            <Paper sx={{ p: 3 }}>
                <Typography variant="body1">
                    Courses management coming soon...
                </Typography>
            </Paper>
        </Container>
    );
};