import { Link } from 'react-router-dom';
import { useAuth } from '../auth/useAuth';

export function NavBar() {
    const { user, isAdmin, logout } = useAuth();

    return (
        <nav>
            <Link to="/rooms">Кімнати</Link>
            {' | '}
            <Link to="/my-bookings">Мої бронювання</Link>
            {isAdmin && (
                <>
                    {' | '}
                    <Link to="/admin/rooms">Керування кімнатами</Link>
                    {' | '}
                    <Link to="/admin/bookings">Усі бронювання</Link>
                </>
            )}
            {' | '}
            <span>{user?.email}</span>
            {' '}
            <button type="button" onClick={logout}>
                Вийти
            </button>
        </nav>
    );
}
