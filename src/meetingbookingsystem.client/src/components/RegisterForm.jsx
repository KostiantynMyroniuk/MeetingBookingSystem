import { useState } from 'react';
import { useAuth } from '../auth/useAuth';
import { extractErrorMessage } from '../auth/authErrors';

export default function RegisterForm({ onSuccess }) {
    const { register } = useAuth();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [error, setError] = useState('');
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleSubmit = async (event) => {
        event.preventDefault();
        setError('');

        if (password !== confirmPassword) {
            setError('Паролі не співпадають');
            return;
        }

        setIsSubmitting(true);

        try {
            await register(email, password);
            onSuccess?.();
        } catch (err) {
            setError(extractErrorMessage(err, 'Не вдалося зареєструватися. Спробуйте ще раз'));
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <form onSubmit={handleSubmit} noValidate>
            <div>
                <label htmlFor="register-email">Email</label>
                <input
                    id="register-email"
                    name="email"
                    type="email"
                    value={email}
                    onChange={(event) => setEmail(event.target.value)}
                    autoComplete="email"
                    required
                />
            </div>
            <div>
                <label htmlFor="register-password">Пароль</label>
                <input
                    id="register-password"
                    name="password"
                    type="password"
                    value={password}
                    onChange={(event) => setPassword(event.target.value)}
                    autoComplete="new-password"
                    required
                />
                <small>Мінімум 6 символів, велика та мала літера, цифра і спецсимвол</small>
            </div>
            <div>
                <label htmlFor="register-confirm-password">Підтвердження пароля</label>
                <input
                    id="register-confirm-password"
                    name="confirmPassword"
                    type="password"
                    value={confirmPassword}
                    onChange={(event) => setConfirmPassword(event.target.value)}
                    autoComplete="new-password"
                    required
                />
            </div>
            {error && <p role="alert">{error}</p>}
            <button type="submit" disabled={isSubmitting}>
                {isSubmitting ? 'Реєстрація...' : 'Зареєструватися'}
            </button>
        </form>
    );
}
