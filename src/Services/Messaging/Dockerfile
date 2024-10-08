
# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /Messaging

# Copy the project files and restore dependencies
COPY Messaging.Api/Messaging.Api.csproj ./Messaging.Api/
COPY Messaging.Application/Messaging.Application.csproj ./Messaging.Application/
COPY Messaging.Domain/Messaging.Domain.csproj ./Messaging.Domain/
COPY Messaging.External/Messaging.External.csproj ./Messaging.External/
COPY Messaging.Persistence/Messaging.Persistence.csproj ./Messaging.Persistence/

RUN dotnet restore Messaging.Api/Messaging.Api.csproj

# Copy the rest of the code and build the application
COPY . ./
WORKDIR /Messaging/Messaging.Api
RUN dotnet build -c Release -o /app/build

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# Use the official ASP.NET Core runtime image for running
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /Messaging
COPY --from=build /app/publish .

# Expose the port that the app runs on
EXPOSE 80
EXPOSE 5002

# Set the entry point to the application
ENTRYPOINT ["dotnet", "Messaging.Api.dll"]