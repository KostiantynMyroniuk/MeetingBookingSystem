import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';


export default defineConfig({
    plugins: [plugin()],
    server: {
        proxy: {
            '/api': {
                target: 'localhost:5241',
                changeOrigin: true,
                secure: false,
            },
            '/hubs': {
                target: 'localhost:5241',
                changeOrigin: true,
                secure: false,
                ws: true,
            },
        },
    },
});