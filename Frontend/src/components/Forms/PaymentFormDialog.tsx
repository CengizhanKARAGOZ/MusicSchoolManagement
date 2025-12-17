import { useEffect, useState } from 'react';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    TextField,
    Box,
    MenuItem,
    FormControl,
    InputLabel,
    Select,
} from '@mui/material';
import { useForm, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { useSnackbar } from 'notistack';
import type { Payment } from '../../types/payment.types';
import { usePayments } from '../../hooks/usePayments';
import axiosInstance from '../../api/axios.config';

const schema = yup.object({
    studentId: yup.number().required('Student is required').min(1, 'Please select a student'),
    packageId: yup.number().nullable(),
    amount: yup.number().required('Amount is required').min(0, 'Amount must be positive'),
    paymentDate: yup.string().required('Payment date is required'),
    paymentMethod: yup.string().required('Payment method is required'),
    status: yup.string().required('Status is required'),
    transactionId: yup.string().max(100),
    notes: yup.string().max(500),
});

interface PaymentFormDialogProps {
    open: boolean;
    onClose: () => void;
    payment: Payment | null;
    onSuccess: () => void;
}

interface SelectOption {
    id: number;
    name: string;
}

export const PaymentFormDialog = ({ open, onClose, payment, onSuccess }: PaymentFormDialogProps) => {
    const { enqueueSnackbar } = useSnackbar();
    const { createPayment, updatePayment } = usePayments();
    const isEdit = !!payment;

    const [students, setStudents] = useState<SelectOption[]>([]);
    const [packages, setPackages] = useState<SelectOption[]>([]);
    const [loading, setLoading] = useState(false);

    const {
        control,
        handleSubmit,
        reset,
        formState: { errors, isSubmitting },
    } = useForm({
        resolver: yupResolver(schema),
        defaultValues: {
            studentId: 0,
            packageId: null as number | null,
            amount: 0,
            paymentDate: new Date().toISOString().split('T')[0],
            paymentMethod: 'Cash',
            status: 'Completed',
            transactionId: '',
            notes: '',
        },
    });

    // Load dropdown data
    useEffect(() => {
        const loadData = async () => {
            setLoading(true);
            try {
                const [studentsRes, packagesRes] = await Promise.all([
                    axiosInstance.get('/Students'),
                    axiosInstance.get('/Packages'),
                ]);

                setStudents(
                    studentsRes.data.data.map((s: any) => ({
                        id: s.id,
                        name: `${s.firstName} ${s.lastName}`,
                    }))
                );

                setPackages(
                    packagesRes.data.data.map((p: any) => ({
                        id: p.id,
                        name: p.name,
                    }))
                );
            } catch (error) {
                console.error('Failed to load data:', error);
                enqueueSnackbar('Failed to load form data', { variant: 'error' });
            } finally {
                setLoading(false);
            }
        };

        if (open) {
            loadData();
        }
    }, [open, enqueueSnackbar]);

    useEffect(() => {
        if (payment) {
            reset({
                studentId: payment.studentId,
                packageId: payment.packageId || null,
                amount: payment.amount,
                paymentDate: payment.paymentDate.split('T')[0],
                paymentMethod: payment.paymentMethod,
                status: payment.status,
                transactionId: payment.transactionId || '',
                notes: payment.notes || '',
            });
        } else {
            reset({
                studentId: 0,
                packageId: null,
                amount: 0,
                paymentDate: new Date().toISOString().split('T')[0],
                paymentMethod: 'Cash',
                status: 'Completed',
                transactionId: '',
                notes: '',
            });
        }
    }, [payment, reset]);

    const onSubmit = async (data: any) => {
        try {
            if (isEdit) {
                await updatePayment({
                    id: payment.id,
                    data: {
                        amount: data.amount,
                        paymentDate: data.paymentDate,
                        paymentMethod: data.paymentMethod,
                        status: data.status,
                        transactionId: data.transactionId,
                        notes: data.notes,
                    }
                });
                enqueueSnackbar('Payment updated successfully!', { variant: 'success' });
            } else {
                await createPayment({
                    studentId: data.studentId,
                    packageId: data.packageId,
                    amount: data.amount,
                    paymentDate: data.paymentDate,
                    paymentMethod: data.paymentMethod,
                    status: data.status,
                    transactionId: data.transactionId,
                    notes: data.notes,
                });
                enqueueSnackbar('Payment created successfully!', { variant: 'success' });
            }
            onSuccess();
            onClose();
        } catch (error: any) {
            enqueueSnackbar(error.response?.data?.message || 'Operation failed', { variant: 'error' });
        }
    };

    return (
        <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
            <DialogTitle>{isEdit ? 'Edit Payment' : 'Add New Payment'}</DialogTitle>
            <form onSubmit={handleSubmit(onSubmit)}>
                <DialogContent>
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                        {/* Student Select */}
                        <Controller
                            name="studentId"
                            control={control}
                            render={({ field }) => (
                                <FormControl fullWidth error={!!errors.studentId} disabled={isEdit}>
                                    <InputLabel>Student</InputLabel>
                                    <Select {...field} label="Student" disabled={loading || isEdit}>
                                        <MenuItem value={0}>Select Student</MenuItem>
                                        {students.map((student) => (
                                            <MenuItem key={student.id} value={student.id}>
                                                {student.name}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                    {errors.studentId && (
                                        <Box sx={{ color: 'error.main', fontSize: '0.75rem', mt: 0.5 }}>
                                            {errors.studentId.message}
                                        </Box>
                                    )}
                                </FormControl>
                            )}
                        />

                        {/* Package Select (Optional) */}
                        <Controller
                            name="packageId"
                            control={control}
                            render={({ field }) => (
                                <FormControl fullWidth>
                                    <InputLabel>Package (Optional)</InputLabel>
                                    <Select {...field} label="Package (Optional)" disabled={loading}>
                                        <MenuItem value="">None</MenuItem>
                                        {packages.map((pkg) => (
                                            <MenuItem key={pkg.id} value={pkg.id}>
                                                {pkg.name}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                            )}
                        />

                        {/* Amount & Date */}
                        <Box sx={{ display: 'flex', gap: 2 }}>
                            <Controller
                                name="amount"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label="Amount (₺)"
                                        type="number"
                                        fullWidth
                                        error={!!errors.amount}
                                        helperText={errors.amount?.message}
                                    />
                                )}
                            />

                            <Controller
                                name="paymentDate"
                                control={control}
                                render={({ field }) => (
                                    <TextField
                                        {...field}
                                        label="Payment Date"
                                        type="date"
                                        fullWidth
                                        InputLabelProps={{ shrink: true }}
                                        error={!!errors.paymentDate}
                                        helperText={errors.paymentDate?.message}
                                    />
                                )}
                            />
                        </Box>

                        {/* Payment Method & Status */}
                        <Box sx={{ display: 'flex', gap: 2 }}>
                            <Controller
                                name="paymentMethod"
                                control={control}
                                render={({ field }) => (
                                    <FormControl fullWidth error={!!errors.paymentMethod}>
                                        <InputLabel>Payment Method</InputLabel>
                                        <Select {...field} label="Payment Method">
                                            <MenuItem value="Cash">Cash</MenuItem>
                                            <MenuItem value="CreditCard">Credit Card</MenuItem>
                                            <MenuItem value="BankTransfer">Bank Transfer</MenuItem>
                                            <MenuItem value="Online">Online</MenuItem>
                                        </Select>
                                        {errors.paymentMethod && (
                                            <Box sx={{ color: 'error.main', fontSize: '0.75rem', mt: 0.5 }}>
                                                {errors.paymentMethod.message}
                                            </Box>
                                        )}
                                    </FormControl>
                                )}
                            />

                            <Controller
                                name="status"
                                control={control}
                                render={({ field }) => (
                                    <FormControl fullWidth error={!!errors.status}>
                                        <InputLabel>Status</InputLabel>
                                        <Select {...field} label="Status">
                                            <MenuItem value="Pending">Pending</MenuItem>
                                            <MenuItem value="Completed">Completed</MenuItem>
                                            <MenuItem value="Failed">Failed</MenuItem>
                                            <MenuItem value="Refunded">Refunded</MenuItem>
                                        </Select>
                                        {errors.status && (
                                            <Box sx={{ color: 'error.main', fontSize: '0.75rem', mt: 0.5 }}>
                                                {errors.status.message}
                                            </Box>
                                        )}
                                    </FormControl>
                                )}
                            />
                        </Box>

                        {/* Transaction ID */}
                        <Controller
                            name="transactionId"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Transaction ID (Optional)"
                                    fullWidth
                                    error={!!errors.transactionId}
                                    helperText={errors.transactionId?.message}
                                    placeholder="e.g., TXN123456789"
                                />
                            )}
                        />

                        {/* Notes */}
                        <Controller
                            name="notes"
                            control={control}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Notes"
                                    fullWidth
                                    multiline
                                    rows={3}
                                    error={!!errors.notes}
                                    helperText={errors.notes?.message}
                                    placeholder="Additional notes..."
                                />
                            )}
                        />
                    </Box>
                </DialogContent>
                <DialogActions>
                    <Button onClick={onClose}>Cancel</Button>
                    <Button type="submit" variant="contained" disabled={isSubmitting || loading}>
                        {isSubmitting ? 'Saving...' : isEdit ? 'Update' : 'Create'}
                    </Button>
                </DialogActions>
            </form>
        </Dialog>
    );
};