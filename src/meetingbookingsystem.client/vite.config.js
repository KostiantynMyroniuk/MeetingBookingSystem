import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    server: {
        proxy: {
            '/api': {
                target: 'http://localhost:5241',
                changeOrigin: true,
                secure: false,
            },
            '/hubs': {
                target: 'http://localhost:5241',
                changeOrigin: true,
                secure: false,
                ws: true
            }
        }
    }
})