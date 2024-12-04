import React, { useState } from "react";
import { login } from "../../services/AuthService";
import { useAuth } from "../../utils/authContext";
import { redirect } from "react-router-dom";

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
		</form>
	);
};

export default Login;
