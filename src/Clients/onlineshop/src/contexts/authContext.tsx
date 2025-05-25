import { createContext, useContext } from "react";
import { User } from "../clients/AuthService/AuthService";

export interface AuthContextType {
    isAuthenticated: boolean;
    onLogin: (user: User) => void;
    onLogout: () => void;
    login: (username: string, password: string) => Promise<void>;
}

export const AuthContext = createContext<AuthContextType | null>(null);

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth must be used within an AuthProvider");
    }
    return context;
};
