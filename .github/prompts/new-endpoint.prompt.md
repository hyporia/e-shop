Your goal is to generate a new endpoint using FastEndpoint nuget package

Ask for the endpoint name if not provided.

Requirements for the new endpoint:

-   The endpoint could be query or command

    -   Query must return data
    -   Query must be a GET request
    -   Query must use IQueries\<TEntity\> to query data from the database
        -   if IQueries\<TEntity\> is lacking functionality, suggest to create a new one
    -   Command could return data
    -   Command could be a POST, PUT, DELETE request
    -   Ensure proper validation is implemented for all requests

-   DTOs should be created in \*.Contracts project in appropriate folder
-   Endpoint itself should be created in \*.Application project in appropriate folder
-   If endpoint creates a new entity or queries entities from the database it should contain the mapping logic to map the DTO to the entity and vice versa.
