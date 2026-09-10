import { useState } from 'react';
import { useAuth } from '../auth/useAuth';
import { extractErrorMessage } from '../auth/authErrors';

export default function LoginForm({ onSuccess }) {
    const { login } = useAuth();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleSubmit = async (event) => {
        event.preventDefault();
        setError('');
        setIsSubmitting(true);

        try {
            await login(email, password);
            onSuccess?.();
        } catch (err) {
            setError(extractErrorMessage(err, 'Не вдалося увійти. Спробуйте ще раз'));
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <form onSubmit={handleSubmit} noValidate>
            <div>
                <label htmlFor="login-email">Email</label>
                <input
                    id="login-email"
                    name="email"
                    type="email"
                    value={email}
                    onChange={(event) => setEmail(event.target.value)}
                    autoComplete="email"
                    required
                />
            </div>
            <div>
                <label htmlFor="login-password">Пароль</label>
                <input
                    id="login-password"
                    name="password"
                    type="password"
                    value={password}
                    onChange={(event) => setPassword(event.target.value)}
                    autoComplete="current-password"
                    required
                />
            </div>
            {error && <p role="alert">{error}</p>}
            <button type="submit" disabled={isSubmitting}>
                {isSubmitting ? 'Вхід...' : 'Увійти'}
            </button>
        </form>
    );
}
