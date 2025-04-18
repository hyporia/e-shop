// This file is auto-generated by @hey-api/openapi-ts

import type { Options as ClientOptions, TDataShape, Client } from '@hey-api/client-axios';
import type { ProductServiceApplicationEndpointsProductEndpointsGetProductsEndpointData, ProductServiceApplicationEndpointsProductEndpointsGetProductsEndpointResponse } from './types.gen';
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

export const productServiceApplicationEndpointsProductEndpointsGetProductsEndpoint = <ThrowOnError extends boolean = false>(options?: Options<ProductServiceApplicationEndpointsProductEndpointsGetProductsEndpointData, ThrowOnError>) => {
    return (options?.client ?? _heyApiClient).get<ProductServiceApplicationEndpointsProductEndpointsGetProductsEndpointResponse, unknown, ThrowOnError>({
        url: '/api/products',
        ...options
    });
};