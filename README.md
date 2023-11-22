# Public.Api

[![Build Status](https://dev.azure.com/MichaelTrullasGarcia/Public/_apis/build/status%2FPublic.Api%20-%20Build?branchName=main)](https://dev.azure.com/MichaelTrullasGarcia/Public/_build/latest?definitionId=3&branchName=main)

Sample API for testing/fun/learning purposes using good architecture and tests

API:
- ASP.NET Core
- N-Layer Architecture
- Anemic Domain Model
- Services
- Entity Framework Core
- SQL Server

Tests:
- Unit, integration and functional tests
- xUnit
- Moq
- FluentAssertions
- Coverlet + ReportGenerator

Build/Deploy:
- Uses docker for build and unit/integration testing
- Uses another pipeline for deployment (see [Public.Deployment](https://github.com/michaeltg17/Public.Deployment) for details)
