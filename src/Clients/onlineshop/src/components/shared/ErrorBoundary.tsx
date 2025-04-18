import React, { Component, ErrorInfo, ReactNode } from "react";
import "./ErrorBoundary.css";

interface Props {
    children: ReactNode;
}

interface State {
    hasError: boolean;
    error: Error | null;
    errorInfo: ErrorInfo | null;
    showDetails: boolean;
}

class ErrorBoundary extends Component<Props, State> {
    public state: State = {
        hasError: false,
        error: null,
        errorInfo: null,
        showDetails: false,
    };

    public static getDerivedStateFromError(error: Error): State {
        // Update state so the next render will show the fallback UI.
        return { hasError: true, error, errorInfo: null, showDetails: false };
    }

    public componentDidCatch(error: Error, errorInfo: ErrorInfo): void {
        // Log error info
        console.error("Error caught by ErrorBoundary:", error, errorInfo);
        this.setState({ errorInfo });
    }

    private handleReload = (): void => {
        window.location.reload();
    };

    private toggleDetails = (): void => {
        this.setState((prevState) => ({ showDetails: !prevState.showDetails }));
    };

    public render(): ReactNode {
        if (this.state.hasError) {
            return (
                <div className="error-boundary">
                    <svg
                        className="error-boundary__icon"
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        stroke="currentColor"
                    >
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth={1.5}
                            d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                        />
                    </svg>
                    <h2 className="error-boundary__title">
                        Something went wrong
                    </h2>
                    <p className="error-boundary__message">
                        We're sorry, but an unexpected error occurred. Our team
                        has been notified about this issue.
                    </p>
                    <div className="error-boundary__actions">
                        <button
                            className="btn btn-primary"
                            onClick={this.handleReload}
                            type="button"
                        >
                            Reload Page
                        </button>
                        <a href="/" className="btn btn-outline">
                            Go to Homepage
                        </a>
                    </div>

                    {this.state.error && (
                        <div className="error-boundary__details">
                            <button
                                className="error-boundary__details-toggle"
                                onClick={this.toggleDetails}
                                type="button"
                            >
                                <svg
                                    className="w-4 h-4"
                                    fill="none"
                                    stroke="currentColor"
                                    viewBox="0 0 24 24"
                                >
                                    <path
                                        strokeLinecap="round"
                                        strokeLinejoin="round"
                                        strokeWidth={2}
                                        d={
                                            this.state.showDetails
                                                ? "M19 9l-7 7-7-7"
                                                : "M9 5l7 7-7 7"
                                        }
                                    />
                                </svg>
                                <span>
                                    {this.state.showDetails ? "Hide" : "Show"}{" "}
                                    Technical Details
                                </span>
                            </button>

                            {this.state.showDetails && (
                                <div className="error-boundary__stack">
                                    <p>
                                        <strong>Error:</strong>{" "}
                                        {this.state.error.toString()}
                                    </p>
                                    {this.state.errorInfo && (
                                        <>
                                            <p className="mt-2">
                                                <strong>
                                                    Component Stack:
                                                </strong>
                                            </p>
                                            <pre>
                                                {
                                                    this.state.errorInfo
                                                        .componentStack
                                                }
                                            </pre>
                                        </>
                                    )}
                                </div>
                            )}
                        </div>
                    )}
                </div>
            );
        }

        return this.props.children;
    }
}

export default ErrorBoundary;
