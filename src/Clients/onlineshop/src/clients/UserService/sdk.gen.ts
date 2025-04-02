// This file is auto-generated by @hey-api/openapi-ts

import { type Options as ClientOptions, type TDataShape, type Client, urlSearchParamsBodySerializer } from '@hey-api/client-axios';
import type { GetUserData, GetUserResponse, PostAccountRegisterData, GetConnectAuthorizeData, PostConnectAuthorizeData, GetConnectLogoutData, PostConnectLogoutData, PostConnectTokenData } from './types.gen';
import { client as _heyApiClient } from './client.gen';

export type Options<TData extends TDataShape = TDataShape, ThrowOnError extends boolean = boolean> = ClientOptions<TData, ThrowOnError> & {
    /**
     * You can provide a client instance returned by `createClient()` instead of
     * individual options. This might be also useful if you want to implement a
     * custom client.
     */
    client?: Client;
    /**
     * You can pass arbitrary values through the `meta` object. This can be
     * used to access values that aren't defined as part of the SDK function.
     */
    meta?: Record<string, unknown>;
};

export const getUser = <ThrowOnError extends boolean = false>(options?: Options<GetUserData, ThrowOnError>) => {
    return (options?.client ?? _heyApiClient).get<GetUserResponse, unknown, ThrowOnError>({
        url: '/user',
        ...options
    });
};

export const postAccountRegister = <ThrowOnError extends boolean = false>(options: Options<PostAccountRegisterData, ThrowOnError>) => {
    return (options.client ?? _heyApiClient).post<unknown, unknown, ThrowOnError>({
        url: '/account/register',
        ...options,
        headers: {
            'Content-Type': 'application/json',
            ...options?.headers
        }
    });
};

export const getConnectAuthorize = <ThrowOnError extends boolean = false>(options?: Options<GetConnectAuthorizeData, ThrowOnError>) => {
    return (options?.client ?? _heyApiClient).get<unknown, unknown, ThrowOnError>({
        url: '/connect/authorize',
        ...options
    });
};

export const postConnectAuthorize = <ThrowOnError extends boolean = false>(options?: Options<PostConnectAuthorizeData, ThrowOnError>) => {
    return (options?.client ?? _heyApiClient).post<unknown, unknown, ThrowOnError>({
        url: '/connect/authorize',
        ...options
    });
};

export const getConnectLogout = <ThrowOnError extends boolean = false>(options?: Options<GetConnectLogoutData, ThrowOnError>) => {
    return (options?.client ?? _heyApiClient).get<unknown, unknown, ThrowOnError>({
        url: '/connect/logout',
        ...options
    });
};

export const postConnectLogout = <ThrowOnError extends boolean = false>(options: Options<PostConnectLogoutData, ThrowOnError>) => {
    return (options.client ?? _heyApiClient).post<unknown, unknown, ThrowOnError>({
        ...urlSearchParamsBodySerializer,
        url: '/connect/logout',
        ...options,
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
            ...options?.headers
        }
    });
};

export const postConnectToken = <ThrowOnError extends boolean = false>(options?: Options<PostConnectTokenData, ThrowOnError>) => {
    return (options?.client ?? _heyApiClient).post<unknown, unknown, ThrowOnError>({
        url: '/connect/token',
        ...options
    });
};