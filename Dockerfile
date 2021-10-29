FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Smarter.iKettle.Api/Smarter.iKettle.Api.csproj", "src/Smarter.iKettle.Api/"]
COPY ["src/Smarter.iKettle.Application/Smarter.iKettle.Application.csproj", "src/Smarter.iKettle.Application/"]
COPY ["src/Smarter.iKettle.Infrastructure/Smarter.iKettle.Infrastructure.csproj", "src/Smarter.iKettle.Infrastructure/"]
RUN dotnet restore "src/Smarter.iKettle.Api/Smarter.iKettle.Api.csproj"
COPY . .
WORKDIR "/src/src/Smarter.iKettle.Api"
RUN dotnet build "Smarter.iKettle.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Smarter.iKettle.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Smarter.iKettle.Api.dll"]