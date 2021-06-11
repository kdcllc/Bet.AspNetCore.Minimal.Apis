FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Bet.AspNetCore.Minimal.Apis/Bet.AspNetCore.Minimal.Apis.csproj", "src/Bet.AspNetCore.Minimal.Apis/"]

RUN dotnet restore "src/Bet.AspNetCore.Minimal.Apis/Bet.AspNetCore.Minimal.Apis.csproj"
COPY . .
WORKDIR "/src/src/Bet.AspNetCore.Minimal.Apis"
RUN dotnet build "Bet.AspNetCore.Minimal.Apis.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bet.AspNetCore.Minimal.Apis.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bet.AspNetCore.Minimal.Apis.dll"]