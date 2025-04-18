import React, { useState } from "react";
import { useAuth } from "../../contexts/authContext";
import { redirect, NavLink } from "react-router-dom";
import { login } from "../../services/AuthService";

const Login: React.FC = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const { onLogin } = useAuth();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const user = await login({
            username,
            password,
            skipUserInfo: false,
        });
        if (user) {
            onLogin(user);
            redirect("/profile");
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <div>
                <label htmlFor="username">Username:</label>
                <input
                    type="text"
                    id="username"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                />
            </div>
            <div>
                <label htmlFor="password">Password:</label>
                <input
                    type="password"
                    id="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
            </div>
            <button type="submit">Login</button>
            <p className="mt-4 text-center text-gray-600">
                Don't have an account?{" "}
                <NavLink
                    to="/register"
                    className="text-blue-600 hover:underline"
                >
                    Register here
                </NavLink>
            </p>
        </form>
    );
};

export default Login;
