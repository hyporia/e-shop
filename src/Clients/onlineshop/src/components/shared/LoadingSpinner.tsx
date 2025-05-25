import React from "react";
import "./LoadingSpinner.css";

interface LoadingSpinnerProps {
    variant?: "spinner" | "dots";
    size?: "small" | "medium" | "large";
}

const LoadingSpinner: React.FC<LoadingSpinnerProps> = ({
    variant = "spinner",
    size = "medium",
}) => {
    if (variant === "dots") {
        return (
            <div className="spinner-container">
                <div className="dots-loader">
                    <div></div>
                    <div></div>
                    <div></div>
                </div>
            </div>
        );
    }

    // Default spinner
    const sizeClass =
        size === "small"
            ? "w-4 h-4 border-2"
            : size === "large"
            ? "w-12 h-12 border-4"
            : "w-8 h-8 border-3";

    return (
        <div className="spinner-container">
            <div className={`loading-spinner ${sizeClass}`}></div>
        </div>
    );
};

export default LoadingSpinner;
