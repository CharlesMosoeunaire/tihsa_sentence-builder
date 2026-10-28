FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY SentenceBuilder.Api/SentenceBuilder.Api.csproj SentenceBuilder.Api/
RUN dotnet restore SentenceBuilder.Api/SentenceBuilder.Api.csproj
COPY SentenceBuilder.Api/ SentenceBuilder.Api/
RUN dotnet publish SentenceBuilder.Api/SentenceBuilder.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "SentenceBuilder.Api.dll"]
