#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM centos:7 AS base

# Add Microsoft package repository and install ASP.NET Core
RUN rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm \
    && yum install -y aspnetcore-runtime-6.0

# Ensure we listen on any IP Address 
ENV DOTNET_URLS=http://+:8080
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Beymen.Service-A/Beymen.Service-A.csproj", "Beymen.Service-A/"]
COPY ["Beymen.ConfigurationLibrary/Beymen.ConfigurationLibrary.csproj", "Beymen.ConfigurationLibrary/"]
RUN dotnet restore "Beymen.Service-A/Beymen.Service-A.csproj"
COPY . .
WORKDIR "/src/Beymen.Service-A"
RUN dotnet build "Beymen.Service-A.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Beymen.Service-A.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Beymen.Service-A.dll"]