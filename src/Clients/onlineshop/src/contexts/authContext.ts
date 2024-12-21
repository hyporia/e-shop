import { createContext, useContext } from "react";
import { User } from "../services/AuthService";

// Define the types for authentication context
interface AuthContextType {
    isAuthenticated: boolean;
    onLogin: (user: User) => void;
    onLogout: () => void;
}

// Create authentication context
export const AuthContext = createContext<AuthContextType | undefined>(
    undefined
);

// Custom hook to use authentication context
export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth must be used within an AuthProvider");
    }
    return context;
};
