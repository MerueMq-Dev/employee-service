# Employee Service API

Web-сервис для управления сотрудниками, компаниями, отделами и паспортами.

## Технологии

- **.NET Core** — платформа
- **PostgreSQL** — база данных
- **Dapper** — ORM
- **MediatR** — CQRS pattern
- **FluentMigrator** — миграции БД

---

## Запуск проекта

## 🔹 Запуск проекта через Docker Compose (рекомендуется)

1. Убедитесь, что установлен **Docker** и **Docker Compose**.  

2. В корне проекта выполните команду:

```bash
docker compose up -d
```
Флаг -d запускает контейнеры в фоне.

Docker автоматически создаст базу данных PostgreSQL и запустит API.

Для остановки контейнеров:

```bash
docker compose down
```

## 🔹 Запуск проекта вручную

### Настройка БД

Обновите connection string в `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=EmployeeServiceDb;Username=postgres;Password=yourpassword"
  }
}
```

### 2. Применение миграций

```bash
dotnet run
```

Миграции применятся автоматически при старте.

### 3. Swagger UI

Откройте браузер: `https://localhost:5001/swagger`

---

## API Endpoints

### 🏢 Companies

| Метод | Endpoint | Описание |
|-------|----------|----------|
| GET | `/api/companies` | Список всех компаний |
| POST | `/api/companies` | Создать компанию |
| GET | `/api/companies/{companyId}` | Получить компанию по ID |
| PUT | `/api/companies/{companyId}` | Обновить компанию |
| DELETE | `/api/companies/{companyId}` | Удалить компанию |

**Пример запроса (POST):**
```json
{
  "name": "Tech Corp",
  "address": "123 Tech Street"
}
```

---

### 🏛️ Departments

| Метод | Endpoint | Описание |
|-------|----------|----------|
| GET | `/api/departments` | Список всех отделов |
| POST | `/api/departments` | Создать отдел |
| GET | `/api/departments/{departmentId}` | Получить отдел по ID |
| PUT | `/api/departments/{departmentId}` | Обновить отдел |
| DELETE | `/api/departments/{departmentId}` | Удалить отдел |

**Пример запроса (POST):**
```json
{
  "name": "IT Department",
  "phone": "+7-999-111-11-11",
  "companyId": 1
}
```

---

### 👤 Employees (Basic)

| Метод | Endpoint | Описание |
|-------|----------|----------|
| GET | `/api/employees` | Список всех сотрудников |
| POST | `/api/employees` | Создать сотрудника |
| GET | `/api/employees/{employeeId}` | Получить сотрудника по ID |
| PUT | `/api/employees/{employeeId}` | Обновить сотрудника |
| DELETE | `/api/employees/{employeeId}` | Удалить сотрудника |

---

### 👥 Detailed Employees (С полной информацией)

| Метод | Endpoint | Описание |
|-------|----------|----------|
| GET | `/api/detailed-employees` | Список всех сотрудников с деталями |
| POST | `/api/detailed-employees` | Создать сотрудника с деталями |
| GET | `/api/detailed-employees/{employeeId}` | Получить сотрудника с деталями |
| GET | `/api/detailed-employees/company/{companyId}` | Сотрудники компании |
| GET | `/api/detailed-employees/department/{departmentId}` | Сотрудники отдела |
| PATCH | `/api/detailed-employees/{employeeId}` | Частично обновить сотрудника |
| DELETE | `/api/detailed-employees/{employeeId}` | Удалить сотрудника |

**Пример запроса (POST):**
```json
{
  "name": "Ivan",
  "surname": "Petrov",
  "phone": "+7-999-555-55-55",
  "companyId": 1,
  "department": {
    "name": "IT Department",
    "phone": "+7-999-111-11-11"
  },
  "passport": {
    "type": "RU",
    "number": "1234567890"
  }
}
```

**Пример ответа:**
```json
{
  "id": 1,
  "name": "Ivan",
  "surname": "Petrov",
  "phone": "+7-999-555-55-55",
  "companyId": 1,
  "department": {
    "name": "IT Department",
    "phone": "+7-999-111-11-11"
  },
  "passport": {
    "type": "RU",
    "number": "1234567890"
  }
}
```

