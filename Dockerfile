FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

COPY . /app/
RUN dotnet publish src/Smarter.iKettle.Api/Smarter.iKettle.Api.csproj  -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app

COPY --from=build-env /app/src/Smarter.iKettle.Api/out .

ENTRYPOINT ["dotnet", "Smarter.iKettle.Api.dll"]