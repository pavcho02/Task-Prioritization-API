# Task-Prioritization-API
ASP.NET RESTful API for task prioritization
## Description
RESTful API that allows users to create, read, update, and delete tasks while introducing a smart priority system based on predefined business rules and user-defined parameters.
## Features
- Creating a task
- Reading a task/tasks(User can specify which reading method to use)
  - Sorting the tasks by priority or dueDate
  - Filtering the tasks by isCompleted or priority
  - Get task by id
  - Get all task sorted by default(by priority)
- Updating an existing task
- Deleting a task
## Setup instructions
### Prerequisites
Before setting up the project, make sure you have the following installed:
- .NET 6.0 or later
- Visual Studio 2022 or later, or Visual Studio Code
- SQL Server or SQL Server Express
- Postman or any other API testing tool (optional)

### Clone the Repository
1. Clone the repository to your local machine:
    ```
    git clone https://github.com/yourusername/TaskPrioritizatorAPI.git
    ```

### Setup the Database
1. Open **SQL Server Management Studio** (SSMS) or any SQL Server tool.
2. Create a new database called `TaskPrioritizatorDb`.
3. In your local project, configure the connection string in `appsettings.json`:
    ```json
    "ConnectionStrings": {
        "TaskDb": "Server=(localdb)\\YourServer;Database=YourDB;Trusted_Connection=True;"
    }
    ```
4. Run the migration to create the database schema:
      ```bash
      //In Terminal
      dotnet ef migrations add InitialCreate
      dotnet ef database update

      //in Package Manager Console(Visual Studio)
      Add-Migration Initial
      Update-Database
      ```

### Running the Project
1. Open the solution in **Visual Studio** or **Visual Studio Code**.
2. Build the project by pressing `Ctrl+Shift+B` or `F6`.
3. Start the project by pressing `F5` or using the terminal:
    ```bash
    dotnet run
    ```
4. The API should now be running locally at `http://localhost:5207`.

### Testing
1. You can use **Postman** or any other API tool to test the endpoints.
2. Set the request method and URL for each endpoint based on your API needs.

### Unit Testing
1. Open the **Test Project** in Visual Studio.
2. Run the tests using Visual Studio Test Explorer or using the following command:
    ```bash
    dotnet test
    ```

## Endpoints examples
### Base URL
```
http://localhost:5027/tasks
```
### Endpoints:
---
#### 1. Create a Task
#### `POST /tasks`
Creates a new task.
##### Request Body:
```json
{
  "title": "Task Title",
  "description": "Task Description",
  "dueDate": "2025-03-01",
  "isCritical": true
}
```
#### Response:
- **Status**: `201 Created`
- **Body**: The created task.
```json
{
  "id": 1,
  "title": "Task Title",
  "description": "Task Description",
  "dueDate": "2025-03-01",
  "priority": "high",
  "isCompleted": false
}
```
---

## 2. Get All Tasks

### `GET /tasks`
Retrieves all tasks.
#### Query Parameters:
- `sort`: The sorting criteria (`priority` or `dueDate`).
- `filter`: The filter criteria (`priority` or `isCompleted`).
- `value`: The value for the filter (e.g., `high`, `medium`, `low`, `true`, `false`).
#### Example Request:
- To get all tasks sorted by priority:
  ```
  GET /tasks?sort=priority
  ```
- To get all tasks filtered by priority `high`:
  ```
  GET /tasks?filter=priority&value=high
  ```
#### Response:
- **Status**: `200 OK`
- **Body**: A list of tasks.
```json
[
  {
    "id": 1,
    "title": "Task 1",
    "description": "Description 1",
    "dueDate": "2025-03-01",
    "priority": "high",
    "isCompleted": false
  },
  {
    "id": 2,
    "title": "Task 2",
    "description": "Description 2",
    "dueDate": "2025-03-05",
    "priority": "medium",
    "isCompleted": false
  }
]
```
---
## 3. Get Task By ID

### `GET /tasks/{id}`

Retrieves a specific task by its ID.

#### Example Request:
```
GET /tasks/1
```
#### Response:
- **Status**: `200 OK` (if task found)
- **Body**: The task details.
```json
{
  "id": 1,
  "title": "Task 1",
  "description": "Description 1",
  "dueDate": "2025-03-01",
  "priority": "high",
  "isCompleted": false
}
```
- **Status**: `404 Not Found` (if task not found)
- **Body**: Error message.
```json
{
  "message": "Invalid task ID"
}
```
---
## 4. Update a Task
### `PUT /tasks/{id}`
Updates a specific task by its ID.
#### Request Body:
```json
{
  "id": 1,
  "title": "Updated Task Title",
  "description": "Updated Description",
  "dueDate": "2025-04-01",
  "isCompleted": true
}
```
#### Example Request:
```
PUT /tasks/1
```
#### Response:
- **Status**: `200 OK`
- **Body**: The updated task.
```json
{
  "id": 1,
  "title": "Updated Task Title",
  "description": "Updated Description",
  "dueDate": "2025-04-01",
  "priority": "high",
  "isCompleted": true
}
```
- **Status**: `404 Not Found` (if task not found)
- **Body**: Error message.
```json
{
  "message": "Invalid task ID"
}
```
- **Status**: `400 Bad Request` (if invalid request data)
- **Body**: Error message.
```json
{
  "message": "Invalid request data"
}
```
---
## 5. Delete a Task
### `DELETE /tasks/{id}`
Deletes a specific task by its ID.
#### Example Request:
```
DELETE /tasks/1
```
#### Response:
- **Status**: `200 OK` (if deletion was successful)
- **Body**: Empty response body.

- **Status**: `404 Not Found` (if task not found)
- **Body**: Error message.
```json
{
  "message": "Invalid task ID"
}
```
## Future improvements
### Authentication & Authorization
- Authentication: Only authorized users should be able to create, update, or delete tasks. This ensures that sensitive data isn't accessible to unauthorized users.
- Authorization: Role-based access control - administrator to maintain the api and tasks assosiated with it's creator.
