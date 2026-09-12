import { useEffect, useState } from 'react';
import { fetchMeetingRooms } from '../rooms/roomsApi';

export function RoomList({ selectedRoomId, onSelectRoom }) {
    const [rooms, setRooms] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        let isMounted = true;

        fetchMeetingRooms()
            .then((data) => {
                if (isMounted) {
                    setRooms(data.items);
                }
            })
            .catch(() => {
                if (isMounted) {
                    setError('Не вдалося завантажити список кімнат');
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
    }, []);

    if (isLoading) {
        return null;
    }

    if (error) {
        return <p role="alert">{error}</p>;
    }

    if (rooms.length === 0) {
        return <p>Кімнат ще немає</p>;
    }

    return (
        <ul className="room-list">
            {rooms.map((room) => (
                <li key={room.id}>
                    <button
                        type="button"
                        className={`room-card${room.id === selectedRoomId ? ' room-card--selected' : ''}`}
                        onClick={() => onSelectRoom(room.id)}
                    >
                        <span>{room.name}</span>
                        {room.description && <span className="room-card__description">{room.description}</span>}
                    </button>
                </li>
            ))}
        </ul>
    );
}
