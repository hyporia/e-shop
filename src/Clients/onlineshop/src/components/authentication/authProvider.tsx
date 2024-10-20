import { useState } from "react";
import { AuthContext } from "../../utils/authContext";
import { User } from "oidc-client-ts";

import { ReactNode } from "react";

export const AuthProvider = ({
  children,
}: {
  children: ReactNode;
}): JSX.Element => {
  const [user, setUser] = useState<User | null>(null);
  const onLogin = (user: User) => {
    setUser(user);
  };
  const onLogout = () => {
    setUser(null);
  };
  const isAuthenticated = !!user;
  return (
    <AuthContext.Provider value={{ isAuthenticated, onLogin, onLogout }}>
      {children}
    </AuthContext.Provider>
  );
};
