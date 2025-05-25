The solution contains ecommerce application

### Backend

-   It consist of multiple services. Each service is located in its own folder, e.g. `src/ProductService`
-   Each service consist of multiple layers. Each layer is a separate project in its own folder
    -   MyService.Api - ASP.NET Core project, entry point of a service
    -   MyService.Domain - contains domain entities of a service
    -   MyService.Application - application layer of the service
    -   MyService.Contracts - DTOs which other services can use to communicate with MyService (via REST API or message queue)
    -   MyService.Data - contains infrastructure logic to persist service's data. DbContext, repositories, Entity configurations, Migrations e.t.c.
-   Existing services:
    -   ProductService
    -   NotificationService
    -   OrderService
    -   ShippingService
    -   UserService

### Frontend

`src\Clients\onlineshop` - react application for an ecommerce

-   `src/clients` http clients for API calls
-   `src/components` contains folders with components. Each folder could contain multiple components. They are grouped by feature.
-   `src/config` contains configuration for authentication of oidc client
-   `src/contexts` contains context providers for the application.
-   `src/styles` contains styles for the application and different components.
-   `App.tsx` is the main entry point of the application. It contains the routing and the main layout of the application.
