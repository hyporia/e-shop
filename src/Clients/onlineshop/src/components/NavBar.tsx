import { User } from "oidc-client-ts";
import { NavLink } from "react-router-dom";

interface NavBarProps {
  isAuthenticated: boolean;
  user: User | null;
}

const NavBar = ({ isAuthenticated, user }: NavBarProps): JSX.Element => {
  return (
    <nav>
      <ul>
        <li>
          <NavLink to="/">Home</NavLink>
        </li>
        {isAuthenticated && (
          <>
            <li>{user?.profile?.email}</li>
            <li>
              <NavLink to="/Log out">Log out</NavLink>
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
