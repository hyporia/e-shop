// src/components/User/Registration.tsx

import { useState, useEffect } from "react";
import validator from "validator";
import "./Registration.css";

function Registration() {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [emailError, setEmailError] = useState("");

  useEffect(() => {
    const timer = setTimeout(() => {
      if (email && !validator.isEmail(email)) {
        setEmailError("Invalid email address");
      } else {
        setEmailError("");
      }
    }, 500); // 500ms debounce time

    return () => clearTimeout(timer);
  }, [email]);

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();
    if (!validator.isEmail(email)) {
      return;
    }
    // Handle form submission logic here
    console.log("User registered:", { username, email, password });
  };

  return (
    <div className="registration-container">
      <h2>Register</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="username">Username</label>
          <input
            type="text"
            id="username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="email">Email</label>
          <input
            type="email"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            className={emailError ? "error-input" : ""}
          />
          {emailError && <div className="error-message">{emailError}</div>}
        </div>
        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input
            type="password"
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit">Register</button>
      </form>
    </div>
  );
}

export default Registration;
