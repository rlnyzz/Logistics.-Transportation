# ---------- BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish


# ---------- RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_CONTENTROOT=/app

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Logistics-Transportation.dll"]