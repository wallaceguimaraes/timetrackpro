FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY . .
RUN dotnet restore "./src/api/api.csproj" --disable-parallel
RUN dotnet publish "./src/api/api.csproj" -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

ENV TZ=America/Sao_Paulo
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "api.dll"]
