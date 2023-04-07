#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM centos:7 AS base

# Add Microsoft package repository and install ASP.NET Core
RUN rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm \
    && yum install -y aspnetcore-runtime-6.0

# Ensure we listen on any IP Address 
ENV DOTNET_URLS=http://+:8081
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Beymen.Service-B/Beymen.Service-B.csproj", "Beymen.Service-B/"]
COPY ["Beymen.ConfigurationLibrary/Beymen.ConfigurationLibrary.csproj", "Beymen.ConfigurationLibrary/"]
RUN dotnet restore "Beymen.Service-B/Beymen.Service-B.csproj"
COPY . .
WORKDIR "/src/Beymen.Service-B"
RUN dotnet build "Beymen.Service-B.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Beymen.Service-B.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Beymen.Service-B.dll"]