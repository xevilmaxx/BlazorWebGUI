# BlazorWebGUI

## Navigation paths

**/** -> main section of website (some parts are protected from unlogged users, please go to /login (manually type in browser navigation) and log in with correct credentials)

**/login** -> login page (in order to be able to navigate in some protected main page sectors)

**/subcat00** -> (had crafted sub categories)

**/subcat10** -> (dynamic sub categories)

---

## Credentials:

**Username:**

pi

**Password:**

pwd


---

## Features

Framework:
* NET 5.0

GUI:
* Blazor [Engine]
* MudBlazor (Bootstrap alternative) [GUI]

API:
* HTTPS
* GRPC-WEB (suggested mode as it is more intuitive becouse has strong types and supports ServiceReference in VisualStudio 2019+)
* HTTP (Alpha stage support derived directly from GRPC endpoint, alternatively you will need to create a dedicated HTTP endpoints but its not too productive)
  - Swagger as automatic API descriptor (HTTP)
* Authentication
  - Initially: Username + Password (eventually password passed Hashed)
  - Generation of JWT Token (for now without expiring date)
  - Use of JWT Token for any website navigation, GRPC call or HTTP call
    - Retrieve user data from Token and eventual activity tracing
  - Negate API access if Token is not valid
* Testable with BloomRPC / Postman (regarding HTTP endpoint)

Backend to Frontend (without additional protection):
* Push notifications through SignalR (internal dedicated channel)

Server to Subscribers (without additional protection):
* Push notifications through SignalR (public dedicated channel)
* Instead of WebSocket, SignalR was preferred for its simplicity, which anyway can use WebSocket (eventually still to review)

---

## Blazor layouts

https://blazor-university.com/layouts/using-layouts/

https://blazor-university.com/layouts/nested-layouts/

https://blazor-tutorial.net/layout



## Menu

MudNavLink

https://mudblazor.com/components/navmenu#icons



## GRPC Web API

https://blog.stevensanderson.com/2020/01/15/2020-01-15-grpc-web-in-blazor-webassembly/

https://azure.github.io/AppService/2021/03/15/How-to-use-gRPC-Web-with-Blazor-WebAssembly-on-App-Service.html

https://docs.microsoft.com/it-it/aspnet/core/grpc/browser?view=aspnetcore-5.0



## GRPC AS REST API:

https://github.com/aspnet/AspLabs/tree/main/src/GrpcHttpApi

https://cloud.google.com/service-infrastructure/docs/service-management/reference/rpc/google.api#google.api.HttpRule

https://docs.microsoft.com/it-it/aspnet/core/grpc/httpapi?view=aspnetcore-5.0





https://cloud.google.com/endpoints/docs/grpc/transcoding
