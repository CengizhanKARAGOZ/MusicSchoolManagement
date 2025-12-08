import { Container, Typography, Paper, Box } from '@mui/material';

export const PaymentsList = () => {
    return (
        <Container maxWidth="lg">
            <Box sx={{ mb: 4 }}>
                <Typography variant="h4" gutterBottom>
                    Payments
                </Typography>
            </Box>

            <Paper sx={{ p: 3 }}>
                <Typography variant="body1">
                    Payments management coming soon...
                </Typography>
            </Paper>
        </Container>
    );
};