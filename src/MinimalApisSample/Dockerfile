FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/MinimalApisSample/MinimalApisSample.csproj", "src/MinimalApisSample/"]

RUN dotnet restore "src/MinimalApisSample/MinimalApisSample.csproj"
COPY . .
WORKDIR "/src/src/MinimalApisSample"
RUN dotnet build "MinimalApisSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinimalApisSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinimalApisSample.dll"]
