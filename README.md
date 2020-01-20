# MediatR.AspNetCore.Endpoints

[![NuGet](https://img.shields.io/nuget/dt/mediatr.aspnetcore.endpoints.svg)](https://www.nuget.org/packages/mediatr.aspnetcore.endpoints) 
[![NuGet](https://img.shields.io/nuget/vpre/mediatr.aspnetcore.endpoints.svg)](https://www.nuget.org/packages/mediatr.aspnetcore.endpoints)


## Bechmark for a http request

Request : {"Id":47,"Name":"kahbazi"}

Respon : {"Message":"Hello kahbazi"}

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-3632QM CPU 2.20GHz (Ivy Bridge), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.101
  [Host]     : .NET Core 3.1.1 (CoreCLR 4.700.19.60701, CoreFX 4.700.19.60801), X64 RyuJIT
  DefaultJob : .NET Core 3.1.1 (CoreCLR 4.700.19.60701, CoreFX 4.700.19.60801), X64 RyuJIT
Method	Mean	Error	StdDev	Rank	Gen 0	Gen 1	Gen 2	Allocated
Mediator	56.14 us	1.427 us	2.462 us	1	2.9297	-	-	9.18 KB
Mvc	151.90 us	2.980 us	3.060 us	2	4.8828	-	-	16.29 KB