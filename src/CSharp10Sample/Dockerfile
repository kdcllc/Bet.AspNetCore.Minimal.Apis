#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/CSharp10Sample/CSharp10Sample.csproj", "src/CSharp10Sample/"]
RUN dotnet restore "src/CSharp10Sample/CSharp10Sample.csproj"
COPY . .
WORKDIR "/src/src/CSharp10Sample"
RUN dotnet build "CSharp10Sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CSharp10Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CSharp10Sample.dll"]
