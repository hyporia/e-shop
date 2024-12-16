import React, { useEffect } from "react";
import { logout } from "../../services/authService";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../utils/authContext";

const Logout: React.FC = () => {
    const navigate = useNavigate();
    const { onLogout } = useAuth();
    useEffect(() => {
        const doLogout = async () => {
            await logout();
            onLogout();
            navigate("/");
        };

        doLogout();
    }, []);

    return <div>Logging out...</div>;
};

export default Logout;
