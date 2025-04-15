# Usar la imagen base de .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Definir el directorio de trabajo
WORKDIR /src

# Copiar el archivo .csproj y restaurar las dependencias
COPY ["WhatsappNet.Api/WhatsappNet.Api.csproj", "WhatsappNet.Api/"]

# Restaurar las dependencias
RUN dotnet restore "WhatsappNet.Api/WhatsappNet.Api.csproj"

# Copiar el resto de los archivos
COPY . .

# Publicar la aplicación
RUN dotnet publish "WhatsappNet.Api/WhatsappNet.Api.csproj" -c Release -o /app/publish

# Usar la imagen de runtime para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=build /app/publish .

# Exponer el puerto de la aplicación
EXPOSE 80

# Definir el comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "WhatsappNet.Api.dll"]
