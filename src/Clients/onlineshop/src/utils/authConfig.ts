import { UserManagerSettings } from "oidc-client-ts";

// custom settings that work with our own OAuth server
const authSettings: UserManagerSettings = {
  authority: import.meta.env.VITE_USERSERVICE_API_URL!,
  client_id: "onlineshop",
  redirect_uri: `http://localhost:${import.meta.env.VITE_PORT}/oauth/callback`,
  response_type: "token",
  // this is for getting user.profile data, when open id connect is implemented
  //scope: 'api1 openid profile'
  // this is just for OAuth2 flow
  scope: "user_api",
};

export const authConfig = {
  settings: authSettings,
  flow: "password",
};
