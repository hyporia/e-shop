// App.tsx
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Login from "./components/User/Login";
import Register from "./components/User/Register";
import NavBar from "./components/NavBar";
import Home from "./components/Home/Home";
import { useEffect, useState } from "react";
import { User } from "oidc-client-ts";
import { getUser } from "./services/AuthService";

const App = (): JSX.Element => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    const checkUser = async () => {
      const user = await getUser();
      setIsAuthenticated(!!user);
      setUser(user);
    };

    checkUser();
  }, []);

  return (
    <>
      <NavBar isAuthenticated={isAuthenticated} user={user} />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
      </Routes>
    </>
  );
};

export default App;
