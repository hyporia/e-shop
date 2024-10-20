// App.tsx
import { Route, Routes } from "react-router-dom";
import Login from "./components/user/Login";
import Register from "./components/user/Register";
import NavBar from "./components/NavBar";
import Home from "./components/home/Home";
import ProtectedRoutes from "./components/shared/ProtectedRoute";
import Profile from "./components/user/Profile";
import { AuthProvider } from "./components/authentication/authProvider";

const App = (): JSX.Element => {
  return (
    <AuthProvider>
      <NavBar />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route element={<ProtectedRoutes />}>
          <Route path="/profile" element={<Profile />} />
        </Route>
        <Route path="/home" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
      </Routes>
    </AuthProvider>
  );
};

export default App;
