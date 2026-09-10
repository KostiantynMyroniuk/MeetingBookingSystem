import api from '../services/api';

export async function fetchMeetingRooms(pageNumber = 1, pageSize = 10) {
    const response = await api.get('/rooms', { params: { pageNumber, pageSize } });
    return response.data;
}

export async function fetchMeetingRoomById(meetingRoomId) {
    const response = await api.get(`/rooms/${meetingRoomId}`);
    return response.data;
}
