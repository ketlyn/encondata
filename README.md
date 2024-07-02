# TechsysLog

This application is divided into 3 parts

MVC presentation

          1 - I include a SQL script in the Presentation layer that can be generating the base
          2-Work with DelegatingHandler concept ,HttpClientFactory,Polly and Circuit Breaker

API Identity=
            1-Identity API, segregate user creation format
            2-As a result, I use Identity (https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio)
            
            3-All apis make use of Entity Framework and Migrations
            4-All APIs make use of JWT with shared token
API

            1-For Entities modeling, I made a lot of use of DDD and SOLID (https://en.wikipedia.org/wiki/Open%E2%80%93closed_principle)
            2- IUnitOfWork (https://martinfowler.com/eaaCatalog/unitOfWork.html)
            3- For http requests, I worked with mediator (https://www.geeksforgeeks.org/mediator-design-pattern/)
            4- I also applied CQRS principles (https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)
            5-All APIs make use of JWT with shared token




            Need to run the 3 applications
