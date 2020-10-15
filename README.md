# HarborControl

## Project Overview 

- The harbor has a perimeter of 10km between the dock, and the open sea.
-	The harbor can only allow one boat into the perimeter at a time.
-	Any boat that enters the perimeter will complete the 10km journey into the harbor before it reaches the dock.
-	No other boat may enter the 10km perimeter once a boat is inside the perimeter.
-	Boats arrive at the perimeter randomly.
-	All boats that arrive at the perimeter must wait at the 10km perimeter line before they are ordered to ender the perimeter by harbor control.
-	There are 3 types of boats:
-	Speedboat	(Speed: 30km/h)
-	Sailboat (Speed: 15km/h)
-	Cargo ship (Speed: 5km/h)
-	These types of boats can arrive at the perimeter at any time.
-	Once the boat in the perimeter has completed the 10km journey and docked, a boat waiting at the perimeter may enter.


## Snapshot example

-	There are 2 speedboats and a Sailboat at the perimeter
-	There is one Cargo ship inside the perimeter
-  The 2 speedboats and sailboat need to wait for the cargo ship to reach the dock before harbor control can signal one of them to enter the perimeter.

## Technology 


```sh
Frontend     : Angular V9.1
Backend API  : ASP.NET core 3.1 (Including Swagger)
WebSchedular : Hangfire
Wether api   : api.openweathermap.org
```
# Database
```sh
Miscrosoft SQL server 2017
```

# Demo Wether API URL 

```sh
API : - http://api.openweathermap.org/data/2.5/weather?q=Durban&appid=<APIKey>
```

### Installation

Install the Angular CLI

```sh
npm install -g @angular/cli
```
```sh
npm install (used to retore the node modules)
```

```sh
ng serve -o (Run Angular)
```

Install .NET Core 3.x

To download the latest version of .NET Core, go to https://dotnet.microsoft.com/download and select the platform you are using.

### Project URL 
- swagger
http://localhost:58031/swagger/index.html

- hangfire
http://localhost:58031/hangfire

- Angular 
http://localhost:4200/#/home




