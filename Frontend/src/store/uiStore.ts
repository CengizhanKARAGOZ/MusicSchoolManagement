import { create } from 'zustand';

interface UiState {
    sidebarOpen: boolean;
    loading: boolean;
    toggleSidebar: () => void;
    setSidebarOpen: (open: boolean) => void;
    setLoading: (loading: boolean) => void;
}

export const useUiStore = create<UiState>((set) => ({
    sidebarOpen: true,
    loading: false,

    toggleSidebar: () => set((state) => ({ sidebarOpen: !state.sidebarOpen })),

    setSidebarOpen: (open: boolean) => set({ sidebarOpen: open }),

    setLoading: (loading: boolean) => set({ loading }),
}));