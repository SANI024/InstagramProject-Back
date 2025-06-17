# Stage 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only the project file first
COPY ./InstagramProjectBack/InstagramProjectBack.csproj ./InstagramProjectBack/
RUN dotnet restore ./InstagramProjectBack/InstagramProjectBack.csproj

# Copy the full project
COPY ./InstagramProjectBack/ ./InstagramProjectBack/

# Set working directory and publish
WORKDIR /src/InstagramProjectBack
RUN dotnet publish -c Release -o /app/out

# Stage 2: Run the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "InstagramProjectBack.dll"]
