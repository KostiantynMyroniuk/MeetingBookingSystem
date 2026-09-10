import { useEffect, useState } from 'react';
import { fetchMeetingRoomById } from '../rooms/roomsApi';
import { fetchTimeSlots } from '../rooms/slotsApi';

export function RoomDetails({ meetingRoomId }) {
    const [room, setRoom] = useState(null);
    const [slots, setSlots] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        let isMounted = true;

        Promise.all([fetchMeetingRoomById(meetingRoomId), fetchTimeSlots(meetingRoomId)])
            .then(([roomData, slotsData]) => {
                if (isMounted) {
                    setRoom(roomData);
                    setSlots(slotsData.items);
                }
            })
            .catch(() => {
                if (isMounted) {
                    setError('Не вдалося завантажити дані про кімнату');
                }
            })
            .finally(() => {
                if (isMounted) {
                    setIsLoading(false);
                }
            });

        return () => {
            isMounted = false;
        };
    }, [meetingRoomId]);

    if (isLoading) {
        return <p>Завантаження кімнати...</p>;
    }

    if (error) {
        return <p role="alert">{error}</p>;
    }

    if (!room) {
        return null;
    }

    return (
        <div>
            <h2>{room.name}</h2>
            {room.description && <p>{room.description}</p>}
            {slots.length === 0 ? (
                <p>Слотів ще немає</p>
            ) : (
                <ul>
                    {slots.map((slot) => (
                        <li key={slot.id}>
                            {new Date(slot.startAt).toLocaleString()} - {new Date(slot.endAt).toLocaleString()}
                            {slot.isBooked ? ' (заброньовано)' : ' (вільно)'}
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}
