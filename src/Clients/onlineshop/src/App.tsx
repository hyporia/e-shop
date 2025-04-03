import { lazy, Suspense, useCallback } from "react";
import { Route, Routes } from "react-router-dom";
import { AuthProvider } from "./components/auth/authProvider";
import NavBar from "./components/layout/NavBar";
import ProtectedRoutes from "./components/shared/ProtectedRoute";
import LoadingSpinner from "./components/shared/LoadingSpinner";
import ErrorBoundary from "./components/shared/ErrorBoundary";

// Memoized lazy-loaded components
const Home = lazy(() => import("./components/home/Home"));
const Login = lazy(() => import("./components/user/Login"));
const Register = lazy(() => import("./components/user/Register"));
const Profile = lazy(() => import("./components/user/Profile"));
const Logout = lazy(() => import("./components/user/Logout"));
const Products = lazy(() => import("./components/products/Products"));

const App = (): JSX.Element => {
    const renderRoutes = useCallback(
        () => (
            <Routes>
                <Route path="/" element={<Home />} />
                <Route element={<ProtectedRoutes />}>
                    <Route path="/profile" element={<Profile />} />
                </Route>
                <Route path="/home" element={<Home />} />
                <Route path="/products" element={<Products />} />
                <Route path="/login" element={<Login />} />
                <Route path="/logout" element={<Logout />} />
                <Route path="/register" element={<Register />} />
                <Route path="/products" element={<Products />} />
            </Routes>
        ),
        []
    );

    return (
        <AuthProvider>
            <NavBar />
            <ErrorBoundary>
                <Suspense fallback={<LoadingSpinner />}>
                    {renderRoutes()}
                </Suspense>
            </ErrorBoundary>
        </AuthProvider>
    );
};

export default App;
