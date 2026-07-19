# Warehouse Management System

A simple Warehouse Management System built with **ASP.NET Core MVC** as part of my Software Developer training.

The project demonstrates the complete development cycle of a database-driven MVC application, including CRUD operations, Entity Framework Core, authentication, file uploads, and responsive UI development.

---

## Features

- User authentication (Session-based)
- Password hashing
- Supplier management (CRUD)
- Product management (CRUD)
- Product image upload
- Stock In / Stock Out operations
- Transaction history
- Dashboard with warehouse statistics
- Product search
- Product sorting
- Product pagination
- Product filtering
- Server-side validation
- Client-side validation
- Responsive Bootstrap interface

---

## Tech Stack

- ASP.NET Core MVC (.NET 10)
- C#
- Entity Framework Core
- SQL Server
- Razor Views
- Bootstrap 5
- HTML
- CSS
- JavaScript
- Session Authentication

---

## Concepts Practiced

During this project I focused on understanding the concepts behind ASP.NET Core MVC rather than only implementing features.

Some of the concepts covered include:

- MVC Architecture
- Entity Framework Core
- Dependency Injection
- DbContext
- LINQ
- CRUD Operations
- Model Binding
- ViewModels
- Data Annotations
- Client-side Validation
- Server-side Validation
- Sessions
- Action Filters
- Password Hashing
- Authentication
- Navigation Properties
- Entity Relationships
- Migrations
- File Uploads
- Pagination
- Searching
- Sorting
- Razor Syntax
- Bootstrap Components
- Asynchronous Programming (`async` / `await`)

---

## Database

The application uses SQL Server with Entity Framework Core Code First.

Main entities:

- Users
- Suppliers
- Products
- Stock Transactions

Relationships:

- One Supplier → Many Products
- One Product → Many Stock Transactions

---

## Authentication

The application uses a simple custom authentication system designed for learning purposes.

- User accounts are stored in SQL Server.
- Passwords are stored as hashed values using ASP.NET Core's `PasswordHasher`.
- Session-based authentication is used to restrict access to protected pages.

---

## Project Structure

```
Controllers/
Data/
Filters/
Migrations/
Models/
ViewModels/
Views/
wwwroot/
```

---

## What I Learned

This project helped me understand:

- How an HTTP request flows through an ASP.NET Core MVC application.
- How Controllers, Models, Views, and ViewModels work together.
- How Entity Framework translates C# objects into SQL queries.
- How Dependency Injection is used throughout ASP.NET Core.
- The difference between client-side and server-side validation.
- How authentication and sessions work.
- How image uploads are stored on disk while only the filename is stored in the database.
- How migrations evolve the database schema over time.
- How to organize a medium-sized MVC project following clean and maintainable practices.

---

## Future Improvements

Some features that could be added in future versions:

- ASP.NET Core Identity
- Role-based authorization
- Registration page
- Password reset
- Product categories
- Dashboard charts
- Export to PDF / Excel
- Audit logging

---

## Purpose

This project was built as part of my Software Developer training to reinforce the fundamentals of ASP.NET Core MVC, Entity Framework Core, and SQL Server through hands-on development.
