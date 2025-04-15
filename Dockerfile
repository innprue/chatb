# Usar imagen base de .NET SDK
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Usar imagen base de .NET SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WhatsappNet.Api/WhatsappNet.Api.csproj", "WhatsappNet.Api/"]
RUN dotnet restore "WhatsappNet.Api/WhatsappNet.Api.csproj"
COPY . .
WORKDIR "/src/WhatsappNet.Api"
RUN dotnet build "WhatsappNet.Api.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "WhatsappNet.Api.csproj" -c Release -o /app/publish

# Imagen final para producción
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WhatsappNet.Api.dll"]
