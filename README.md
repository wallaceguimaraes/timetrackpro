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

## Setup / Deploy

1. Clone this repository.

2. Navigate to the project directory: Open a terminal and navigate to the project root directory using the cd command.

3. Before lifting the containers, we will create a network by running the following command:
docker network create time_track

4. Then list all networks with the command:
docker network ls

5. Open the terminal in the root folder of the project and run the command: 
docker compose up

6. Open a web browser and access the URL 
http://localhost:5000/api/request to view your running application.

7. Before running any other endpoints it is necessary to create the container for the database. Then access the ./sqlserver folder by some terminal and execute the command: docker compose up

8. After lifting the database container, access the project folder ./src/api and execute the command:
dotnet ef database update
This command will update the database with all the tables needed for the application.

9. After creating the database tables, create your first user with the following 'password' and 'salt' directly into the database on the 'Usuario' screen:

Encrypted password: rBG1oDjTq9qBhW4EI7ouNdkBxI9C/IdF/FlU1+hn5yg=
Salt:d2de614740c24985b7194ba7f095e5a9


10. Access the endpoint 

POST
http://localhost:5000/api/v1/authenticate

Body request:

{ "login": 'teste'; 
  "password": 12345678 
}

## Tests

To run all the project's tests, access the ./src/tests folder and run the command:
dotnet test

