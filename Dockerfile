FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["StudyBuddy/StudyBuddy.Api/StudyBuddy.Api.csproj", "StudyBuddy.Api/"]
RUN dotnet restore "StudyBuddy.Api/StudyBuddy.Api.csproj"
COPY . .
WORKDIR "/src/StudyBuddy.Api"
RUN dotnet build "StudyBuddy.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StudyBuddy.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudyBuddy.Api.dll"]
