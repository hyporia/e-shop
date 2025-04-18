import { useContext, useEffect, useState } from "react";
import { AuthContext, AuthContextType } from "../contexts/authContext";
import { ReactNode } from "react";
import {
    User,
    login,
    getUser,
    logout,
} from "../clients/AuthService/AuthService";
import { SigninResourceOwnerCredentialsArgs } from "oidc-client-ts";

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth must be used within an AuthProvider");
    }
    return context;
};

export const AuthProvider = ({
    children,
}: {
    children: ReactNode;
}): JSX.Element => {
    const [user, setUser] = useState<User | null>(null);

    const handleLogin = async (username: string, password: string) => {
        const user = await login({
            username,
            password,
        } as SigninResourceOwnerCredentialsArgs);
        setUser(user);
    };

    const onLogin = (user: User) => {
        setUser(user);
    };

    const onLogout = async () => {
        await logout();
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

    const contextValue = {
        isAuthenticated,
        onLogin,
        onLogout,
        login: handleLogin,
    } satisfies AuthContextType;

    return (
        <AuthContext.Provider value={contextValue}>
            {children}
        </AuthContext.Provider>
    );
};
