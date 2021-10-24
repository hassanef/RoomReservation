#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/Reservation/Reservation.Api/Reservation.Api.csproj", "src/Reservation/Reservation.Api/"]
COPY ["src/Reservation/Reservation.Application/Reservation.Application.csproj", "src/Reservation/Reservation.Application/"]
COPY ["src/Reservation/Reservation.Infrastructure/Reservation.Infrastructure.csproj", "src/Reservation/Reservation.Infrastructure/"]
COPY ["src/Reservation/Reservation.Domain/Reservation.Domain.csproj", "src/Reservation/Reservation.Domain/"]
RUN dotnet restore "src/Reservation/Reservation.Api/Reservation.Api.csproj"
COPY . .
WORKDIR "/src/src/Reservation/Reservation.Api"
RUN dotnet build "Reservation.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Reservation.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Reservation.Api.dll"]