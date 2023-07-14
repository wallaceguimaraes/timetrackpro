# TimeTrackPro

The application's target audience is professionals who need to record working hours in associated projects.

## Technologies Used

- ASP.NET Core 6 (C#)
- EntityFrameworkCore 5
- SQL Server database hosted on Heroku
- Docker to facilitate application setup
- XUnit for integration testing
- In-memory SQLite database for testing

## Description

[]

## Dependencies

- Microsoft.AspNetCore.Authentication.JwtBearer v3.1.12
- Microsoft.AspNetCore.Authentication v2.2.0
- Hashids.net v1.7.0
- Microsoft.AspNetCore.Mvc.NewtonsoftJson v3.1.28
- Microsoft.EntityFrameworkCore v5.0.5
- Microsoft.EntityFrameworkCore.Design v5.0.5
- Microsoft.EntityFrameworkCore.Relational v5.0.5
- Microsoft.EntityFrameworkCore.SqlServer v5.0.5
- Microsoft.EntityFrameworkCore.Tools v5.0.5
- Microsoft.IdentityModel.Tokens v6.31.0
- Newtonsoft.Json v13.0.3
- Swashbuckle.AspNetCore v6.2.3
- System.IdentityModel.Tokens.Jwt v6.31.0

## Setup

1. Install .NET 6 SDK: ASP.NET Core 6 requires .NET 6 SDK installed on your machine. You can download the appropriate SDK for your operating system from the official Microsoft website: https://dotnet.microsoft.com/download/dotnet/6.0

2. Verify the installation: After installing the .NET 6 SDK, open a terminal (such as Command Prompt on Windows or Terminal on macOS/Linux) and run the command dotnet --version to verify that the installation completed correctly. It should display the installed .NET SDK version.

3. Clone this repository.

4. Navigate to the project directory: Open a terminal and navigate to the project root directory using the cd command.

5. Access the folder ./src/api and run: dotnet restore 
and then run command: dotnet run
This will compile the project and start a local web server to host your application. By default, the application will be available at http://localhost:5000 (or https://localhost:5001 for HTTPS).

6> Access the application in the browser: Open a web browser and access the URL 
http://localhost:5000/api/request to view your running application.


## Deploy

Open the terminal in the root folder of the project and run the command: 
 
docker compose up.

After the command is executed, the container will be created, when the creation of the container is finished, the application will be available at the address. 

http://localhost:5000

The database will be temporarily hosted on heroku so you don't have to worry about database setup.
## Tests

To run all the project's tests, access the ./src/tests folder and run the command:
dotnet test

## Development Hours

[]

## Contact

[]