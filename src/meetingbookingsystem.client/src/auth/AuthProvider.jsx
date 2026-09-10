import { useCallback, useEffect, useState } from 'react';
import { AuthContext } from './AuthContext';
import { fetchCurrentUser, loginUser, registerUser } from './authApi';

export default function AuthProvider({ children }) {
    const [user, setUser] = useState(null);
    const [isAuthLoading, setIsAuthLoading] = useState(true);

    useEffect(() => {
        let isMounted = true;

        fetchCurrentUser()
            .then((currentUser) => {
                if (isMounted) {
                    setUser(currentUser);
                }
            })
            .catch(() => {
                if (isMounted) {
                    setUser(null);
                }
            })
            .finally(() => {
                if (isMounted) {
                    setIsAuthLoading(false);
                }
            });

        return () => {
            isMounted = false;
        };
    }, []);

    const login = useCallback(async (email, password) => {
        await loginUser(email, password);
        const currentUser = await fetchCurrentUser();
        setUser(currentUser);
        return currentUser;
    }, []);

    const register = useCallback(async (email, password) => {
        await registerUser(email, password);
        return login(email, password);
    }, [login]);

    const value = {
        user,
        isAuthenticated: Boolean(user),
        isAuthLoading,
        login,
        register,
    };

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
