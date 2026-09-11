import api from '../services/api';

export async function createBooking(timeSlotId) {
    const response = await api.post('/bookings', { timeSlotId });
    return response.data;
}

export async function fetchMyBookings(pageNumber = 1, pageSize = 10) {
    const response = await api.get('/bookings/my-bookings', { params: { pageNumber, pageSize } });
    return response.data;
}

export async function fetchAllBookings(pageNumber = 1, pageSize = 10) {
    const response = await api.get('/bookings/all', { params: { pageNumber, pageSize } });
    return response.data;
}
