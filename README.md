# Orders API - Documentation

## Overview

This API allows :
- Managing orders (Add, Get) with minimum bin width calculation.
- Managing product configuration (Add, Get).

**Response Format:** JSON

##  Technology Stack

| Layer | Technology |
|-------|-----------|
| **Controllers** | ASP.NET Core MVC |
| **Validation** | FluentValidation |
| **Services** | C# Business Logic |
| **Caching** | IMemoryCache (In-Memory) |
| **Mapping** | Custom Mappers |
| **Repositories** | Repository Pattern |
| **ORM** | Entity Framework Core |
| **Database** | SQLite |
| **DI Container** | ASP.NET Core DI |

##  Design Patterns Used

1. **Repository Pattern**
2. **Dependency Injection**
3. **Service Layer Pattern**
4. **DTO Pattern**
5. **Factory Pattern**
6. **Strategy Pattern**
7. **Singleton Pattern**

## Clone / Run / Test

#### Clone repository
```bash
git clone https://github.com/mahrezTaharboucht/Test-DotNet.git
```
#### Run API

```bash
cd Test-DotNet/OrdersApi
dotnet restore
dotnet run
```

#### Run unit tests

```bash
cd Test-DotNet
dotnet test --filter "Category=Unit"
```

#### Run integration tests

```bash
cd Test-DotNet
dotnet test --filter "Category=Integration"
```

#### Settings (appsettings.json)

| Variable | Description | Default Value |
|----------|-------------|---------------|
| `SqlLiteDbConnection` | SQLite connection string | `Data Source=app.db` |

***By default, the database file will be created in the API's current directory. Update the connection string to change the file location.***

---

## 📡 Endpoints

### Orders

#### Create Order

**Endpoint:** `PUT /order/{orderId}`

**HTTP Method:** `PUT`

**URL Parameters:**

| Parameter | Type | Required | Constraints |
|-----------|------|----------|-------------|
| `orderId` | integer | ✅ | Unique, greater than 0 |

**Body (JSON):**

```json
{
  "items": [
    {
      "productType": "Mug",
      "quantity": 2
    },
    {
      "productType": "Calendar",
      "quantity": 1
    }
  ]
}
```
| Property | Type | Required | Constraints |
|-----------|------|----------|-------------|
| `productType` | string | ✅ | Exist in database  |
| `quantity` | interger | ✅ | Greater than 0  |

**Success Response (201 Created):**

```json
{
  "success": true,
  "message": "",
  "data": {    
    "requiredBinWidth": 40
  },
  "errors": null
}
```

**Error Responses:**

| Code | Error |
|------|---------|
| 400 | `The order Id should be greater than 0.` |
| 400 | `Order items should contain at least one element.` |
| 400 | `Item quantity should be greater than 0.` |
| 400 | `Unknown product type.` |
| 409 | `The order already exist.` |
| 500 | `Internal service error.` |

---

#### Get Order

Retrieves details of an existing order.

**Endpoint:** `GET /order/{orderId}`

**HTTP Method:** `GET`

**URL Parameters:**

| Parameter | Type | Required | Constraints |
|-----------|------|----------|-------------|
| `orderId` | integer | ✅ |  Exist in database|

**Success Response (200 OK):**

```json
{
  "success": true,
  "message": "",
  "data": {
    "requiredBinWidth": 40,
    "items": [
      {
        "quantity": 2,
        "productName": "Mug"       
      },
      {        
        "quantity": 1,
        "productName": "Cards"       
      }
    ]
  },
  "errors": null
}
```

**Error Responses:**

| Code | Error |
|------|-------------|
| 404 | `The order {0} was not found.` |
| 500 | `Internal service error.` |

---

### Product Configurations

#### List Configurations

Retrieves all available product configurations.

**Endpoint:** `GET /ProductConfiguration`

**HTTP Method:** `GET`

**Success Response (200 OK):**

```json
{
  "success": true,
  "message": "",
  "data": [
    {
      "id": 1,
      "productType": "photoBook",
      "width": 19,
      "numberOfItemsInStack": 1
    },
    {
      "id": 2,
      "productType": "calendar",
      "width": 10,
      "numberOfItemsInStack": 1
    }
  ],
  "errors": null
}
```
---

#### Create Configuration

Creates a new product configuration.

**Endpoint:** `POST /ProductConfiguration`

**HTTP Method:** `POST`

**Body (JSON):**

```json
{
  "productType": "NewProduct",
  "width": 10,
  "numberOfItemsInStack": 2
}
```

| Property | Type | Required | Constraints |
|-----------|------|----------|-------------|
| `productType` | string | ✅ | Not Empty, Not n database  |
| `width` | decimal | ✅ | Greater than 0  |
| `numberOfItemsInStack` | integer | ✅ | Greater than 0  |

**Success Response (201 Created):**

```json
{
  "success": true,
  "message": "",
  "data": {
    "id": 7,
    "productType": "NewProduct",
    "width": 10,
    "numberOfItemsInStack": 2
  },
  "errors": null
}
```

**Error Responses:**

| Code | Description |
|------|-------------|
| 400 | `Product type should be provided.` |
| 400 | `Width should be greater than 0.` |
| 400 | `The number of items in stack should be greater than 0.` |
| 409 | `The given product type value already exists.` |
| 500 | `Internal service error.` |

---

## 📝 Notes

### Database initialization
At startup, the database is initialized with the following product configurations.

| Product type | Number Of Items In Stack | Width (mm) |
|--------------|--------------------------|-------|
| photoBook | 1 | 19 |
| calendar | 1 | 10 |
| canvas | 1 | 16 |
| cards | 1 | 4.7 |
| mug | 4 | 94 |

### Cache
Product configurations are cached for 2 hours to improve performance. The cache is automatically invalidated when creating a new configuration.

