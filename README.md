# TimeTrackPro

The application's target audience is professionals who need to record working hours in associated projects.

## Technologies Used

- ASP.NET Core 6 (C#)
- EntityFrameworkCore 5
- SQL Server database hosted on Heroku
- Docker to facilitate application setup
- XUnit for integration testing
- In-memory SQLite database for testing

## Endpoints

On endpoints that are not public, it is necessary to inform the bearer token in the authorization header

PUBLIC: YES
POST
http://localhost:5000/api/v1/authenticate

{
	"login": "teste",
	"password": "12345678"
}

Authenticate user using token bearer.

--------------------------------------------------------------
PUBLIC: NO
POST
http://localhost:5000/api/v1/times

{
	"project_id": 2,
	"user_id": 1,
	"started_at": "2023-07-10T00:49:30.6145613",
	"ended_at": "2023-10-10T00:49:30.6145613"
}

Registers new time in which the user is allocated in the project.

-------------------------------------------------------------------
 
 PUBLIC: NO
 GET
 http://localhost:5000/api/v{n}/users/{uder_id}

 Search user through its identifier.

 -------------------------------------------
 PUBLIC: NO
 http://localhost:5000/api/v1/users
 POST

 {
	"name": "teste",
	"email": "teste",
	"login": "teste1",
	"password": "12345"
}

Create new user.

-----------------------------------------------------------
 PUBLIC: NO 
 http://localhost:5000/api/v1/users/{uder_id}
 PUT

 {
	"name": "teste primeiro",
	"email": "teste@email.com",
	"login":  "teste",
	"password": "12345678"
}

Update user.

------------------------------------------------------------------

PUBLIC: NO 
http://localhost:5000/api/v1/projects
GET

Fetches all projects of the authenticated user

----------------------------------------------------

PUBLIC: NO
http://localhost:5000/api/v1/projects/{project_id}
GET

Search specific project

--------------------------------------------------------

PUBLIC: NO
http://localhost:5000/api/v1/projects
POST

{
	"title": "sonar",
	"description": "corrige bugs",
	"user_id": [1]
}

Create new project

--------------------------------------------------------

PUBLIC: NO
http://localhost:5000/api/v{n}/projects/
GET

Search specific project

---------------------------------------------------------
PUBLIC: NO
http://localhost:5000/api/v1/projects/{project_id}
PUT

{
	"title": "sonar",
	"description": "corrige bugs pequenos",
	"user_id": [
		1
	]
}

Update specific project 

-------------------------------------------------------
PUBLIC: NO
http://localhost:5000/api/v1/times/{project_id}
GET

---------------------------------------------------
PUBLIC: NO
http://localhost:5000/api/v1/times/{time_id}
PUT

{
	"project_id": 2,
	"user_id": 1,
	"started_at": "2023-07-10T00:49:30.6145613",
	"ended_at": "2023-09-10T00:49:30.6145613"
}



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

8. List the containers within the network with the command:
 docker network inspect time_track

 If the network does not have any container connected, run the command:
 to list the containers and add them to the network: docker ps
 
 docker network connect time_track timetrack
 docker network connect time_track sqlserver-mssql-1

9. After lifting the database container, access the project folder ./src/api and execute the command:
dotnet ef database update
This command will update the database with all the tables needed for the application.

10. After creating the database tables, create your first user with the following 'password' and 'salt' directly into the database on the 'Usuario' screen:

Encrypted password: rBG1oDjTq9qBhW4EI7ouNdkBxI9C/IdF/FlU1+hn5yg=
Salt: d2de614740c24985b7194ba7f095e5a9


11. Access the endpoint 

POST
http://localhost:5000/api/v1/authenticate

Body request:

{ "login": 'teste'; 
  "password": 12345678 
}

