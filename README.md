# MediatR.AspNetCore.Endpoints

[![NuGet](https://img.shields.io/nuget/dt/mediatr.aspnetcore.endpoints.svg)](https://www.nuget.org/packages/mediatr.aspnetcore.endpoints) 
[![NuGet](https://img.shields.io/nuget/vpre/mediatr.aspnetcore.endpoints.svg)](https://www.nuget.org/packages/mediatr.aspnetcore.endpoints)



## Bechmark for a simple http request

Request: 
```json
{
  "Id" : 47,
  "Name" : "kahbazi"
}
```

Response:
```json 
{
  "Message" : "Hello kahbazi"
}
```

  
| Method | Mean | Error | StdDev | Rank | Gen 0 | Gen 1 | Gen 2 | Allocated |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| MediatR.AspNetCore.Endpoints | 56.14 us | 1.427 us | 2.462 us | 1 | 2.9297 | - | - | 9.18 KB |
| Microsoft.AspNetCore.Mvc | 151.90 us | 2.980 us | 3.060 us | 2 | 4.8828 | - | - | 16.29 KB |
