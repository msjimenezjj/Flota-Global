FROM ://microsoft.com AS build
WORKDIR /src

COPY ["FlotaGlobal/FlotaGlobal.csproj", "FlotaGlobal/"]
RUN dotnet restore "./FlotaGlobal/FlotaGlobal.csproj"

COPY . .
WORKDIR "/src/FlotaGlobal"
RUN dotnet publish "./FlotaGlobal.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM ://microsoft.com AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
EXPOSE 8080

ENTRYPOINT ["dotnet", "FlotaGlobal.dll"]
