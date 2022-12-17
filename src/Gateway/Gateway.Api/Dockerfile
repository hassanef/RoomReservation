#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /src
COPY ["src/Gateway/Gateway.Api/Gateway.Api.csproj", "src/Gateway/Gateway.Api/"]
RUN dotnet restore "src/Gateway/Gateway.Api/Gateway.Api.csproj"
COPY . .
WORKDIR "/src/src/Gateway/Gateway.Api"
RUN dotnet build "Gateway.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateway.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.Api.dll"]