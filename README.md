# Event Manager System – Assignment 4

This project implements a Clean Architecture Event Manager System using ASP.NET Core MVC, Entity Framework Core, and SQLite.

## Prerequisites

* .NET SDK 7.0 or later
* Visual Studio 2022 or VS Code with C# extension
* Git
* (Optional) DB Browser for SQLite

## Setup

### Clone the repository

```bash
git clone https://github.com/joudy-sabbagh/Assignment4.git
cd Assignment4
```

### Restore dependencies

```bash
dotnet restore
```

## Database Configuration

The application uses SQLite. The database file `app.db` is located in the Presentation folder.

### Apply migrations

```bash
cd Presentation
dotnet tool install --global dotnet-ef    # if needed
dotnet ef database update
```

## Run the application

From the root folder run:

```bash
dotnet run --project Presentation
```

The server will start at:

```
https://localhost:5085
http://localhost:7106
```

Open one of these URLs in your browser.

## Application Usage

### Register as a regular user

1. Click **Register** in the navigation bar.
2. Fill out the form and submit.
3. You will have a user account with regular permissions.

### Admin login

Use the preconfigured admin account:

```
Email: joudy.f.sabbagh@gmail.com
Password: Admin123!
```

After logging in as admin you can create, edit, or delete Events, Venues, Attendees, and Tickets.
