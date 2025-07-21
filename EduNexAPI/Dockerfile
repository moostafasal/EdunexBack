# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["EduNexAPI/EduNexAPI.csproj", "EduNexAPI/"]
COPY ["EduNexBL/EduNexBL.csproj", "EduNexBL/"]
COPY ["EDuNexDB/EduNexDB.csproj", "EDuNexDB/"]
RUN dotnet restore "EduNexAPI/EduNexAPI.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/EduNexAPI"
RUN dotnet publish "EduNexAPI.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose ports
EXPOSE 80
EXPOSE 443

# Default environment variables for DB connection
ENV DB_HOST=db18803.public.databaseasp.net \
    DB_NAME=db18803 \
    DB_USER=db18803 \
    DB_PASSWORD=Ne3+-2Jx9i=C

ENTRYPOINT ["dotnet", "EduNexAPI.dll"]
