import {
    SigninResourceOwnerCredentialsArgs,
    UserManager,
} from "oidc-client-ts";
import { authConfig } from "../../config/authConfig";

export class User {
    constructor(public id: string, public name: string) {}
}

const userManager = new UserManager(authConfig.settings);

export async function getUser(): Promise<User | null> {
    const userInfo = await userManager.getUser();
    if (!userInfo) return null;
    return new User(userInfo.profile.sub, userInfo.profile.name!);
}

export async function isAuthenticated(): Promise<boolean> {
    const oidcUser = await userManager.getUser();
    return !!oidcUser?.access_token;
}

export async function login(
    signInData: SigninResourceOwnerCredentialsArgs
): Promise<User> {
    const oidcUser = await userManager.signinResourceOwnerCredentials(
        signInData
    );
    return new User(oidcUser.profile.sub, oidcUser.profile.name!);
}

export async function renewToken(): Promise<void> {
    await userManager.signinSilent();
}

export async function logout(): Promise<void> {
    await userManager.clearStaleState();
    await userManager.signoutRedirect();
}

export default {
    getUser,
    isAuthenticated,
    login,
    renewToken,
    logout,
};
