FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
ARG PUBLISH_DIRECTORY
COPY $PUBLISH_DIRECTORY /app/publish
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "Api.dll"]