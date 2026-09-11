import api from '../services/api';

export async function fetchMeetingRooms(pageNumber = 1, pageSize = 10) {
    const response = await api.get('/rooms', { params: { pageNumber, pageSize } });
    return response.data;
}

export async function fetchMeetingRoomById(meetingRoomId) {
    const response = await api.get(`/rooms/${meetingRoomId}`);
    return response.data;
}

export async function createMeetingRoom(name, description) {
    const response = await api.post('/rooms', { name, description });
    return response.data;
}

export async function updateMeetingRoom(meetingRoomId, name, description) {
    const response = await api.put(`/rooms/${meetingRoomId}`, { name, description });
    return response.data;
}

export async function deleteMeetingRoom(meetingRoomId) {
    await api.delete(`/rooms/${meetingRoomId}`);
}
