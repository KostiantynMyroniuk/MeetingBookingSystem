import { useCallback, useEffect, useState } from 'react';
import { fetchMeetingRoomById } from '../rooms/roomsApi';
import { fetchTimeSlots } from '../rooms/slotsApi';
import { createBooking } from '../bookings/bookingsApi';
import { useRoomSlotUpdates } from '../signalr/useRoomSlotUpdates';

export function RoomDetails({ meetingRoomId }) {
    const [room, setRoom] = useState(null);
    const [slots, setSlots] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');
    const [bookingSlotId, setBookingSlotId] = useState(null);
    const [bookingError, setBookingError] = useState('');

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

    const handleSlotBooked = useCallback((updatedSlot) => {
        setSlots((current) =>
            current.map((slot) => (slot.id === updatedSlot.id ? { ...slot, isBooked: updatedSlot.isBooked } : slot)),
        );
    }, []);

    useRoomSlotUpdates(meetingRoomId, handleSlotBooked);

    async function handleBook(slotId) {
        setBookingError('');
        setBookingSlotId(slotId);

        try {
            await createBooking(slotId);
            setSlots((current) =>
                current.map((slot) => (slot.id === slotId ? { ...slot, isBooked: true } : slot)),
            );
        } catch (err) {
            if (err.response?.status === 409) {
                setBookingError('Цей слот щойно забронював хтось інший');
                setSlots((current) =>
                    current.map((slot) => (slot.id === slotId ? { ...slot, isBooked: true } : slot)),
                );
            } else {
                setBookingError('Не вдалося забронювати слот');
            }
        } finally {
            setBookingSlotId(null);
        }
    }

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
            {bookingError && <p role="alert">{bookingError}</p>}
            {slots.length === 0 ? (
                <p>Слотів ще немає</p>
            ) : (
                <ul>
                    {slots.map((slot) => (
                        <li key={slot.id}>
                            {new Date(slot.startAt).toLocaleString()} - {new Date(slot.endAt).toLocaleString()}
                            {slot.isBooked ? (
                                ' (заброньовано)'
                            ) : (
                                <button
                                    type="button"
                                    onClick={() => handleBook(slot.id)}
                                    disabled={bookingSlotId === slot.id}
                                >
                                    {bookingSlotId === slot.id ? 'Бронюємо…' : 'Забронювати'}
                                </button>
                            )}
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}
