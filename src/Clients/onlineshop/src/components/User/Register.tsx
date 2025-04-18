import React, { useState } from "react";
import { postAccountRegister } from "../../clients/UserService";
import { useNavigate, NavLink } from "react-router-dom";

const Register: React.FC = () => {
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        setSuccess(null);

        if (password !== confirmPassword) {
            setError("Passwords do not match");
            return;
        }

        setLoading(true);
        try {
            const response = await postAccountRegister({
                body: {
                    email: email || username,
                    username,
                    password,
                },
            });

            if (response.status === 200) {
                setSuccess("Registration successful! Redirecting to login...");
                setTimeout(() => navigate("/login"), 2000);
            } else {
                const errorDetail = response.error
                    ? JSON.stringify(response.error)
                    : `HTTP error ${response.status}`;
                setError(`Registration failed: ${errorDetail}`);
            }
        } catch (err) {
            console.error("Registration failed:", err);
            setError(
                err instanceof Error
                    ? err.message
                    : "An unexpected error occurred during registration."
            );
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="max-w-md w-full mx-auto">
            <div className="text-center mb-8">
                <h1 className="text-3xl font-bold">Create Account</h1>
                <p className="text-text-light mt-2">
                    Join us and start shopping
                </p>
            </div>

            <div className="bg-surface rounded-xl shadow-md p-6">
                {error && <div className="alert alert-error mb-4">{error}</div>}

                {success && (
                    <div className="alert alert-success mb-4">{success}</div>
                )}

                <form onSubmit={handleSubmit} className="space-y-4">
                    <div className="form-group">
                        <label htmlFor="username" className="form-label">
                            Username
                        </label>
                        <input
                            type="text"
                            id="username"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            className="form-input"
                            placeholder="Choose a username"
                            required
                            disabled={loading || !!success}
                        />
                    </div>

                    <div className="form-group">
                        <label htmlFor="email" className="form-label">
                            Email
                        </label>
                        <input
                            type="email"
                            id="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            className="form-input"
                            placeholder="Enter your email"
                            required
                            disabled={loading || !!success}
                        />
                    </div>

                    <div className="form-group">
                        <label htmlFor="password" className="form-label">
                            Password
                        </label>
                        <input
                            type="password"
                            id="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            className="form-input"
                            placeholder="Create a password"
                            required
                            minLength={6}
                            disabled={loading || !!success}
                        />
                        <p className="text-xs text-text-light mt-1">
                            Password must be at least 6 characters
                        </p>
                    </div>

                    <div className="form-group">
                        <label htmlFor="confirmPassword" className="form-label">
                            Confirm Password
                        </label>
                        <input
                            type="password"
                            id="confirmPassword"
                            value={confirmPassword}
                            onChange={(e) => setConfirmPassword(e.target.value)}
                            className="form-input"
                            placeholder="Confirm your password"
                            required
                            disabled={loading || !!success}
                        />
                    </div>

                    <div className="pt-2">
                        <button
                            type="submit"
                            className="btn btn-primary btn-lg w-full"
                            disabled={loading || !!success}
                        >
                            {loading ? (
                                <>
                                    <span className="loading-spinner w-4 h-4 border-2 mr-2"></span>
                                    Creating Account...
                                </>
                            ) : success ? (
                                <>
                                    <svg
                                        className="w-5 h-5 mr-2"
                                        fill="none"
                                        stroke="currentColor"
                                        viewBox="0 0 24 24"
                                    >
                                        <path
                                            strokeLinecap="round"
                                            strokeLinejoin="round"
                                            strokeWidth="2"
                                            d="M5 13l4 4L19 7"
                                        />
                                    </svg>
                                    Account Created
                                </>
                            ) : (
                                "Create Account"
                            )}
                        </button>
                    </div>

                    <div className="text-center mt-4">
                        <p className="text-sm text-text-light">
                            By creating an account, you agree to our{" "}
                            <a
                                href="#"
                                className="text-primary hover:underline"
                            >
                                Terms of Service
                            </a>{" "}
                            and{" "}
                            <a
                                href="#"
                                className="text-primary hover:underline"
                            >
                                Privacy Policy
                            </a>
                        </p>
                    </div>
                </form>

                <div className="border-t border-gray-200 mt-6 pt-6">
                    <p className="text-center text-sm text-text-light">
                        Already have an account?{" "}
                        <NavLink
                            to="/login"
                            className="text-primary hover:underline font-medium"
                        >
                            Sign in
                        </NavLink>
                    </p>
                </div>
            </div>
        </div>
    );
};

export default Register;
