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
## Endpoints examples
## Base URL
```
http://localhost:5027/tasks
```
### Endpoints:
---
## 1. Create a Task
### `POST /tasks`
Creates a new task.
#### Request Body:
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
## Setup instructions
## Future improvements
### Authentication & Authorization
- Authentication: Only authorized users should be able to create, update, or delete tasks. This ensures that sensitive data isn't accessible to unauthorized users.
- Authorization: Role-based access control - administrator to maintain the api and tasks assosiated with it's creator.
