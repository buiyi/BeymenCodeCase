#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Beymen.Configuration.BackOffice/Beymen.Configuration.BackOffice.csproj", "Beymen.Configuration.BackOffice/"]
RUN dotnet restore "Beymen.Configuration.BackOffice/Beymen.Configuration.BackOffice.csproj"
COPY . .
WORKDIR "/src/Beymen.Configuration.BackOffice"
RUN dotnet build "Beymen.Configuration.BackOffice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Beymen.Configuration.BackOffice.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Beymen.Configuration.BackOffice.dll"]