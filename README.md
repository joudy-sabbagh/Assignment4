# Event Manager System

## Overview
The **Event Manager System** is an ASP.NET Core MVC application designed to help event managers organize venues, manage events, and track ticket assignments efficiently. This version is focused on the **admin/event manager side**, allowing CRUD operations for venues, events, attendees, and ticket assignments. Future updates will include attendee registration and ticket purchases.

## Features
### 1. Event Management
- Create, update, and delete events.
- Assign events to existing venues.
- Define tier-based pricing for tickets (Normal, VIP, Backstage).

### 2. Attendee and Ticket Management
- Add attendees manually for testing purposes.
- Assign tickets to attendees with tier selection.
- View which attendee has which ticket and for which event.

### 3. Sorting, Searching, and Filtering
- **Sorting**: Tickets can be sorted by **price** (ascending/descending).
- **Searching**: Events can be searched by **name** (case-insensitive).
- **Filtering**: Tickets can be filtered by **event** and **ticket tier**.

## User Stories
### Event Management
- **As an event manager**, I want to create new events with details (name, date, prices, venue) so that I can manage upcoming events.
- **As an event manager**, I want to update or delete events so that I can modify schedules or cancel events.
- **As an event manager**, I want to assign ticket tiers to events to differentiate between different levels of access.
- **As an event manager**, I want to view all available venues before creating an event so that I can select the correct location.
- **As an event manager**, I want to be able to manually add attendees to an event so that I can track ticket assignments for testing.

### Attendee and Ticket Management
- **As an admin**, I want to manually add attendees to events so that ticket sales can be tested.
- **As an admin**, I want to assign tickets to attendees based on tier selection.
- **As an admin**, I want to track which attendee has which ticket and for which event.
- **As an admin**, I want to filter tickets by event and tier so that I can quickly find relevant information.
- **As an admin**, I want an organized list of attendees, their tickets, and their assigned event so that I can manage them efficiently.

## Installation & Setup
### Prerequisites
- **.NET SDK 9.0+**
- **SQLite** for database management
- **Git** (for version control)

### Steps to Run Locally
1. **Clone the repository**:
    ```sh
    git clone "link" or clone from IDE directly if using Vscode.
   After cloning delete the .sln file which will pop up and proceed. This is so you wont need to specify the project.
    It should be called "MyMVCApp.csproj"
    ```
2. **Restore dependencies**:
    ```sh
    dotnet restore
    ```
3. **Check for pending migrations**:
    ```sh
    dotnet ef migrations list
    ```
    - If there are pending migrations, apply them in the next step.
4. **Apply database migrations**:
    ```sh
    dotnet ef database update
    ```
    - If the database does not exist or the update fails, reset the database by running:
    ```sh
    rm -f YourDatabase.db  # Delete existing database (Linux/macOS)
    del YourDatabase.db     # Delete existing database (Windows CMD)
    dotnet ef migrations add InitialCreate --context AppDbContext
    dotnet ef database update
    ```
5. **Run the application**:
    ```sh
    dotnet run
    ```
6. Open the browser and go to:
    ```
    http://localhost:5000
    ```

## Future Enhancements
- **Attendee registration & ticket purchasing**.
- **QR code-based ticket validation**.
- **Event analytics dashboard** (e.g., total sales per event).

## Author
**Amir Basha** - ID: **202205806**

---
This repository serves as the first phase of a full-scale **Event Management System**, focusing on **CRUD operations** and event organization. More features will be added in future updates.
