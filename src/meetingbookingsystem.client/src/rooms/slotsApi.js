import api from '../services/api';

export async function fetchTimeSlots(meetingRoomId, pageNumber = 1, pageSize = 10) {
    const response = await api.get(`/rooms/${meetingRoomId}/slots`, { params: { pageNumber, pageSize } });
    return response.data;
}
