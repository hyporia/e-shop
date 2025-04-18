import { useEffect, useState } from "react";
import {
    getUser,
    User,
    renewToken,
} from "../../clients/AuthService/AuthService";
import { useNavigate } from "react-router-dom";
import LoadingSpinner from "../shared/LoadingSpinner";

const Profile = (): JSX.Element => {
    const [user, setUser] = useState<User | null>(null);
    const [loading, setLoading] = useState(true);
    const [renewing, setRenewing] = useState(false);
    const [renewSuccess, setRenewSuccess] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        const checkUser = async () => {
            try {
                setLoading(true);
                const user = await getUser();
                setUser(user);
            } catch (error) {
                console.error("Error fetching user:", error);
            } finally {
                setLoading(false);
            }
        };

        checkUser();
    }, []);

    const handleRenewToken = async () => {
        try {
            setRenewing(true);
            await renewToken();
            setRenewSuccess(true);
            setTimeout(() => setRenewSuccess(false), 3000);
        } catch (error) {
            console.error("Token renewal failed:", error);
        } finally {
            setRenewing(false);
        }
    };

    if (loading) {
        return (
            <div className="py-16">
                <LoadingSpinner />
            </div>
        );
    }

    if (!user) {
        return (
            <div className="bg-surface rounded-xl shadow-md p-8 max-w-lg mx-auto text-center">
                <svg
                    className="w-16 h-16 mx-auto text-gray-400 mb-4"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                >
                    <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={1}
                        d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"
                    />
                </svg>
                <h2 className="text-2xl font-semibold mb-2">Not logged in</h2>
                <p className="text-text-light mb-6">
                    You need to be logged in to view your profile.
                </p>
                <div className="flex flex-col sm:flex-row justify-center gap-3">
                    <button
                        onClick={() => navigate("/login")}
                        className="btn btn-primary"
                    >
                        Sign In
                    </button>
                    <button
                        onClick={() => navigate("/register")}
                        className="btn btn-outline"
                    >
                        Create Account
                    </button>
                </div>
            </div>
        );
    }

    return (
        <div className="fade-in">
            <div className="bg-surface rounded-xl shadow-md overflow-hidden max-w-3xl mx-auto">
                {/* Profile header */}
                <div className="relative bg-primary p-6 pb-24 text-white">
                    <div className="flex justify-between">
                        <h1 className="text-2xl font-bold">My Profile</h1>
                        <button
                            onClick={handleRenewToken}
                            className="bg-white/20 hover:bg-white/30 text-white text-sm py-1 px-3 rounded-full transition-colors flex items-center"
                            disabled={renewing}
                        >
                            {renewing ? (
                                <>
                                    <span className="loading-spinner w-3 h-3 border-2 mr-2"></span>
                                    Renewing...
                                </>
                            ) : renewSuccess ? (
                                <>
                                    <svg
                                        className="w-4 h-4 mr-1"
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
                                    Token Renewed
                                </>
                            ) : (
                                "Renew Token"
                            )}
                        </button>
                    </div>
                </div>

                {/* User info with avatar */}
                <div className="relative px-6 pt-0">
                    <div className="flex flex-col sm:flex-row items-start sm:items-center -mt-16 mb-6">
                        {/* Avatar */}
                        <div className="w-24 h-24 rounded-full border-4 border-surface bg-gray-200 overflow-hidden flex-shrink-0 shadow-md mb-4 sm:mb-0 sm:mr-4">
                            <img
                                src={`https://ui-avatars.com/api/?name=${encodeURIComponent(
                                    user.name || "User"
                                )}&background=3b82f6&color=fff&size=128`}
                                alt={user.name || "User"}
                                className="w-full h-full object-cover"
                            />
                        </div>

                        {/* User details */}
                        <div>
                            <h2 className="text-2xl font-bold">
                                {user.name || "User"}
                            </h2>
                            <p className="text-text-light">
                                {/* {user.email || ""} */}
                                {"example@example.com"}
                            </p>
                            <p className="text-text-light">
                                {/* {user.address || "No address saved"} */}
                                {"123 Main St, Springfield, USA"}
                            </p>
                        </div>
                    </div>
                </div>

                {/* User info cards */}
                <div className="p-6 pt-0 grid gap-4 sm:grid-cols-2">
                    <div className="border border-gray-200 rounded-lg p-4">
                        <h3 className="text-sm font-medium text-text-light mb-1">
                            User ID
                        </h3>
                        <p className="font-medium">{user.id}</p>
                    </div>

                    <div className="border border-gray-200 rounded-lg p-4">
                        <h3 className="text-sm font-medium text-text-light mb-1">
                            Member Since
                        </h3>
                        <p className="font-medium">
                            {new Date().toLocaleDateString()}
                        </p>
                    </div>

                    <div className="border border-gray-200 rounded-lg p-4 sm:col-span-2">
                        <h3 className="text-sm font-medium text-text-light mb-1">
                            Shipping Address
                        </h3>
                        <p className="font-medium">
                            {/* {user.address || "No address saved"} */}
                            {"123 Main St, Springfield, USA"}
                        </p>
                    </div>
                </div>

                {/* Actions section */}
                <div className="border-t border-gray-200 p-6">
                    <h3 className="font-semibold mb-4">Account Actions</h3>
                    <div className="flex flex-wrap gap-3">
                        <button className="btn btn-primary">
                            Edit Profile
                        </button>
                        <button className="btn btn-outline">
                            Change Password
                        </button>
                        <button
                            className="btn btn-outline text-error border-error hover:bg-error-light"
                            onClick={() => navigate("/logout")}
                        >
                            <svg
                                className="w-5 h-5 mr-1"
                                fill="none"
                                stroke="currentColor"
                                viewBox="0 0 24 24"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth={2}
                                    d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"
                                />
                            </svg>
                            Sign Out
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Profile;
