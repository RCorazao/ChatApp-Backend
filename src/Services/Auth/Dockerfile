
# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /auth

# Copy the project files and restore dependencies
COPY Auth.Api/Auth.Api.csproj ./Auth.Api/
COPY Auth.Application/Auth.Application.csproj ./Auth.Application/
COPY Auth.Domain/Auth.Domain.csproj ./Auth.Domain/
COPY Auth.External/Auth.External.csproj ./Auth.External/
COPY Auth.Persistence/Auth.Persistence.csproj ./Auth.Persistence/

RUN dotnet restore Auth.Api/Auth.Api.csproj

# Copy the rest of the code and build the application
COPY . ./
WORKDIR /auth/Auth.Api
RUN dotnet build -c Release -o /app/build

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# Use the official ASP.NET Core runtime image for running
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /auth
COPY --from=build /app/publish .

# Expose the port that the app runs on
EXPOSE 80
EXPOSE 5001

# Set the entry point to the application
ENTRYPOINT ["dotnet", "Auth.Api.dll"]