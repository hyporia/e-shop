# E-Commerce Order Processing System

## Overview

This project is an e-commerce platform where users can buy and sell products. The system is built using microservices architecture, with each service responsible for specific tasks. Message queues are used to decouple these services, ensuring scalability and reliability.

**Note:** This project was created for educational purposes.

## Microservices

1. **User Service**
   - Handles user registration, authentication, and profile management.
2. **Product Service**
   - Manages product listings, inventory, and details.
3. **Order Service**
   - Processes orders, calculates totals, and communicates with payment gateways.
4. **Notification Service**
   - Sends order confirmation emails and notifications.
5. **Shipping Service**
   - Coordinates shipping and tracking information.

## Technologies Used

- **.NET Core** for building microservices
- **RabbitMQ** for message queuing
- **MassTransit** as an abstraction over RabbitMQ
- **Entity Framework Core** for database interactions
- **Docker** for containerization
- **Aspire** for orchestration
- Other tools which will be listed later
