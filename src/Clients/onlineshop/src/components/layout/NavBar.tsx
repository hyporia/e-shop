import { NavLink } from "react-router-dom";
import { useAuth } from "../../contexts/authContext";
import { useState } from "react";

const NavBar = (): JSX.Element => {
    const { isAuthenticated } = useAuth();
    const [mobileMenuOpen, setMobileMenuOpen] = useState(false);

    const toggleMobileMenu = () => setMobileMenuOpen(!mobileMenuOpen);

    // Dynamic navigation link styling
    const getLinkClass = ({ isActive }: { isActive: boolean }) => {
        const baseClass =
            "px-3 py-2 rounded-md text-sm font-medium transition-colors";
        const activeClass = isActive
            ? "text-primary font-semibold"
            : "text-text-light hover:text-primary";
        return `${baseClass} ${activeClass}`;
    };

    return (
        <header className="bg-surface shadow-sm">
            <div className="container mx-auto px-4">
                <div className="flex items-center justify-between h-16">
                    {/* Logo and brand */}
                    <div className="flex items-center">
                        <NavLink
                            to="/"
                            className="text-primary font-bold text-xl tracking-tight"
                        >
                            MinShop
                        </NavLink>
                    </div>

                    {/* Desktop navigation */}
                    <nav className="hidden md:block">
                        <ul className="flex space-x-4">
                            <li>
                                <NavLink to="/" className={getLinkClass} end>
                                    Home
                                </NavLink>
                            </li>
                            <li>
                                <NavLink
                                    to="/products"
                                    className={getLinkClass}
                                >
                                    Products
                                </NavLink>
                            </li>

                            {isAuthenticated ? (
                                <>
                                    <li>
                                        <NavLink
                                            to="/profile"
                                            className={getLinkClass}
                                        >
                                            Profile
                                        </NavLink>
                                    </li>
                                    <li>
                                        <NavLink
                                            to="/logout"
                                            className={getLinkClass}
                                        >
                                            Logout
                                        </NavLink>
                                    </li>
                                </>
                            ) : (
                                <>
                                    <li>
                                        <NavLink
                                            to="/login"
                                            className={getLinkClass}
                                        >
                                            Login
                                        </NavLink>
                                    </li>
                                    <li>
                                        <NavLink
                                            to="/register"
                                            className={getLinkClass}
                                        >
                                            Register
                                        </NavLink>
                                    </li>
                                </>
                            )}
                        </ul>
                    </nav>

                    {/* Mobile menu button */}
                    <div className="md:hidden">
                        <button
                            type="button"
                            className="inline-flex items-center justify-center p-2 rounded-md text-text-light hover:text-primary"
                            aria-controls="mobile-menu"
                            aria-expanded={mobileMenuOpen}
                            onClick={toggleMobileMenu}
                        >
                            <span className="sr-only">Open main menu</span>
                            {/* Icon when menu is closed */}
                            <svg
                                className={`${
                                    mobileMenuOpen ? "hidden" : "block"
                                } h-6 w-6`}
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 24 24"
                                stroke="currentColor"
                                aria-hidden="true"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth="2"
                                    d="M4 6h16M4 12h16M4 18h16"
                                />
                            </svg>
                            {/* Icon when menu is open */}
                            <svg
                                className={`${
                                    mobileMenuOpen ? "block" : "hidden"
                                } h-6 w-6`}
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 24 24"
                                stroke="currentColor"
                                aria-hidden="true"
                            >
                                <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth="2"
                                    d="M6 18L18 6M6 6l12 12"
                                />
                            </svg>
                        </button>
                    </div>
                </div>
            </div>

            {/* Mobile menu, show/hide based on menu state */}
            <div
                className={`${mobileMenuOpen ? "block" : "hidden"} md:hidden`}
                id="mobile-menu"
            >
                <div className="px-2 pt-2 pb-3 space-y-1">
                    <NavLink
                        to="/"
                        className={({ isActive }) =>
                            `block px-3 py-2 rounded-md text-base font-medium ${
                                isActive
                                    ? "bg-primary-light text-primary"
                                    : "text-text-light hover:bg-gray-100"
                            }`
                        }
                        end
                    >
                        Home
                    </NavLink>
                    <NavLink
                        to="/products"
                        className={({ isActive }) =>
                            `block px-3 py-2 rounded-md text-base font-medium ${
                                isActive
                                    ? "bg-primary-light text-primary"
                                    : "text-text-light hover:bg-gray-100"
                            }`
                        }
                    >
                        Products
                    </NavLink>

                    {isAuthenticated ? (
                        <>
                            <NavLink
                                to="/profile"
                                className={({ isActive }) =>
                                    `block px-3 py-2 rounded-md text-base font-medium ${
                                        isActive
                                            ? "bg-primary-light text-primary"
                                            : "text-text-light hover:bg-gray-100"
                                    }`
                                }
                            >
                                Profile
                            </NavLink>
                            <NavLink
                                to="/logout"
                                className={({ isActive }) =>
                                    `block px-3 py-2 rounded-md text-base font-medium ${
                                        isActive
                                            ? "bg-primary-light text-primary"
                                            : "text-text-light hover:bg-gray-100"
                                    }`
                                }
                            >
                                Logout
                            </NavLink>
                        </>
                    ) : (
                        <>
                            <NavLink
                                to="/login"
                                className={({ isActive }) =>
                                    `block px-3 py-2 rounded-md text-base font-medium ${
                                        isActive
                                            ? "bg-primary-light text-primary"
                                            : "text-text-light hover:bg-gray-100"
                                    }`
                                }
                            >
                                Login
                            </NavLink>
                            <NavLink
                                to="/register"
                                className={({ isActive }) =>
                                    `block px-3 py-2 rounded-md text-base font-medium ${
                                        isActive
                                            ? "bg-primary-light text-primary"
                                            : "text-text-light hover:bg-gray-100"
                                    }`
                                }
                            >
                                Register
                            </NavLink>
                        </>
                    )}
                </div>
            </div>
        </header>
    );
};

export default NavBar;
