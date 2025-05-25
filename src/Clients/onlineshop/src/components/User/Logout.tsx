import React, { useEffect, useState } from "react";
import { logout } from "../../clients/AuthService/AuthService";
import { useAuth } from "../../contexts/authContext";
import { useNavigate } from "react-router-dom";
import LoadingSpinner from "../shared/LoadingSpinner";

const Logout: React.FC = () => {
    const { onLogout } = useAuth();
    const navigate = useNavigate();
    const [status, setStatus] = useState<"loading" | "success" | "error">(
        "loading"
    );
    const [errorMessage, setErrorMessage] = useState<string | null>(null);

    useEffect(() => {
        const doLogout = async () => {
            try {
                await logout();
                onLogout();
                setStatus("success");

                // Redirect after successful logout
                setTimeout(() => {
                    navigate("/login");
                }, 1500);
            } catch (error) {
                console.error("Logout failed:", error);
                setStatus("error");
                setErrorMessage(
                    error instanceof Error
                        ? error.message
                        : "An unexpected error occurred during logout."
                );
            }
        };

        doLogout();
    }, [onLogout, navigate]);

    return (
        <div className="max-w-md mx-auto text-center p-8">
            {status === "loading" && (
                <div className="bg-surface rounded-xl shadow-md p-8">
                    <LoadingSpinner variant="dots" />
                    <p className="mt-4 text-text">Signing out...</p>
                </div>
            )}

            {status === "success" && (
                <div className="bg-surface rounded-xl shadow-md p-8 animate-fade-in">
                    <div className="w-16 h-16 bg-primary-light rounded-full flex items-center justify-center mx-auto mb-4">
                        <svg
                            className="w-8 h-8 text-primary"
                            fill="none"
                            stroke="currentColor"
                            viewBox="0 0 24 24"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth={2}
                                d="M5 13l4 4L19 7"
                            />
                        </svg>
                    </div>
                    <h2 className="text-xl font-semibold mb-2">
                        Successfully Signed Out
                    </h2>
                    <p className="text-text-light mb-6">
                        You have been successfully logged out of your account.
                    </p>
                    <button
                        onClick={() => navigate("/login")}
                        className="btn btn-primary"
                    >
                        Sign In Again
                    </button>
                </div>
            )}

            {status === "error" && (
                <div className="bg-surface rounded-xl shadow-md p-8">
                    <div className="w-16 h-16 bg-error-light rounded-full flex items-center justify-center mx-auto mb-4">
                        <svg
                            className="w-8 h-8 text-error"
                            fill="none"
                            stroke="currentColor"
                            viewBox="0 0 24 24"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth={2}
                                d="M6 18L18 6M6 6l12 12"
                            />
                        </svg>
                    </div>
                    <h2 className="text-xl font-semibold mb-2">
                        Logout Failed
                    </h2>
                    <p className="text-text-light mb-2">
                        There was a problem signing you out.
                    </p>
                    {errorMessage && (
                        <div className="alert alert-error mb-4 text-sm">
                            {errorMessage}
                        </div>
                    )}
                    <div className="flex flex-col sm:flex-row justify-center gap-3">
                        <button
                            onClick={() => window.location.reload()}
                            className="btn btn-primary"
                        >
                            Try Again
                        </button>
                        <button
                            onClick={() => navigate("/")}
                            className="btn btn-outline"
                        >
                            Go to Home
                        </button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Logout;
