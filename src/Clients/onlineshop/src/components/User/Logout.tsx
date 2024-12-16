import React, { useEffect } from "react";
import { logout } from "../../services/authService";
import { useNavigate } from "react-router-dom";

const Logout: React.FC = () => {
    const navigate = useNavigate();
    useEffect(() => {
        const doLogout = async () => {
            await logout();
            navigate("/");
        };

        doLogout();
    }, []);

    return <div>Logging out...</div>;
};

export default Logout;
