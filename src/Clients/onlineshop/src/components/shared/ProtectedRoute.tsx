import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../../utils/authContext";

const ProtectedRoutes = (): JSX.Element => {
	const { isAuthenticated } = useAuth();
	return isAuthenticated ? <Outlet /> : <Navigate to="/login" />;
};

// const ProtectedRoute = ({
//   element,
//   ...rest
// }: RouteProps): Jsx => {
//   const { isAuthenticated } = useAuth();
//   return (
//     <Route
//       {...rest}
//       element={isAuthenticated ? element : <Navigate to="/login" />}
//     />
//   );
// };

export default ProtectedRoutes;
