import { defineConfig, loadEnv } from 'vite';
import plugin from '@vitejs/plugin-react';


export default defineConfig(({ mode }) => {

    const env = loadEnv(mode, process.cwd(), '');

    return {
        plugins: [plugin()],
        server: {
            proxy: {
                '/api': {
                    target: env.API_URL,
                    changeOrigin: true,
                    secure: false,
                },
                '/hubs': {
                    target: env.API_URL,
                    changeOrigin: true,
                    secure: false,
                    ws: true
                }
            }
        }
    };
});