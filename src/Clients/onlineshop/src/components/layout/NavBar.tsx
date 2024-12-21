import { NavLink } from "react-router-dom";
import { useAuth } from "../utils/authContext";

const NavBar = (): JSX.Element => {
    const { isAuthenticated } = useAuth();
    return (
        <nav>
            <ul>
                <li>
                    <NavLink to="/">Home</NavLink>
                </li>
                {isAuthenticated && (
                    <>
                        <li>
                            <NavLink to="/profile">Profile</NavLink>
                        </li>
                        <li>
                            <NavLink to="/logout">Log out</NavLink>
                        </li>
                    </>
                )}
                {!isAuthenticated && (
                    <>
                        <li>
                            <NavLink to="/login">Login</NavLink>
                        </li>
                        <li>
                            <NavLink to="/register">Register</NavLink>
                        </li>
                    </>
                )}
            </ul>
        </nav>
    );
};

export default NavBar;
