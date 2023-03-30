#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /src
COPY ["src/Identity/Identity.Api/Identity.Api.csproj", "src/Identity/Identity.Api/"]
COPY ["src/Identity/Identity.Services/Identity.Services.csproj", "src/Identity/Identity.Services/"]
COPY ["src/Identity/Identity.DataLayer/Identity.DataLayer.csproj", "src/Identity/Identity.DataLayer/"]
COPY ["src/Identity/Identity.Entities/Identity.Entities.csproj", "src/Identity/Identity.Entities/"]
COPY ["src/Identity/Identity.Common/Identity.Common.csproj", "src/Identity/Identity.Common/"]
RUN dotnet restore "src/Identity/Identity.Api/Identity.Api.csproj"
COPY . .
WORKDIR "/src/src/Identity/Identity.Api"
RUN dotnet build "Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]