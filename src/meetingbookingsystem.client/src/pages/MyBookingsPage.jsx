import { useEffect, useState } from 'react';
import { NavBar } from '../components/NavBar';
import { cancelBooking, fetchMyBookings } from '../bookings/bookingsApi';

export default function MyBookingsPage() {
    const [bookings, setBookings] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');
    const [cancellingId, setCancellingId] = useState(null);

    useEffect(() => {
        let isMounted = true;

        fetchMyBookings()
            .then((data) => {
                if (isMounted) {
                    setBookings(data.items);
                }
            })
            .catch(() => {
                if (isMounted) {
                    setError('Не вдалося завантажити бронювання');
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

    async function handleCancel(bookingId) {
        setError('');
        setCancellingId(bookingId);

        try {
            await cancelBooking(bookingId);
            setBookings((current) => current.filter((booking) => booking.id !== bookingId));
        } catch {
            setError('Не вдалося скасувати бронювання');
        } finally {
            setCancellingId(null);
        }
    }

    return (
        <div className="App">
            <NavBar />
            <h1>Мої бронювання</h1>
            {error && <p role="alert">{error}</p>}
            {!isLoading && !error && bookings.length === 0 && <p>Бронювань немає</p>}
            {bookings.length > 0 && (
                <ul className="bookings-list">
                    {bookings.map((booking) => (
                        <li key={booking.id}>
                            <span>
                                {booking.meetingRoomName}: {new Date(booking.startAt).toLocaleString()} -{' '}
                                {new Date(booking.endAt).toLocaleString()}
                            </span>
                            <button
                                type="button"
                                className="btn-danger"
                                onClick={() => handleCancel(booking.id)}
                                disabled={cancellingId === booking.id}
                            >
                                {cancellingId === booking.id ? 'Скасовуємо…' : 'Скасувати'}
                            </button>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}
