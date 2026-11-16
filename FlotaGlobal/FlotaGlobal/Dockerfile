## Etapa 1: Compilación en Linux (.NET 8 SDK)
FROM ://microsoft.com AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar el .csproj desde la subcarpeta FlotaGlobal y restaurar
COPY ["FlotaGlobal/FlotaGlobal.csproj", "FlotaGlobal/"]
RUN dotnet restore "./FlotaGlobal/FlotaGlobal.csproj"

# Copiar el resto del código y publicar
COPY . .
WORKDIR "/src/FlotaGlobal"
RUN dotnet publish "./FlotaGlobal.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa 2: Ejecución ligera en Linux (ASP.NET Core Runtime)
FROM ://microsoft.com AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render asigna dinámicamente el puerto mediante la variable PORT
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
EXPOSE 8080

ENTRYPOINT ["dotnet", "FlotaGlobal.dll"]
