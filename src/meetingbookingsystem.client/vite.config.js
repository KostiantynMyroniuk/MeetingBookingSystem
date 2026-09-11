import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
const API_URL = import.meta.env.VITE_API_URL;

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    server: {
        proxy: {
            '/api': {
                target: API_URL,
                changeOrigin: true,
                secure: false,
            },
            '/hubs': {
                target: API_URL,
                changeOrigin: true,
                secure: false,
                ws: true
            }
        }
    }
})