import { Container, Typography, Paper, Box } from '@mui/material';

export const ClassroomsList = () => {
    return (
        <Container maxWidth="lg">
            <Box sx={{ mb: 4 }}>
                <Typography variant="h4" gutterBottom>
                    Classrooms
                </Typography>
            </Box>

            <Paper sx={{ p: 3 }}>
                <Typography variant="body1">
                    Classrooms management coming soon...
                </Typography>
            </Paper>
        </Container>
    );
};