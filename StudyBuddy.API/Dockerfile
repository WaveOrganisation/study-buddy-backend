FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["StudyBuddy.API/StudyBuddy.API.csproj", "StudyBuddy.API/"]

RUN dotnet restore "StudyBuddy.API/StudyBuddy.API.csproj"

COPY . .

WORKDIR /src/StudyBuddy.API
RUN dotnet build "StudyBuddy.API.csproj" -c Release -o /app/build

RUN dotnet publish "StudyBuddy.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "StudyBuddy.API.dll"]
