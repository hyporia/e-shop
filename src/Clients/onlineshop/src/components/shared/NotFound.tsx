import React from "react";
import { useNavigate } from "react-router-dom";

const NotFound: React.FC = () => {
    const navigate = useNavigate();

    return (
        <div className="flex flex-col items-center justify-center min-h-[70vh] text-center px-4">
            <div className="w-24 h-24 bg-primary-light rounded-full flex items-center justify-center mb-6">
                <svg
                    className="w-12 h-12 text-primary"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                >
                    <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth="1.5"
                        d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                    />
                </svg>
            </div>

            <h1 className="text-4xl font-bold text-text mb-2">404</h1>
            <h2 className="text-2xl font-semibold text-text mb-4">
                Page Not Found
            </h2>
            <p className="text-text-light max-w-md mb-8">
                The page you're looking for doesn't exist or has been moved.
                Let's get you back on track.
            </p>

            <div className="flex flex-col sm:flex-row gap-4">
                <button
                    onClick={() => navigate(-1)}
                    className="btn btn-outline"
                >
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
                            d="M10 19l-7-7m0 0l7-7m-7 7h18"
                        />
                    </svg>
                    Go Back
                </button>

                <button
                    onClick={() => navigate("/")}
                    className="btn btn-primary"
                >
                    Go to Homepage
                </button>
            </div>
        </div>
    );
};

export default NotFound;
