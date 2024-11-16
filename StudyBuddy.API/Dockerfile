FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StudyBuddy.API/StudyBuddy.API.csproj", "StudyBuddy.API/"]
RUN dotnet restore "StudyBuddy.API/StudyBuddy.API.csproj"
COPY . .
WORKDIR "/src/StudyBuddy.API"
RUN dotnet build "StudyBuddy.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "StudyBuddy.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudyBuddy.API.dll"]
