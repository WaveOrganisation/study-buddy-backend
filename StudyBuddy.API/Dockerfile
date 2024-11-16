FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StudyBuddy.API/StudyBuddy.API.csproj", "StudyBuddy.API/"]
COPY ["StudyBuddy.Application/StudyBuddy.Application.csproj", "StudyBuddy.Application/"]
COPY ["StudyBuddy.Domain/StudyBuddy.Domain.csproj", "StudyBuddy.Domain/"]
COPY ["StudyBuddy.Persistence/StudyBuddy.Persistence.csproj", "StudyBuddy.Persistence/"]
COPY ["StudyBuddy.Infrastructure/StudyBuddy.Infrastructure.csproj", "StudyBuddy.Infrastructure/"]
RUN dotnet restore "StudyBuddy.API/StudyBuddy.API.csproj"
COPY . .
WORKDIR "/src/StudyBuddy.API"
RUN dotnet build "StudyBuddy.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StudyBuddy.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudyBuddy.API.dll"]
