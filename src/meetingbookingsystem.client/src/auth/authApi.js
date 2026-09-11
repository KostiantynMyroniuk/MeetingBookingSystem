import api from '../services/api';

export async function registerUser(email, password) {
    await api.post('/register', { email, password });
}

export async function loginUser(email, password) {
    await api.post('/login?useCookies=true', { email, password });
}

export async function fetchCurrentUser() {
    const response = await api.get('/me');
    return response.data;
}

export async function logoutUser() {
    await api.post('/logout');
}
