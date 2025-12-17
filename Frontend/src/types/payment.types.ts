export interface Payment {
    id: number;
    studentId: number;
    studentName: string;
    packageId?: number;
    packageName?: string;
    amount: number;
    paymentDate: string;
    paymentMethod: 'Cash' | 'CreditCard' | 'BankTransfer' | 'Online';
    status: 'Pending' | 'Completed' | 'Failed' | 'Refunded';
    transactionId?: string;
    notes?: string;
    createdAt: string;
    updatedAt?: string;
}

export interface CreatePaymentDto {
    studentId: number;
    packageId?: number;
    amount: number;
    paymentDate: string;
    paymentMethod: string;
    status: string;
    transactionId?: string;
    notes?: string;
}

export interface UpdatePaymentDto {
    amount: number;
    paymentDate: string;
    paymentMethod: string;
    status: string;
    transactionId?: string;
    notes?: string;
}