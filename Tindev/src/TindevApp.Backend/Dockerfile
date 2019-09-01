FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/TindevApp.Backend/TindevApp.Backend.csproj", "src/TindevApp.Backend/"]
RUN dotnet restore "src/TindevApp.Backend/TindevApp.Backend.csproj"
COPY . .
WORKDIR "/src/src/TindevApp.Backend"
RUN dotnet build "TindevApp.Backend.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TindevApp.Backend.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TindevApp.Backend.dll"]