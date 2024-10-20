import {
  SigninResourceOwnerCredentialsArgs,
  User,
  UserManager,
} from "oidc-client-ts";

import { authConfig } from "../utils/authConfig";

const userManager = new UserManager(authConfig.settings);
export async function getUser(): Promise<User | null> {
  return await userManager.getUser();
}

export async function isAuthenticated() {
  let token = await getAccessToken();

  return !!token;
}

export async function login(
  signInData: SigninResourceOwnerCredentialsArgs
) : Promise<User> {
  return await userManager.signinResourceOwnerCredentials(signInData);
}

// renews token using refresh token
export async function renewToken() {
  const user = await userManager.signinSilent();

  return user;
}

export async function getAccessToken() {
  const user = await getUser();
  return user?.access_token;
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
