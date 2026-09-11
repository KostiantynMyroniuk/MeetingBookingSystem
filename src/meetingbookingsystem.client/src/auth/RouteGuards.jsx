import { Navigate } from 'react-router-dom';
import { useAuth } from './useAuth';

export function RequireAuth({ children }) {
    const { isAuthenticated, isAuthLoading } = useAuth();

    if (isAuthLoading) {
        return <p>Завантаження...</p>;
    }

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    return children;
}

export function RequireAdmin({ children }) {
    const { isAuthenticated, isAdmin, isAuthLoading } = useAuth();

    if (isAuthLoading) {
        return <p>Завантаження...</p>;
    }

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    if (!isAdmin) {
        return <Navigate to="/rooms" replace />;
    }

    return children;
}

export function RedirectIfAuthenticated({ children }) {
    const { isAuthenticated, isAuthLoading } = useAuth();

    if (isAuthLoading) {
        return <p>Завантаження...</p>;
    }

    if (isAuthenticated) {
        return <Navigate to="/rooms" replace />;
    }

    return children;
}
