Paye (formerly BackEnd) Clean Architecture + CQRS template

Structure
- Domain: Entities and interfaces (namespace `Paye.Domain`)
- Application: CQRS requests and DTOs (namespace `Paye.Application`)
- Infrastructure: Implementations and DI (namespace `Paye.Infrastructure`)
- Api: ASP.NET Core Web API entry and controllers (namespace `Paye.Api`)

How to use
1. Clone the BackEnd folder to start a new feature project and rename as needed to `Paye`.
2. Add new entities to Domain
3. Add requests and handlers to Application
4. Add repo implementations to Infrastructure and register via `AddInfrastructure`
5. Wire MediatR and DI in Api/Program.cs
6. Add FluentValidation validators in `Application` and they will be auto-registered. Validation runs via MediatR pipeline.

Notes
- Uses MediatR for CQRS
- Uses FluentValidation for request validation
- Sample feature: Weather with in-memory repo
- Replace InMemoryWeatherRepository with EF implementation for production