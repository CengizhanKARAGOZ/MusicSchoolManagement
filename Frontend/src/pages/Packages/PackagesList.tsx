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
import { usePackages } from '../../hooks/usePackages';
import { PackageFormDialog } from '../../components/Forms/PackageFormDialog';
import type { Package } from '../../types/package.types';
import { useQueryClient } from '@tanstack/react-query';

export const PackagesList = () => {
    const { packages, isLoading, error, deletePackage } = usePackages();
    const queryClient = useQueryClient();
    const [openDialog, setOpenDialog] = useState(false);
    const [selectedPackage, setSelectedPackage] = useState<Package | null>(null);

    const handleAdd = () => {
        setSelectedPackage(null);
        setOpenDialog(true);
    };

    const handleEdit = (pkg: Package) => {
        setSelectedPackage(pkg);
        setOpenDialog(true);
    };

    const handleDelete = async (id: number) => {
        if (window.confirm('Are you sure you want to delete this package?')) {
            try {
                await deletePackage(id);
            } catch (err) {
                console.error('Delete failed:', err);
            }
        }
    };

    const handleCloseDialog = () => {
        setOpenDialog(false);
        setSelectedPackage(null);
    };

    const handleSuccess = () => {
        queryClient.invalidateQueries({ queryKey: ['packages'] });
    };

    if (isLoading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
                <CircularProgress />
            </Box>
        );
    }

    if (error) {
        return <Alert severity="error">Failed to load packages</Alert>;
    }

    return (
        <Box sx={{ width: '100%', height: '100%' }}>
            <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Typography variant="h4" fontWeight={600}>
                    Packages
                </Typography>
                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={handleAdd}
                    size="large"
                >
                    Add Package
                </Button>
            </Box>

            <TableContainer component={Paper} sx={{ boxShadow: 3, width: '100%', overflow: 'auto' }}>
                <Table sx={{ tableLayout: 'fixed', width: '100%' }}>
                    <TableHead>
                        <TableRow sx={{ backgroundColor: '#f5f5f5' }}>
                            <TableCell sx={{ fontWeight: 600, width: '5%' }}>ID</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '20%' }}>Name</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '30%' }}>Description</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Lessons</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Price</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }}>Validity</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '8%' }}>Status</TableCell>
                            <TableCell sx={{ fontWeight: 600, width: '10%' }} align="center">Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {packages && packages.length > 0 ? (
                            packages.map((pkg) => (
                                <TableRow
                                    key={pkg.id}
                                    sx={{ '&:hover': { backgroundColor: '#fafafa' } }}
                                >
                                    <TableCell>{pkg.id}</TableCell>
                                    <TableCell sx={{ fontWeight: 500 }}>{pkg.name}</TableCell>
                                    <TableCell
                                        sx={{
                                            overflow: 'hidden',
                                            textOverflow: 'ellipsis',
                                            whiteSpace: 'nowrap',
                                            maxWidth: 300
                                        }}
                                        title={pkg.description || ''}
                                    >
                                        {pkg.description || '-'}
                                    </TableCell>
                                    <TableCell>{pkg.lessonCount} lessons</TableCell>
                                    <TableCell>₺{pkg.price.toLocaleString()}</TableCell>
                                    <TableCell>{pkg.validityDays} days</TableCell>
                                    <TableCell>
                                        <Chip
                                            label={pkg.isActive ? 'Active' : 'Inactive'}
                                            color={pkg.isActive ? 'success' : 'default'}
                                            size="small"
                                            sx={{ fontWeight: 500 }}
                                        />
                                    </TableCell>
                                    <TableCell align="center">
                                        <Box sx={{ display: 'flex', gap: 0.5, justifyContent: 'center' }}>
                                            <IconButton
                                                color="primary"
                                                size="small"
                                                onClick={() => handleEdit(pkg)}
                                            >
                                                <EditIcon fontSize="small" />
                                            </IconButton>
                                            <IconButton
                                                color="error"
                                                size="small"
                                                onClick={() => handleDelete(pkg.id)}
                                            >
                                                <DeleteIcon fontSize="small" />
                                            </IconButton>
                                        </Box>
                                    </TableCell>
                                </TableRow>
                            ))
                        ) : (
                            <TableRow>
                                <TableCell colSpan={8} align="center" sx={{ py: 8 }}>
                                    <Typography variant="h6" color="text.secondary" gutterBottom>
                                        No packages found
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                                        Add your first package to get started!
                                    </Typography>
                                    <Button
                                        variant="contained"
                                        startIcon={<AddIcon />}
                                        onClick={handleAdd}
                                    >
                                        Add Package
                                    </Button>
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </TableContainer>

            <PackageFormDialog
                open={openDialog}
                onClose={handleCloseDialog}
                package={selectedPackage}
                onSuccess={handleSuccess}
            />
        </Box>
    );
};