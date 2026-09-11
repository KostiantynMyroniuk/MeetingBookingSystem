import { Navigate, Route, Routes } from 'react-router-dom';
import { RequireAdmin, RequireAuth, RedirectIfAuthenticated } from './auth/RouteGuards';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import RoomsPage from './pages/RoomsPage';
import MyBookingsPage from './pages/MyBookingsPage';
import AdminRoomsPage from './pages/AdminRoomsPage';
import AdminBookingsPage from './pages/AdminBookingsPage';

function App() {
    return (
        <Routes>
            <Route path="/" element={<Navigate to="/rooms" replace />} />
            <Route
                path="/login"
                element={
                    <RedirectIfAuthenticated>
                        <LoginPage />
                    </RedirectIfAuthenticated>
                }
            />
            <Route
                path="/register"
                element={
                    <RedirectIfAuthenticated>
                        <RegisterPage />
                    </RedirectIfAuthenticated>
                }
            />
            <Route
                path="/rooms"
                element={
                    <RequireAuth>
                        <RoomsPage />
                    </RequireAuth>
                }
            />
            <Route
                path="/rooms/:roomId"
                element={
                    <RequireAuth>
                        <RoomsPage />
                    </RequireAuth>
                }
            />
            <Route
                path="/my-bookings"
                element={
                    <RequireAuth>
                        <MyBookingsPage />
                    </RequireAuth>
                }
            />
            <Route
                path="/admin/rooms"
                element={
                    <RequireAdmin>
                        <AdminRoomsPage />
                    </RequireAdmin>
                }
            />
            <Route
                path="/admin/bookings"
                element={
                    <RequireAdmin>
                        <AdminBookingsPage />
                    </RequireAdmin>
                }
            />
            <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
    );
}

export default App;
