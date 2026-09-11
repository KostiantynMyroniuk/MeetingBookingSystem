import { useEffect, useState } from 'react';
import { NavBar } from '../components/NavBar';
import { fetchAllBookings } from '../bookings/bookingsApi';

export default function AdminBookingsPage() {
    const [bookings, setBookings] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        let isMounted = true;

        fetchAllBookings()
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
            <h1>Усі бронювання</h1>
            {isLoading && <p>Завантаження...</p>}
            {error && <p role="alert">{error}</p>}
            {!isLoading && !error && bookings.length === 0 && <p>Бронювань немає</p>}
            {bookings.length > 0 && (
                <table>
                    <thead>
                        <tr>
                            <th>Кімната</th>
                            <th>Час</th>
                            <th>Хто забронював</th>
                            <th>Коли</th>
                        </tr>
                    </thead>
                    <tbody>
                        {bookings.map((booking) => (
                            <tr key={booking.id}>
                                <td>{booking.meetingRoomName}</td>
                                <td>
                                    {new Date(booking.startAt).toLocaleString()} -{' '}
                                    {new Date(booking.endAt).toLocaleString()}
                                </td>
                                <td>{booking.bookedByUserId}</td>
                                <td>{new Date(booking.bookedAtUtc).toLocaleString()}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
}
