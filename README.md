# Public.Api

[![Build Status](https://dev.azure.com/MichaelTrullasGarcia/Public/_apis/build/status%2FPublic.Api%20-%20Build?branchName=main)](https://dev.azure.com/MichaelTrullasGarcia/Public/_build/latest?definitionId=3&branchName=main)

.NET 8, ASP.NET Core Api + Tests template of my recommended architecture for a successful, dev efficient and scalable solution. 

See the new template here [dotnet-api-template](https://github.com/michaeltg17/dotnet-api-template)

API:
- ASP.NET Core
- OpenAPI
- ProblemDetails
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
- Build + Unit/Integration tests + Coverage + Artifacts
- Uses another pipeline for deployment + functional tests (see [Public.Deployment](https://github.com/michaeltg17/Public.Deployment) for details)