**Пример частичного обновления (PATCH):**
```json
{
  "name": "Alexander"
}
```

Обновляются только указанные поля.

---

### 🛂 Passports

| Метод | Endpoint | Описание |
|-------|----------|----------|
| GET | `/api/passports` | Список всех паспортов |
| POST | `/api/passports` | Создать паспорт |
| GET | `/api/passports/{passportId}` | Получить паспорт по ID |
| PUT | `/api/passports/{passportId}` | Обновить паспорт |
| DELETE | `/api/passports/{passportId}` | Удалить паспорт |

**Пример запроса (POST):**
```json
{
  "type": "RU",
  "number": "1234567890"
}
```

---

## Архитектура

### Структура проекта

```
EmployeeService/
├── Controllers/           # API endpoints
├── Application/
│   ├── Commands/         # MediatR commands
│   ├── Handlers/         # Command handlers
│   └── DTOs/             # Data Transfer Objects
├── Domain/
│   └── Entities/         # Domain models
├── Infrastructure/
│   ├── Repositories/     # Data access (Dapper)
│   └── Migrations/       # FluentMigrator
└── Common/
    └── Responses/        # API response wrappers
```

### Слои

- **Controllers** — обрабатывают HTTP запросы, маршрутизация
- **Commands** — команды (CQRS pattern)
- **Handlers** — бизнес-логика, валидация
- **Repositories** — доступ к данным (Dapper + PostgreSQL)
- **Entities** — доменные модели

---

## Бизнес-правила

### Связи между сущностями

```
Company (1) ──→ (*) Department (1) ──→ (*) Employee (1) ──→ (1) Passport
```

- Один **Company** может иметь несколько **Departments**
- Один **Department** принадлежит одной **Company**
- Один **Employee** принадлежит одному **Department**
- Один **Employee** имеет один **Passport** (1:1)

### Валидация

- При создании/обновлении **Employee** проверяется существование **Company**, **Department** и **Passport**
- **Passport** может быть привязан только к одному **Employee**
- При изменении **Department** у **Employee** автоматически обновляется **CompanyId**

---

## Особенности реализации

### Partial Updates (PATCH)

Метод `PATCH /api/detailed-employees/{id}` обновляет только переданные поля:

```json
{
  "name": "NewName"
}
```

Остальные поля остаются без изменений.

### Detailed Employees vs Employees

- **Employees** — базовый CRUD (только IDs)
- **Detailed Employees** — расширенный CRUD с вложенными объектами

Используйте **Detailed Employees** для работы согласно требованиям тестового задания.

---

## Примеры использования

### Создать компанию и сотрудника

```bash
# 1. Создать компанию
POST /api/companies
{
  "name": "Tech Corp",
  "address": "123 Street"
}
# Ответ: { "id": 1 }

# 2. Создать отдел
POST /api/departments
{
  "name": "IT",
  "phone": "+7-999-111-11-11",
  "companyId": 1
}
# Ответ: { "id": 1 }

# 3. Создать паспорт
POST /api/passports
{
  "type": "RU",
  "number": "1234567890"
}
# Ответ: { "id": 1 }

# 4. Создать сотрудника
POST /api/detailed-employees
{
  "name": "Ivan",
  "surname": "Petrov",
  "phone": "+7-999-555-55-55",
  "companyId": 1,
  "department": { "name": "IT" },
  "passport": { "number": "1234567890" }
}
# Ответ: полная информация о сотруднике
```

### Получить сотрудников компании

```bash
GET /api/detailed-employees/company/1
```

### Обновить отдел сотрудника

```bash
PATCH /api/detailed-employees/1
{
  "department": { "name": "Marketing" }
}
```

---

## Обработка ошибок

API возвращает стандартные HTTP статус-коды:

| Код | Описание |
|-----|----------|
| 200 | OK — успешно |
| 201 | Created — создано |
| 204 | No Content — удалено |
| 400 | Bad Request — некорректные данные |
| 404 | Not Found — объект не найден |
| 500 | Internal Server Error — ошибка сервера |

**Пример ошибки:**
```json
{
  "error": "Department with name 'NonExistent' does not exist"
}
```

------