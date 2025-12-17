import { useState } from 'react';
import {
    Box,
    Button,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    IconButton,
    Chip,
    CircularProgress,
    Alert,
    Typography,
} from '@mui/material';
import {
    Add as AddIcon,
    Edit as EditIcon,
    Delete as DeleteIcon,
} from '@mui/icons-material';
import { usePayments } from '../../hooks/usePayments';
import { PaymentFormDialog } from '../../components/Forms/PaymentFormDialog';
import type { Payment } from '../../types/payment.types';
import { useQueryClient } from '@tanstack/react-query';

export const PaymentsList = () => {
    const { payments, isLoading, error, deletePayment } = usePayments();
    const queryClient = useQueryClient();
    const [openDialog, setOpenDialog] = useState(false);
    const [selectedPayment, setSelectedPayment] = useState<Payment | null>(null);

    const handleAdd = () => {
        setSelectedPayment(null);
        setOpenDialog(true);
    };

    const handleEdit = (payment: Payment) => {
        setSelectedPayment(payment);
        setOpenDialog(true);
    };

    const handleDelete = async (id: number) => {
        if (window.confirm('Are you sure you want to delete this payment?')) {
            try {
                await deletePayment(id);
            } catch (err) {
                console.error('Delete failed:', err);
            }
        }
    };

    const handleCloseDialog = () => {
        setOpenDialog(false);
        setSelectedPayment(null);
    };

    const handleSuccess = () => {
        queryClient.invalidateQueries({ queryKey: ['payments'] });
    };

    const getStatusColor = (status: string) => {
        switch (status) {
            case 'Completed':
                return 'success';
            case 'Pending':
                return 'warning';
            case 'Failed':
                return 'error';
            case 'Refunded':
                return 'info';
            default:
                return 'default';
        }
    };

    const getMethodColor = (method: string) => {
        switch (method) {
            case 'Cash':
                return 'default';
            case 'CreditCard':
                return 'primary';
            case 'BankTransfer':
                return 'secondary';
            case 'Online':
                return 'info';
            default:
                return 'default';
        }
    };

    if (isLoading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
                <CircularProgress />
            </Box>
        );
    }

    if (error) {
        return <Alert severity="error">Failed to load payments</Alert>;
    }

    return (
        <Box sx={{ width: '100%', height: '100%' }}>
            <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Typography variant="h4" fontWeight={600}>
                    Payments
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={handleAdd}
                    size="large"
                >
                    Add Payment
                </Button>
            </Box>

            <TableContainer component={Paper} sx={{ boxShadow: 3, width: '100%', overflow: 'auto' }}>
                <Table sx={{ tableLayout: 'fixed', width: '100%' }}>
                    <TableHead>
                        <TableRow sx={{ backgroundColor: '#f5f5f5' }}>
                            <TableCell sx={{ fontWeight: 600, width: '5%' }}>ID</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '15%' }}>Student</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '12%' }}>Package</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Amount</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '12%' }}>Date</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Method</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Status</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '12%' }}>Transaction ID</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }} align="center">Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {payments && payments.length > 0 ? (
                            payments.map((payment) => (
                                <TableRow
                                    key={payment.id}
                                    sx={{ '&:hover': { backgroundColor: '#fafafa' } }}
                                >
                                    <TableCell>{payment.id}</TableCell>
                                    <TableCell sx={{ fontWeight: 500 }}>{payment.studentName}</TableCell>
                                    <TableCell>{payment.packageName || '-'}</TableCell>
                                    <TableCell sx={{ fontWeight: 600, color: 'success.main' }}>
                                        ₺{payment.amount.toLocaleString()}
                                    </TableCell>
                                    <TableCell>{new Date(payment.paymentDate).toLocaleDateString('tr-TR')}</TableCell>
                                    <TableCell>
                                        <Chip
                                            label={payment.paymentMethod}
                                            color={getMethodColor(payment.paymentMethod)}
                                            size="small"
                                            sx={{ fontWeight: 500 }}
                                        />
                                    </TableCell>
                                    <TableCell>
                                        <Chip
                                            label={payment.status}
                                            color={getStatusColor(payment.status)}
                                            size="small"
                                            sx={{ fontWeight: 500 }}
                                        />
                                    </TableCell>
                                    <TableCell
                                        sx={{
                                            overflow: 'hidden',
                                            textOverflow: 'ellipsis',
                                            whiteSpace: 'nowrap',
                                            maxWidth: 150
                                        }}
                                        title={payment.transactionId || ''}
                                    >
                                        {payment.transactionId || '-'}
                                    </TableCell>
                                    <TableCell align="center">
                                        <Box sx={{ display: 'flex', gap: 0.5, justifyContent: 'center' }}>
                                            <IconButton
                                                color="primary"
                                                size="small"
                                                onClick={() => handleEdit(payment)}
                                            >
                                                <EditIcon fontSize="small" />
                                            </IconButton>
                                            <IconButton
                                                color="error"
                                                size="small"
                                                onClick={() => handleDelete(payment.id)}
                                            >
                                                <DeleteIcon fontSize="small" />
                                            </IconButton>
                                        </Box>
                                    </TableCell>
                                </TableRow>
                            ))
                        ) : (
                            <TableRow>
                                <TableCell colSpan={9} align="center" sx={{ py: 8 }}>
                                    <Typography variant="h6" color="text.secondary" gutterBottom>
                                        No payments found
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                                        Add your first payment to get started!
                                    </Typography>
                                    <Button
                                        variant="contained"
                                        startIcon={<AddIcon />}
                                        onClick={handleAdd}
                                    >
                                        Add Payment
                                    </Button>
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </TableContainer>

            <PaymentFormDialog
                open={openDialog}
                onClose={handleCloseDialog}
                payment={selectedPayment}
                onSuccess={handleSuccess}
            />
        </Box>
    );
};