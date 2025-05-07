# Airport Information System

A web-based system for flight information and ticket management.

## Project Overview

This ASP.NET Core MVC application provides a system for checking flights and booking tickets. It supports three types of users:

- **Anonymous users** – can search for flights by route and date
- **Clients** – can book tickets and view their purchase history
- **Administrators** – can cancel and add flights or routes

Flight data is pre-defined, and each flight has a unique code based on the airline prefix and date. Ticket prices are hardcoded and do not change dynamically.

## Technologies Used

- **Backend**: ASP.NET Core MVC, Entity Framework Core (Code-First), Identity Framework
- **Database**: Microsoft SQL Server
- **Frontend**: Razor Views, HTML/CSS, Bootstrap 
- **Authentication**: ASP.NET Core Identity with GUID-based users

## Main Entities

- **FlightRoute** – defines flight templates
- **Flight** – specific flight instances
- **AirplaneType** – airplane model and capacity
- **Company** – airline name and code prefix
- **City / Country** – locations
- **Ticket** – issued per user per flight
- **ApplicationUser** – inherits from IdentityUser<Guid>

## Features

- View flights by city pair and date (anonymous)
- See available seats, price, and company (anonymous)
- Purchase ticket after login (client)
- View ticket history (client)
- Cancel and add single flights or routes (admin)

## Author

Developed by **Iliyan Iliyanov**  