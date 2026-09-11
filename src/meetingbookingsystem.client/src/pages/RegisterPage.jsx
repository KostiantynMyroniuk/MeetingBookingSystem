import { Link, useNavigate } from 'react-router-dom';
import RegisterForm from '../components/RegisterForm';

export default function RegisterPage() {
    const navigate = useNavigate();

    return (
        <div className="auth-page">
            <h1>Система бронювання</h1>
            <RegisterForm onSuccess={() => navigate('/rooms')} />
            <p>
                Вже маєте акаунт? <Link to="/login">Увійти</Link>
            </p>
        </div>
    );
}
