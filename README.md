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

## Microservices
I tried to implement this project based on Microservices, because the project can be scalable that mentioned in Dephion case document, and also use Domain Driven Design for separate domain models and bounded context,  Microservices and DDD have a lot of features and very useful when you want to implement scalable services, and also you want to implement some other services that they want to communicate with each other.

## Description about technology
This project consist of two Api services(RoomReservation, Identity) and a Gateway(Api Gateway).
I use Gateway because it helps to implementation of cross cutting-concern, all request should cross from Gateway then the client doesn't know about services. in this Gateway I just implemented authentication and authorization, but other cross-cutting concern  like logging, caching etc can be implemented there.
I implemented Identity based on Asp.net Identity with JWT token, and this is a CRUD service that doesn't need to implement base on DDD, it uses for Authentication, authorization and user management.
In Reservation Api, I tried to use CQRS pattern with MediatR, but just on code level not on databases level, if this application has a lot of request then it's better to use two different database(write and read), and I implemented validation of requst with fluent validation that implemenrted in MedatR pipeline.

## Gateway 
When a request send from client then at first it should cross from Gateway, in Gateway that request should be authenticated and authorization, for about authentication at first JWT token checked and the request send to Identity Api for authorization, if the request authorized the Gateway send request to Reservation Api, Gateway has an ocelot.json file that has configuration of upstream and downstream.

### Project services:
Accout and Register in Identity Api.
RoomReservation, Room, ResourceReservation in Reservation Api

### User guid:

Project run on Docker with docker compose, I created migration in efcore, when project run then at first get sql server image, create containers, and the databases(IdentityDB, ReservationDB) should be created on sql data container.
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
    "location":"Amsterdam"
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

