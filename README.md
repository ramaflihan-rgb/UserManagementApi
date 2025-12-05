# User Management API

A simple API for managing users with registration, login, role-based access, and user profile management.

---
# UserManagementApi

## How to Run the Project

1. Open your terminal or command prompt.
2. Navigate to the project folder:
   ```bash
   cd .\UserManagementApi\
 3. dotnet run 
-----

Endpoints

--AuthController--
POST /api/auth/register
    Body:
    {
      "fullName": "Rama Flihan",
      "email": "RamaFlihan@example.com",
      "password": "Rama1234",
      "confirmPassword": "Rama1234"
    }
    Response (200 OK):
    {
      "id": "user-guid",
      "fullName": "Rama Flihan",
      "email": "RamaFlihan@example.com",
      "passwordHash": "hashed-password",
      "roleId": "role-guid",
      "role": {
                "roleId": "role-guid"
                "roleName": "roleName"
      "createdAt": "2025-12-05T12:31:20.991983"
    }
    Response (400 BadRequest):
    {
       Text error
    }


POST /api/auth/login
    Body:
    {
      "email": "RamaFlihan@example.com",
      "password": "Rama1234"
    }
    Response (200 OK):
    {
        "token": "jwt-token-string"
    }
    Response (400 BadRequest):
    {
       Text error
    }
    

--UsersController--

GET /api/users
    - Get all users (Admin only)
    - Requires JWT token with Admin role

GET /api/users/{id}
    - Get user by ID
    - Requires JWT token

PUT /api/users/{id}
    Body:
    {
      "fullName": "Rama",
      "email": "Rama@example.com"
    }
    - Update user data
    - Requires JWT token

DELETE /api/users/{id}
    - Delete user (Admin only)
    - Requires JWT token with Admin role

PUT /api/users/{id}/role
    Body:
    - Change user's role (Admin only)
    - Requires JWT token with Admin role

PUT /api/users/me/password
    Body:
    {
      "oldPassword": "Old123",
      "newPassword": "New123"
    }
    - Change current user's password
    - Requires JWT token
