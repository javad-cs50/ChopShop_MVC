# 👕 ChopShop MVC

A modern **E-Commerce Clothing Store** built with **ASP.NET Core 9 MVC** using the **N-Layer Architecture** pattern.

The project focuses on building a scalable and maintainable backend structure similar to real-world enterprise applications.

---

## 🚀 Features

* 🔐 Authentication & Authorization
* 🛍 Product Management
* 🗂 Category Management
* 🛒 Shopping Cart System
* 📦 Order Management
* 👨‍💼 Admin Dashboard
* 👥 User & Role Management
* ✅ Form Validation
* 💾 SQL Server Integration
* 🧱 Clean Layered Architecture
* ⚡ Session-Based Cart Handling

---

## 🛠 Technologies Used

| Technology            | Description                    |
| --------------------- | ------------------------------ |
| ASP.NET Core 9 MVC    | Web Framework                  |
| C#                    | Backend Language               |
| Entity Framework Core | ORM                            |
| SQL Server            | Database                       |
| ASP.NET Identity      | Authentication & Authorization |
| Bootstrap             | UI Framework                   |
| LINQ                  | Data Querying                  |
| Git & GitHub          | Version Control                |

---

## 🏗 Architecture

This project follows the **N-Layer Architecture** pattern to improve:

* Maintainability
* Scalability
* Separation of Concerns
* Testability

### 📂 Layers

```text id="jlwmnm"
ChopShop/
│
├── ChopShop.Web          → Presentation Layer
├── ChopShop.Models       → Domain Models & ViewModels
├── ChopShop.DataAccess   → Database & Repository Layer
├── ChopShop.Utility      → Helpers & Utility Classes
└── ChopShop.sln
```

---

## 🧩 Design Patterns & Concepts

* Repository Pattern
* Dependency Injection
* MVC Pattern
* N-Layer Architecture
* Entity Framework Core
* Session Management
* Authentication & Authorization

---

## ⚙️ Getting Started

### Prerequisites

Before running the project, install:

* .NET 9 SDK
* SQL Server
* Visual Studio 2022 / Rider / VS Code

---

## 📥 Installation

Clone the repository:

```bash id="n63f8g"
git clone https://github.com/javad-cs50/ChopShop_MVC.git
```

Navigate to the project directory:

```bash id="4c6l1p"
cd ChopShop_MVC
```

Restore dependencies:

```bash id="pp0r7x"
dotnet restore
```

Configure the database connection string inside:

```text id="wz6k4h"
ChopShop.Web/appsettings.json
```

Apply migrations:

```bash id="0vg8hq"
dotnet ef database update
```

Run the application:

```bash id="bopttq"
dotnet run
```

---

## 👨‍💼 Admin Panel

The application includes a dedicated admin area for managing:

* Products
* Categories
* Orders
* Users

---


## 🔮 Future Improvements

* 💳 Online Payment Integration
* ⭐ Product Reviews & Ratings
* ❤️ Wishlist System
* 🌐 REST API Version
* 🐳 Docker Support
* 🚀 Redis Caching
* 📊 OpenTelemetry Monitoring

---

## 📚 Learning Goals

This project was built to improve skills in:

* ASP.NET Core MVC
* N-Layer Architecture
* Repository Pattern
* Backend Development
* Entity Framework Core
* Authentication & Authorization
* Real-world E-Commerce Systems

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome.

Feel free to fork the project and submit a pull request.

---

## 📄 License

This project is licensed under the MIT License.

---

## 👨‍💻 Author

**mr.Abbaspour**

GitHub:
https://github.com/javad-cs50
