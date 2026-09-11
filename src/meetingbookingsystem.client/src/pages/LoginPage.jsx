import { Link, useNavigate } from 'react-router-dom';
import LoginForm from '../components/LoginForm';

export default function LoginPage() {
    const navigate = useNavigate();

    return (
        <div className="auth-page">
            <h1>Система бронювання</h1>
            <LoginForm onSuccess={() => navigate('/rooms')} />
            <p>
                Немає акаунту? <Link to="/register">Зареєструватися</Link>
            </p>
        </div>
    );
}
