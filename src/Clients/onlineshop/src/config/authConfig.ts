import { UserManagerSettings } from "oidc-client-ts";

// custom settings that work with our own OAuth server
const authSettings: UserManagerSettings = {
    authority: import.meta.env.VITE_USERSERVICE_API_URL!,
    client_id: "onlineshop",
    redirect_uri: `http://localhost:${
        import.meta.env.VITE_PORT
    }/oauth/callback`,
    response_type: "token",
    loadUserInfo: false,
    post_logout_redirect_uri: `http://localhost:${import.meta.env.VITE_PORT}`,
    automaticSilentRenew: true,
    metadata: {
        userinfo_endpoint: `${
            import.meta.env.VITE_USERSERVICE_API_URL
        }/connect/userinfo`,
        token_endpoint: `${
            import.meta.env.VITE_USERSERVICE_API_URL
        }/connect/token`,
        end_session_endpoint: `${
            import.meta.env.VITE_USERSERVICE_API_URL
        }/connect/logout`,
    },
};

export const authConfig = {
    settings: authSettings,
    flow: "password",
};
