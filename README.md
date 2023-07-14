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

3. Open the terminal in the root folder of the project and run the command: 
docker compose up

4. Open a web browser and access the URL 
http://localhost:5000/api/request to view your running application.

5. Access the endpoint 

POST
http://localhost:5000/api/v1/authenticate

Body request:

{ "login": 'teste'; 
  "password": 12345678 
}

Use this user already registered in the database

"login": "teste",
"password": "12345678"

The database is temporarily hosted on the heroku server.

