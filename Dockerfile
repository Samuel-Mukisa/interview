# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

# Copy the project files to the container
COPY . .

# Set the working directory to the location of the csproj file
WORKDIR /app/Ishop.Gui

# Build the application
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Use the official .NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /app

# Copy the published application from the build image
COPY --from=build /app/Ishop.Gui/out .

# Expose the port your application will run on
EXPOSE 80

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Ishop.Gui.dll"]
