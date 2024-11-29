FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY [".", "."]

RUN dotnet restore "AskifyAPI/src/Askify.Api/Askify.Api.csproj"
COPY . .

WORKDIR "/src/AskifyAPI/src/Askify.Api"
RUN dotnet build "Askify.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Askify.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Askify.Api.dll"]