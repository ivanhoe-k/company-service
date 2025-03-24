[![Unit Tests](https://github.com/ivanhoe-k/company-service/actions/workflows/backend_unit_tests_ci.yml/badge.svg)](https://github.com/ivanhoe-k/company-service/actions/workflows/backend_unit_tests_ci.yml)
[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

# 📘 Company Service

A full-stack demo built with **.NET 9.0** and **React + Vite**, following Hexagonal (Ports & Adapters) Architecture.

---

## ✅ Highlights

- Modular architecture: API, Domain, Application, Persistence
- EF Core + SQL Server (Dockerized)
- JWT-based Machine-to-Machine Auth
- Serilog logging + Seq integration
- React frontend with Axios + React Router
- Pagination, Filtering, Form Validation
- Docker + CI-friendly setup
- Swagger UI for API testing

---

## 📂 Project Structure

```
company-service/
├── backend/                   # .NET backend solution
│   ├── CompanyService.Api/
│   ├── CompanyService.Application/
│   ├── CompanyService.Core/
│   ├── CompanyService.Domain/
│   ├── CompanyService.Persistence/
│   ├── CompanyService.Tests/
│   └── ...
├── frontend/                  # React + Vite frontend
│   ├── features/
│   ├── api/
│   ├── hooks/
│   └── ...
├── docker-compose.yml         # Full-stack environment
```

---

## 🔐 Auth (Machine-to-Machine)

Basic JWT flow with in-memory client credentials.

**Test Clients:**

```csharp
private static readonly Dictionary<string, string> _clients = new()
{
    { "client-id-123", "client-secret-abc" },
    { "another-client", "another-secret" }
};
```

---

## 🚀 Run the App

```bash
docker-compose up --build -d
```

- Backend: `http://localhost:5193`
- Frontend (Vite): `http://localhost:5173`
- Swagger: `http://localhost:5193/swagger`
- Logs: `http://localhost:5341`

---

## 💻 Frontend Features

- Login with client ID / secret
- Company list (paginated)
- Create / Edit company forms
- Validation, conditional fields (ISIN only on create)
- Protected routes

> React + Vite + TypeScript + TailwindCSS

---

## 🧪 Run Backend Tests

```bash
dotnet test
```

---

## 📜 License

Licensed under the [Apache 2.0 License](https://opensource.org/licenses/Apache-2.0)

---

⚠️ _This project was built as part of a coding assignment. Some areas like authentication, client management, and validation were intentionally simplified._
