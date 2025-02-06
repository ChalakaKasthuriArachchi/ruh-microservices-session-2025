# RUH Microservices Session 2025

This repository contains two microservices: `invoicing-service` and `payment-service`. These services communicate with each other using RabbitMQ for message passing.

## Services

### Invoicing Service

The `invoicing-service` is responsible for managing invoices. It provides endpoints to create and retrieve invoices.

- **Endpoints:**
  - `GET /Invoice`: Retrieve all invoices.
  - `GET /Invoice/{id}`: Retrieve a specific invoice by ID.
  - `POST /Invoice`: Create a new invoice.

- **Technologies:**
  - .NET 8
  - Entity Framework Core (In-Memory Database)
  - RabbitMQ

- **Key Files:**
  - [InvoiceController.cs](invoicing-service/Controllers/InvoiceController.cs)
  - [PaymentRequestService.cs](invoicing-service/Services/PaymentRequestService.cs)
  - [PaymentAckConsumer.cs](invoicing-service/Services/PaymentAckConsumer.cs)
  - [AppDbContext.cs](invoicing-service/Models/AppDbContext.cs)

### Payment Service

The `payment-service` is responsible for processing payments. It listens for payment requests and sends payment acknowledgments.

- **Endpoints:**
  - `GET /weatherforecast`: Sample endpoint for testing.

- **Technologies:**
  - .NET 8
  - RabbitMQ

- **Key Files:**
  - [PaymentRequestConsumer.cs](payment-service/Services/PaymentRequestConsumer.cs)
  - [PaymentAckService.cs](payment-service/Services/PaymentAckService.cs)

## Running the Services

### Prerequisites

- Docker (https://www.docker.com/get-started/)
- Docker Compose

### Steps

1. Clone the repository:
   ```sh
   git clone https://github.com/ChalakaKasthuriArachchi/ruh-microservices-session-2025
   cd RUH-MICROSERVICES-SESSION-2025

2. Build & run the services using Docker Compose
    ```docker-compose up --build

3. Access the Invoice service using Rest API
    ```http://localhost:8080

4. Stop all services
    ```docker-compose down`

