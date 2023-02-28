# EpilepsyDB

## Quick Start

To start the application on a local computer, follow these steps.
1. Open the page https://github.com/Jakalama/EpilepsyDB.
2. Clone the project to a local directory.
3. Ensure that a PostgreSQL instance is running on the computer. If PostgreSQL is not already installed, follow the instruction on: https://www.postgresql.org/download/. Otherwise, follow the instructions below.
   1. Press "Windows Key + R".
   2. Type `services.msc`.
   3. Search for a service like this `postgresql-x64-13 PostgreSQL Server 13`. The specific version is not important.
   4. Check if the service is marked as "running". If not, right-click the PostgreSQL column and click on "Start".
4. Open the project in a Code Editor. (The following instructions assume that you are using Visual Studio 2022)
5. Edit the `appsettings.Development.json` file to insert the database connection information.
Every environment variable starting with an uppercase "DB" is needed to be set.
   1. "DB_HOST" should be set to "localhost" if the database runs locally.
   2. "DB_PORT" specifies the port of the PostgreSQL server. PostgreSQL usually uses 5432.
   3. "DB_NAME" specifies the name of the new database. (Can be chosen arbitrarily)
   4. "DB_USER" insert the name of the PostgreSQL user. PostgreSQL usually uses "postgres".
   5. "DB_PASS" insert the password for the PostgreSQL user.
6. Configure the credentials for the first admin. (See this for more info: https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows)
   1. Change the value of the `UserZero` variable inside the `appsettings.Development.json`. It should be an Email address.
   2. Change the value of the `UserZeroPW` variable inside the `appsettings.Development.json`. It should contain the password.
7. Inside Visual Studio, click on "Run" to execute the application.


## Release

The application can be released as is or packaged inside a Docker container.
This manual only gives an overview of how to execute it inside a Docker container.

**Note:** As sensible configuration data should not be stored inside the repository, it is important to securely provide the final `appsettings.json` file. Another possibility is setting all the required variables via environment variables and not using the `appsettings.json` file.

**Note:** Ensure SSL and the Email Service are configured correctly.

**Note:** This README does not explain configuring a web server like Nginx. To publish the application, it is recommended to follow the official documentation. http://nginx.org/en/docs/

### Docker

1. Make sure the Docker Deamon is installed on the system. Instructions can be found here: https://docs.docker.com/engine/install/
2. EpilepsyDB already has a preconfigured Dockerfile.
   1. Default the application will try to start listening on ports 80 and 443.
   2. To change the application port for the Build, set another port. This can be done by 
   providing an environment variable or via an explicit code call. Provide the variable like this `ASPNETCORE_URLS=http://+:80;https://+:443` or add `UseUrls("http://localhost:80")` to the WebHostConfiguration inside the Program.cs.
3. Navigate inside a command prompt to the directory with the Docker file inside.
4. Build the Docker Image with `docker build -t epilepsyDB_image .`
5. Run the application inside a Docker container with `docker run -p 80:80 epilepsyDB_image`. Connecting to a database not running in the same environment needs special attention. It may be helpful to package the database in a Docker container and start both containers using docker-compose.

### Docker Compose

If the application and the database should run inside a Docker Container, use the `docker-compose.yml`. 

1. Set the desired database connection inside the `.env` file.
2. Adapt ports inside `docker-compose.yml` and inside the `Dockerfile`.
3. Run `docker-compose build`.
4. Run `docker-compose up`.


## Change the Database Engine

To change the Database Engine from PostgreSQL to another, modify the `AddDbContext` call inside the `Startup.cs`. For more information, see https://learn.microsoft.com/de-de/aspnet/core/tutorials/first-mvc-app/working-with-sql?view=aspnetcore-6.0&tabs=visual-studio

## Configure SSL

To use SSL, further configuration is needed. See https://learn.microsoft.com/de-de/aspnet/core/security/authentication/certauth?view=aspnetcore-6.0

## Configure Email Service

To use the application correctly, you must insert the connection information of your email provider and email account. If you use Gmail for this, remember to enable the Gmail API under this link https://console.developers.google.com/apis/dashboard. 
These can be directly inserted inside the `appsettings.json` file or injected via an environment variable into the file.

**Note:** The data already inside the file serves only as an example.

## API Endpoints

See Swagger UI: /swagger/index.html
It is only enabled in a development environment.

### Change the API token lifetime

To change the JWT token lifetime, you need to change the value of `EXPIRATION_MINUTES` inside the `JwtService` class.
