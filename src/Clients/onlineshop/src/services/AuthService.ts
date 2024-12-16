import {
    SigninResourceOwnerCredentialsArgs,
    UserManager,
} from "oidc-client-ts";

import { authConfig } from "../utils/authConfig";

export class User {
    constructor(public id: string, public name: string) {}
}

const userManager = new UserManager(authConfig.settings);

export async function getUser(): Promise<User | null> {
    var userInfo = await userManager.getUser();
    if (!userInfo) {
        return null;
    }

    return new User(userInfo.profile.sub, userInfo.profile.name!);
}

export async function isAuthenticated() {
    let token = await getAccessToken();
    return !!token;
}

export async function login(
    signInData: SigninResourceOwnerCredentialsArgs
): Promise<User> {
    const oidsUser = await userManager.signinResourceOwnerCredentials(
        signInData
    );

    return new User(oidsUser.profile.sub, oidsUser.profile.name!);
}

// renews token using refresh token
export async function renewToken() {
    await userManager.signinSilent();
}

export async function getAccessToken() {
    const oidsUser = await userManager.getUser();
    return oidsUser?.access_token;
}

export async function logout() {
    await userManager.clearStaleState();
    await userManager.signoutRedirect();
}

// This function is used to access token claims
// `.profile` is available in Open Id Connect implementations
// in simple OAuth2 it is empty, because UserInfo endpoint does not exist
// export async function getRole() {
//     const user = await getUser();
//     return user?.profile?.role;
// }

// This function is used to change account similar way it is done in Google
// export async function selectOrganization() {
//     const args = {
//         prompt: "select_account"
//     }
//     await userManager.signinRedirect(args);
// }
