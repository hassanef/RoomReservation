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
* ApiGateway(ocelot framework)
* Asp.Net Identity
* Sql Server
* Docker Compose

## Microservices
I have been trying to implement this project based on Microservices, because the project can be scalable and can be implemented other projects with different technologies that they can communicate with each other so well, and also use Domain Driven Design for separate domain models and bounded context. 

## Developer Guid
This project consist of three projects, RoomReservation, Identity and ApiGateway.

I implement Identity based on Asp.net Identity and this is a CRUD service that doesn't need to implement base on DDD, it uses for Authentication, authorization and user management.
At first in this project user should register then user can log in and get a token(JWT), with that token user can send request to RoomReservation, but all request should be cross from ApiGateway.
ApiGateway uses for cross-cutting concern, and it has a lot of advantages, for example client doesn't need to know a bout other services and client just connect to ApiGateway and then ApiGateway manage all request from client to other services like RoomReservation.
In this project I have been trying to use CQRS pattern but just on code level not on databases level, if this application has a lot of request then it's better to use two different database(write and read).
