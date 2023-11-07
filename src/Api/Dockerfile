#To be run from src folder

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Api", "r/ImageSharingPlatform.Backend/src/"]
COPY ["Application", "r/ImageSharingPlatform.Backend/src/"]
COPY ["CrossCutting", "r/ImageSharingPlatform.Backend/src/"]
COPY ["Domain/Domain.csproj", "r/ImageSharingPlatform.Backend/src/"]
COPY ["Persistence/Persistence.csproj", "r/ImageSharingPlatform.Backend/src/"]
COPY ["../../Michael.Net/src/", "r/Michael.Net/src/"]
RUN dotnet restore "r/ImageSharingPlatform.Backend/src/Api/Api.csproj"
WORKDIR "r/ImageSharingPlatform.Backend/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]