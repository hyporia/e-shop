import { useEffect, useState } from "react";
import { AuthContext } from "../../contexts/authContext";

import { ReactNode } from "react";
import { getUser, User } from "../../services/AuthService";

export const AuthProvider = ({
    children,
}: {
    children: ReactNode;
}): JSX.Element => {
    const [user, setUser] = useState<User | null>(null);
    const onLogin = (user: User) => {
        setUser(user);
    };
    const onLogout = () => {
        setUser(null);
    };
    useEffect(() => {
        const loadUser = async () => {
            const storedUser = await getUser();
            if (storedUser) {
                setUser(storedUser);
            }
        };
        loadUser();
    }, []);
    const isAuthenticated = !!user;
    return (
        <AuthContext.Provider value={{ isAuthenticated, onLogin, onLogout }}>
            {children}
        </AuthContext.Provider>
    );
};
