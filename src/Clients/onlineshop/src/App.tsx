import { Routes, Route } from "react-router-dom";
import { AuthProvider } from "./components/auth/authProvider";
import Layout from "./components/layout/Layout";
import Home from "./components/home/Home";
import Products from "./components/products/Products";
import ProductDetails from "./components/products/ProductDetails";
import Login from "./components/user/Login";
import Register from "./components/user/Register";
import Logout from "./components/user/Logout";
import Profile from "./components/user/Profile";
import NotFound from "./components/shared/NotFound";
import ErrorBoundary from "./components/shared/ErrorBoundary";
import "./styles/styles.css";

function App() {
    return (
        <ErrorBoundary>
            <AuthProvider>
                <Layout>
                    <Routes>
                        <Route path="/" element={<Home />} />
                        <Route path="/products" element={<Products />} />
                        <Route
                            path="/products/:id"
                            element={<ProductDetails />}
                        />
                        <Route path="/login" element={<Login />} />
                        <Route path="/register" element={<Register />} />
                        <Route path="/logout" element={<Logout />} />
                        <Route path="/profile" element={<Profile />} />
                        <Route path="*" element={<NotFound />} />
                    </Routes>
                </Layout>
            </AuthProvider>
        </ErrorBoundary>
    );
}

export default App;
