# RoomReservation
**Technology:**
* .Net5.0
* Microservices
* DDD
* CQRS
* MediatR 
* UnitTest
* IntegartionTest
* Repository
* ApiGateway(ocelot)
* Asp.Net Identity
* Fluent Validation
* Sql Server
* Docker Compose


## RoomReservationOnContainers - Microservices Architecture and Docker Containers 
RoomReservation .NET 5.0 application, based on a simplified microservices architecture and Docker containers.
>DISCLAIMER
>
>This application is a simplified microservice oriented architecture that use technologies like .NET 5.0 and Docker containers, it helps to implement scalable and autonomous application.
The next step (still not covered in RoomReservation) after understanding Docker containers and microservices development with .NET 5.0, is to select a microservice cluster/orchestrator like Docker Swarm, Kubernetes or DC/OS (in Azure Container Service) or Azure Service Fabric which in most of the cases will require additional partial changes to application's configuration (although the present architecture should work on most orchestrators with small changes). Additional steps would be to move databases to HA cloud services.
### Architecture overview: 
This application is cross-platform either at the server and client side, thanks to .NET 5.0 services capable of running on Linux or Windows containers depending on your Docker host. The architecture proposes a simplified microservice oriented architecture implementation with multiple autonomous microservices (each one owning its own data/db) and implementing different approaches within each microservice (simple CRUD vs. DDD/CQRS patterns) using Http as the current communication protocol.

>Important Note on Database Servers/Containers
>
>In this solution's current configuration for a development environment, the SQL databases are automatically deployed with sample data into a single SQL Server  (a single shared Docker container for SQL databases) so the whole solution can be up and running without any dependency to any cloud or specific server. Each database could also be deployed as a single Docker container.
>
>A similar case is defined in regards Redis cache running as a container for the development environment.
>
>However, in a real production environment it is recommended to have your databases (SQL Server and Redis, in this case) in HA (High Available) services like Azure SQL Database, Redis as a service or any other clustering system. If you want to change to a production configuration, you'll just need to change the connection strings once you have set up the servers in a HA cloud or on-premises.

## Overview of the application code
In this repo you can find an application that will implement a microservice architecture based application using .NET 5.0 and Docker.
The example business domain or scenario is based on a Reservations' application, which is implemented as a multi-container application. Each container is a microservice deployment (like the RoomReservation-microservice and Identity-microservice) which are developed using ASP.NET running on .NET 5.0 so they can run either on Linux Containers and Windows Containers.
This application use ApiGateway using ocelot that helps to implement cross cutting-concern like authentication and authorization, but In this application doesn't implement all the cross cutting-concern, and it gives developer a good opinion about them.
In This application will implement CQRS on code level using mediatR and different dbcontext for read and write, and also implement validation in mediatr pipeline using fluent validation.
Repository pattern uses for communication with database layer, and it's better to use generic repository to implement CRUD and some other generic methods in a repository and use everywhere that need like in commandhandler for use-case implementation.

### Identity
Identity will implement based on asp.net Identity and JWT token, this is a CRUD application, user will register and after login get JWT access_token and the JWT token should be send in header of all http requests, authentication and authorization should check in GateWay.



### User guid:

Project run on Docker with docker compose, migration created in efcore, when project run then at first get sql server image, next create containers, and the databases(IdentityDB, ReservationDB) should be create on sql data container.
User can use any tools like Postman for using Api services.

1- Should be register with this address: Post method http://localhost:5010/api/v1/identity/Register/Register

example data:

```
{
  "username": "jackuser",
  "firstname": "Jack",
  "lastname": "Jackie",
  "email": "user1@gmail.com",
  "password": "Ja@123",
  "confirmPassword": "Ja@123"
}
```

2- Should be login with this address: Post method http://localhost:5010/api/v1/identity/Account/Login

example data:

```
{
  "username": "jackuser",
  "password": "Ja@123",
  "rememberMe": true
}
```

After login you get an access_token that you should add it in any http request header as a Authorization.

3- Create room reservation with this address: Post method http://localhost:5010/api/v1/reservation/RoomReservation/CreateRoomReservation

example data:

```
{
    "roomid":1,
    "startdate":"2021-10-26 16:16:32.473",
    "enddate":"2021-10-26 16:59:32.473",
    "locationId":1
}
```

4- Get rooms from Berlin and Amsterdam locations with this address: Get method http://localhost:5010/api/v1/reservation/Room/GetRooms/1

officeId = 1 is Amsterdam,
officeId = 2 is Berlin,



5- Create resource room reservation with this address:  Post method http://localhost:5010/api/v1/reservation/ResourceReservation/CreateResourceReservation

example data:

```
{
    "roomReservationId":1,
    "ResourceId":1
}
```

