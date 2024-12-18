import React, { useEffect } from "react";
import { logout } from "../../services/authService";
import { useAuth } from "../../utils/authContext";

const Logout: React.FC = () => {
    const { onLogout } = useAuth();
    useEffect(() => {
        const doLogout = async () => {
            await logout();
            onLogout();
        };

        doLogout();
    }, []);

    return <div>Logging out...</div>;
};

export default Logout;
