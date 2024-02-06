# Credit Cards PoC

## Running the project

- Copy `example.env` in the same directory and rename the copied file to `.env` 

### Requirements

- You need to have Docker Desktop installed, up and running.

### Through the CLI

__You may need to open Powershell as Administrator__

```
cd credit-cards-poc/
docker-compose up --build
```

### Through Visual Studio

__You may need to open Visual Studio as Administrator__

Set `docker-compose` as Startup project, Ctrl + F5

## Accessing the database through SSMS

1. Select `SQL Server Authentication` from the dropdown.
2. Server name: `localhost,1401`
3. User name: `sa`
4. Password: `String1@`
5. Click `Connect`
