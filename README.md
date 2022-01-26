# BlazorWebGUI

## Navigation paths
/ -> main section of website
/login -> login page (in order to be able to navigate in some protected main page sectors)

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
