#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Bet.Exentions.Minimal.Worker/Bet.Exentions.Minimal.Worker.csproj", "src/Bet.Exentions.Minimal.Worker/"]
RUN dotnet restore "src/Bet.Exentions.Minimal.Worker/Bet.Exentions.Minimal.Worker.csproj"
COPY . .
WORKDIR "/src/src/Bet.Exentions.Minimal.Worker"
RUN dotnet build "Bet.Exentions.Minimal.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bet.Exentions.Minimal.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bet.Exentions.Minimal.Worker.dll"]