import { useCallback, useEffect, useState } from 'react';
import { AuthContext } from './AuthContext';
import { fetchCurrentUser, loginUser, logoutUser, registerUser } from './authApi';

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

    const logout = useCallback(async () => {
        await logoutUser();
        setUser(null);
    }, []);

    const roles = user?.roles ?? [];

    const value = {
        user,
        roles,
        isAuthenticated: Boolean(user),
        isAdmin: roles.includes('Admin'),
        isAuthLoading,
        login,
        register,
        logout,
    };

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
