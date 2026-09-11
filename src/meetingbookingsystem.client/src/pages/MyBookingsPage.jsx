import { useEffect, useState } from 'react';
import { NavBar } from '../components/NavBar';
import { fetchMyBookings } from '../bookings/bookingsApi';

export default function MyBookingsPage() {
    const [bookings, setBookings] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');

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

    return (
        <div className="App">
            <NavBar />
            <h1>Мої бронювання</h1>
            {isLoading && <p>Завантаження...</p>}
            {error && <p role="alert">{error}</p>}
            {!isLoading && !error && bookings.length === 0 && <p>Бронювань немає</p>}
            {bookings.length > 0 && (
                <ul>
                    {bookings.map((booking) => (
                        <li key={booking.id}>
                            {booking.meetingRoomName}: {new Date(booking.startAt).toLocaleString()} -{' '}
                            {new Date(booking.endAt).toLocaleString()}
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}
