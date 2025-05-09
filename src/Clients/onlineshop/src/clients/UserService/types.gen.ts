// This file is auto-generated by @hey-api/openapi-ts

export type GetUsersResponse = {
    users?: Array<GetUsersResponseUser>;
};

export type GetUsersResponseUser = {
    id: string;
    name: string;
};

export type RegisterUser = {
    email: string;
    username: string;
    password: string;
};

export type GetUserData = {
    body?: never;
    path?: never;
    query?: never;
    url: '/user';
};

export type GetUserResponses = {
    /**
     * OK
     */
    200: GetUsersResponse;
};

export type GetUserResponse = GetUserResponses[keyof GetUserResponses];

export type PostAccountRegisterData = {
    body: RegisterUser;
    path?: never;
    query?: never;
    url: '/account/register';
};

export type PostAccountRegisterResponses = {
    /**
     * OK
     */
    200: unknown;
};

export type GetConnectAuthorizeData = {
    body?: never;
    path?: never;
    query?: never;
    url: '/connect/authorize';
};

export type GetConnectAuthorizeResponses = {
    /**
     * OK
     */
    200: unknown;
};

export type PostConnectAuthorizeData = {
    body?: never;
    path?: never;
    query?: never;
    url: '/connect/authorize';
};

export type PostConnectAuthorizeResponses = {
    /**
     * OK
     */
    200: unknown;
};

export type GetConnectLogoutData = {
    body?: never;
    path?: never;
    query?: {
        post_logout_redirect_uri?: string;
    };
    url: '/connect/logout';
};

export type GetConnectLogoutResponses = {
    /**
     * OK
     */
    200: unknown;
};

export type PostConnectLogoutData = {
    body: {
        post_logout_redirect_uri?: string;
    };
    path?: never;
    query?: never;
    url: '/connect/logout';
};

export type PostConnectLogoutResponses = {
    /**
     * OK
     */
    200: unknown;
};

export type PostConnectTokenData = {
    body?: never;
    path?: never;
    query?: never;
    url: '/connect/token';
};

export type PostConnectTokenResponses = {
    /**
     * OK
     */
    200: unknown;
};

export type ClientOptions = {
    baseURL: 'https://localhost:7101' | (string & {});
};