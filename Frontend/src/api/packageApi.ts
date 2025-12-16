import axiosIstance from './axios.config';
import type { ApiResponse } from '../types/common.types';
import type { Package, CreatePackageDto, UpdatePackageDto } from '../types/package.types';

export const packageApi = {
    getAll: async (): Promise<Package[]> => {
        const response = await axiosIstance.get<ApiResponse<Package[]>>('/Packages');
        return response.data.data;
    },

    getById: async (id: number): Promise<Package> => {
        const response = await axiosIstance.get<ApiResponse<Package>>(`/Packages/${id}`);
        return response.data.data;
    },
    
    create: async (data: CreatePackageDto): Promise<Package> => {
        const response = await axiosIstance.post<ApiResponse<Package>>('/Packages', data);
        return response.data.data;
    },
    
    update: async (id: number, data: UpdatePackageDto): Promise<Package> => {
        const response = await axiosIstance.put<ApiResponse<Package>>(`/Packages/${id}`, data);
        return response.data.data;
    },
    
    delete: async (id: number): Promise<void> => {
        await axiosIstance.delete(`/Packages/${id}`);
    },
};
