[![Run Unit Tests](https://github.com/ivanhoe-k/company-service/actions/workflows/backend_unit_tests_ci.yml/badge.svg)](https://github.com/ivanhoe-k/company-service/actions/workflows/backend_unit_tests_ci.yml)
[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

# 📘 Company Service

A **.NET 9.0 backend project** influenced by **Hexagonal (Ports & Adapters) Architecture**.

## ✅ Highlights

- Modular structure: API, Domain, Application, Persistence
- Entity Framework Core + SQL Server (Dockerized)
- Machine-to-Machine JWT Authentication
- Logging with Serilog + Seq support
- Pagination, Filtering, and Validation
- Swagger UI for API exploration
- Docker- and CI-friendly setup

---

## 📂 Repository Layout

```
company-service/
├── backend/                # Entire backend solution
│   ├── CompanyService.Api/
│   ├── CompanyService.Application/
│   ├── CompanyService.Core/
│   ├── CompanyService.Domain/
│   ├── CompanyService.Persistence/
│   ├── CompanyService.Tests/
│   └── ...
├── frontend/               # (Not implemented yet)
├── docker-compose.yml      # Docker setup for API + SQL + Seq
```

---

## 🔐 Auth (Machine-to-Machine)

Simple JWT token endpoint with **in-memory client credentials** (no DB check).

### 🧪 Test Clients

```csharp
private static readonly Dictionary<string, string> _clients = new()
{
    { "client-id-123", "client-secret-abc" },
    { "another-client", "another-secret" }
};
```

---

## 🚀 Run Locally (Docker)

```bash
docker-compose up --build -d
```

- API: `http://localhost:5193`
- Swagger UI: `http://localhost:5193/swagger`
- Seq Logs: `http://localhost:5341`

---

## 🧪 Run Unit Tests

```bash
dotnet test
```

---

## 📜 License

Licensed under the [Apache 2.0 License](https://opensource.org/licenses/Apache-2.0)

---

⚠️ _This project was built as part of a coding assignment. It intentionally simplifies several aspects (like authentication, error handling, and client management) to focus on structure and domain-driven design._
