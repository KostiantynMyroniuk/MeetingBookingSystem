import { Navigate, Route, Routes } from 'react-router-dom';
import { RequireAuth, RedirectIfAuthenticated } from './auth/RouteGuards';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import RoomsPage from './pages/RoomsPage';

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
            <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
    );
}

export default App;
