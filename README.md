# Profiles API

Profiles API is a backend service that manages user profile data, providing endpoints for creating, retrieving, updating, and deleting profile information.

## Prerequisites

Before working on this project, make sure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 10.0 or later)
- [Git](https://git-scm.com/downloads)
- An IDE such as [Visual Studio 2022](https://visualstudio.microsoft.com/), [Visual Studio Code](https://code.visualstudio.com/), or [JetBrains Rider](https://www.jetbrains.com/rider/)

## Local Setup & Run

1. **Clone the repository**

   ```bash
   git clone https://github.com/EvgeniyaInternship/InnoProfilesApi.git
   cd InnoProfilesApi
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Build the project**

   ```bash
   cd ProfilesApi
   dotnet build
   ```

4. **Run the API**

   The solution is split into multiple projects. The executable entry point is `ProfilesApi.Api`, so run it directly by project:

   ```bash
   dotnet run --project ProfilesApi.Api
   ```

5. **Verify it's running**

   By default, the API should be available at:

   ```
   https://localhost:5001
   http://localhost:5000
   ```
