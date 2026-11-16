ARG M=mcr.microsoft
ARG C=com
FROM ${M}.${C}/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo el repositorio sin depender de rutas exactas
COPY . .

# Buscar el archivo .csproj donde esté ubicado y publicarlo
RUN find . -name "*.csproj" -exec dotnet publish {} -c Release -o /app/publish /p:UseAppHost=false \;

ARG M
ARG C
FROM ${M}.${C}/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
EXPOSE 8080

ENTRYPOINT ["dotnet", "FlotaGlobal.dll"]

