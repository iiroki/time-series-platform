# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /app
ARG TARGET='TimeSeriesPlatform'
COPY . ./
RUN dotnet restore $TARGET
RUN dotnet publish -c Release -o build $TARGET

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=builder /app/build .
EXPOSE 8080
ENTRYPOINT ["dotnet", "TimeSeriesPlatform.dll"]
