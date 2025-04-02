import { NavLink } from "react-router-dom";
import { useAuth } from "../../contexts/authContext";
import "../../styles/styles.css";

const NavBar = (): JSX.Element => {
    const { isAuthenticated } = useAuth();
    return (
        <nav className="fixed top-0 left-0 right-0 bg-gray-800 p-md shadow-md z-50">
            <ul className="flex list-none m-0 p-0 gap-lg">
                <li>
                    <NavLink
                        to="/"
                        className={({ isActive }) =>
                            `text-base ${
                                isActive
                                    ? "text-white font-bold"
                                    : "text-gray-300 font-normal"
                            }`
                        }
                    >
                        Home
                    </NavLink>
                </li>
                <li>
                    <NavLink
                        to="/products"
                        className={({ isActive }) =>
                            `text-base ${
                                isActive
                                    ? "text-white font-bold"
                                    : "text-gray-300 font-normal"
                            }`
                        }
                    >
                        Products
                    </NavLink>
                </li>
                {isAuthenticated && (
                    <>
                        <li>
                            <NavLink
                                to="/profile"
                                className={({ isActive }) =>
                                    `text-base ${
                                        isActive
                                            ? "text-white font-bold"
                                            : "text-gray-300 font-normal"
                                    }`
                                }
                            >
                                Profile
                            </NavLink>
                        </li>
                        <li>
                            <NavLink
                                to="/logout"
                                className={({ isActive }) =>
                                    `text-base ${
                                        isActive
                                            ? "text-white font-bold"
                                            : "text-gray-300 font-normal"
                                    }`
                                }
                            >
                                Log out
                            </NavLink>
                        </li>
                    </>
                )}
                {!isAuthenticated && (
                    <>
                        <li>
                            <NavLink
                                to="/login"
                                className={({ isActive }) =>
                                    `text-base ${
                                        isActive
                                            ? "text-white font-bold"
                                            : "text-gray-300 font-normal"
                                    }`
                                }
                            >
                                Login
                            </NavLink>
                        </li>
                        <li>
                            <NavLink
                                to="/register"
                                className={({ isActive }) =>
                                    `text-base ${
                                        isActive
                                            ? "text-white font-bold"
                                            : "text-gray-300 font-normal"
                                    }`
                                }
                            >
                                Register
                            </NavLink>
                        </li>
                    </>
                )}
            </ul>
        </nav>
    );
};

export default NavBar;
