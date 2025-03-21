[![Run Unit Tests](https://github.com/ivanhoe-k/company-service/actions/workflows/backend_unit_tests_ci.yml/badge.svg)](https://github.com/ivanhoe-k/company-service/actions/workflows/backend_unit_tests_ci.yml)
[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

# 📘 Company Service

A .NET 9.0 backend project influenced by Hexagonal (Ports & Adapters) Architecture. 

## ✅ Highlights

- Modular architecture: API, Domain, Application, Persistence
- EF Core with SQL Server + Dockerized environment
- JWT-based Machine-to-Machine Authentication
- Serilog logging with Seq support
- Pagination, Filtering, and Validation included
- Ready-to-use Swagger UI
- CI-friendly Docker setup

---

## 📂 Project Structure

```
CompanyService/
├── Api/                # REST API Controllers
├── Application/        # Use cases and orchestration
├── Core/               # Shared configs, utilities, models
├── Domain/             # Business services, domain models, domain contracts
├── Persistence/        # EF Core models, DbContext, Repositories
├── Tests/              # Unit & Integration tests
├── docker-compose.yml  # Local dev setup
```

---

## 🔐 Auth

Simplified JWT token endpoint with in-memory credentials (no DB check).

**Test Clients:**

```csharp
private static readonly Dictionary<string, string> _clients = new()
{
    { "client-id-123", "client-secret-abc" },
    { "another-client", "another-secret" }
};
```

---

## 🚀 Run with Docker

```bash
docker-compose up --build -d
```

- API: `http://localhost:5193`
- Swagger: `http://localhost:5193/swagger`
- Seq logs: `http://localhost:5341`

---

## 🖪 Run Tests

```bash
dotnet test
```

---

## 📜 License

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

---

*This is a simplified demo for a coding assignment. Some areas like auth and error handling are intentionally kept minimal.*

