# Simpl.Expenses

A simple expense management Web API built with .NET 8.

## Getting Started

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 or Visual Studio Code

### Running the Application

1. Clone the repository
2. Navigate to the project directory
3. Run the application:
   ```bash
   dotnet run --project src/Core.WebApi
   ```
4. Open your browser and navigate to `https://localhost:7242/swagger` to access the Swagger UI

### Project Structure

- `src/Core.WebApi/` - Main Web API project
  - Contains controllers, configuration, and startup logic
  - Uses Swagger for API documentation

## Development

The application is configured to run on:
- HTTPS: `https://localhost:7242`
- HTTP: `http://localhost:5212`

## API Documentation

Once the application is running, you can view the API documentation at:
- Swagger UI: `https://localhost:7242/swagger`
