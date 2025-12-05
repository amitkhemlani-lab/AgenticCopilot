# API Documentation - Engagement Controller

## Table of Contents
1. [Overview](#overview)
2. [Base URL](#base-url)
3. [Authentication](#authentication)
4. [API Endpoints](#api-endpoints)
5. [Request/Response Examples](#requestresponse-examples)
6. [Error Handling](#error-handling)
7. [Rate Limiting](#rate-limiting)
8. [ApplyTo Guidelines](#applyto-guidelines)

## Overview

The Engagement API provides a RESTful interface for managing engagement records. Each engagement contains information about the engagement name, associated client, and partner.

### API Version
- **Version**: 1.0
- **Protocol**: HTTP/HTTPS
- **Data Format**: JSON
- **Character Encoding**: UTF-8

### Resource Model

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Digital Transformation Project",
  "clientName": "Acme Corporation",
  "partnerName": "Tech Solutions Inc",
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": "2024-01-15T10:30:00Z"
}
```

## Base URL

### Development
```
http://localhost:5000/api
```

### Staging
```
https://staging-api.yourdomain.com/api
```

### Production
```
https://api.yourdomain.com/api
```

## Authentication

### Current Implementation
⚠️ **No authentication required** (Development only)

### Production Implementation (ApplyTo)

#### JWT Bearer Token

**Required Header**:
```http
Authorization: Bearer <your_jwt_token>
```

**Obtaining a Token**:
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "user@example.com",
  "password": "your_password"
}
```

**Response**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "tokenType": "Bearer"
}
```

**Token Expiration**: 1 hour (3600 seconds)

## API Endpoints

### Summary Table

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/engagement` | Get all engagements | Yes (Production) |
| GET | `/api/engagement/{id}` | Get engagement by ID | Yes (Production) |
| POST | `/api/engagement` | Create new engagement | Yes (Production) |
| PUT | `/api/engagement/{id}` | Update engagement | Yes (Production) |
| DELETE | `/api/engagement/{id}` | Delete engagement | Yes (Production) |
| HEAD | `/api/engagement/{id}` | Check if engagement exists | Yes (Production) |

---

## API Endpoints - Detailed

### 1. Get All Engagements

Retrieves a list of all engagements.

**Endpoint**: `GET /api/engagement`

**Request**:
```http
GET /api/engagement HTTP/1.1
Host: api.yourdomain.com
Authorization: Bearer <token>
Accept: application/json
```

**Success Response** (200 OK):
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Digital Transformation",
    "clientName": "Acme Corp",
    "partnerName": "Tech Solutions",
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": "2024-01-15T10:30:00Z"
  },
  {
    "id": "7bc95a82-6821-4973-a5dc-3d973f66bfa7",
    "name": "Cloud Migration",
    "clientName": "Global Industries",
    "partnerName": "Cloud Experts",
    "createdAt": "2024-01-16T14:20:00Z",
    "updatedAt": "2024-01-16T14:20:00Z"
  }
]
```

**Error Responses**:

```json
// 401 Unauthorized
{
  "error": "Unauthorized",
  "message": "Authentication required"
}

// 500 Internal Server Error
{
  "error": "Internal Server Error",
  "message": "An error occurred while retrieving engagements"
}
```

**ApplyTo Enhancements**:
- [ ] Add pagination (`?pageNumber=1&pageSize=20`)
- [ ] Add filtering (`?clientName=Acme`)
- [ ] Add sorting (`?sortBy=name&sortOrder=asc`)
- [ ] Add field selection (`?fields=id,name`)

---

### 2. Get Engagement by ID

Retrieves a specific engagement by its unique identifier.

**Endpoint**: `GET /api/engagement/{id}`

**URL Parameters**:
- `id` (required): GUID - Unique identifier of the engagement

**Request**:
```http
GET /api/engagement/3fa85f64-5717-4562-b3fc-2c963f66afa6 HTTP/1.1
Host: api.yourdomain.com
Authorization: Bearer <token>
Accept: application/json
```

**Success Response** (200 OK):
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Digital Transformation",
  "clientName": "Acme Corp",
  "partnerName": "Tech Solutions",
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": "2024-01-15T10:30:00Z"
}
```

**Error Responses**:

```json
// 404 Not Found
{
  "error": "Not Found",
  "message": "Engagement with ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 not found"
}

// 401 Unauthorized
{
  "error": "Unauthorized",
  "message": "Authentication required"
}

// 500 Internal Server Error
{
  "error": "Internal Server Error",
  "message": "An error occurred while retrieving the engagement"
}
```

**cURL Example**:
```bash
curl -X GET \
  https://api.yourdomain.com/api/engagement/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H 'Authorization: Bearer <token>' \
  -H 'Accept: application/json'
```

---

### 3. Create Engagement

Creates a new engagement.

**Endpoint**: `POST /api/engagement`

**Request Headers**:
```
Content-Type: application/json
Authorization: Bearer <token>
```

**Request Body**:
```json
{
  "name": "Digital Transformation Project",
  "clientName": "Acme Corporation",
  "partnerName": "Tech Solutions Inc"
}
```

**Request Body Schema**:
| Field | Type | Required | Min Length | Max Length | Description |
|-------|------|----------|------------|------------|-------------|
| name | string | Yes | 3 | 200 | Name of the engagement |
| clientName | string | Yes | 2 | 200 | Client name |
| partnerName | string | Yes | 2 | 200 | Partner name |

**Full Request**:
```http
POST /api/engagement HTTP/1.1
Host: api.yourdomain.com
Authorization: Bearer <token>
Content-Type: application/json
Accept: application/json

{
  "name": "Digital Transformation Project",
  "clientName": "Acme Corporation",
  "partnerName": "Tech Solutions Inc"
}
```

**Success Response** (201 Created):
```json
{
  "id": "8da95f64-6828-5673-c4fd-3d974g77cga8",
  "name": "Digital Transformation Project",
  "clientName": "Acme Corporation",
  "partnerName": "Tech Solutions Inc",
  "createdAt": "2024-01-17T09:15:00Z",
  "updatedAt": "2024-01-17T09:15:00Z"
}
```

**Response Headers**:
```
Location: /api/engagement/8da95f64-6828-5673-c4fd-3d974g77cga8
```

**Error Responses**:

```json
// 400 Bad Request - Validation Error
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Name": [
      "Name is required"
    ],
    "ClientName": [
      "ClientName must be between 2 and 200 characters"
    ]
  }
}

// 401 Unauthorized
{
  "error": "Unauthorized",
  "message": "Authentication required"
}

// 403 Forbidden (if user lacks permission)
{
  "error": "Forbidden",
  "message": "You don't have permission to create engagements"
}

// 500 Internal Server Error
{
  "error": "Internal Server Error",
  "message": "An error occurred while creating the engagement"
}
```

**cURL Example**:
```bash
curl -X POST \
  https://api.yourdomain.com/api/engagement \
  -H 'Authorization: Bearer <token>' \
  -H 'Content-Type: application/json' \
  -d '{
    "name": "Digital Transformation Project",
    "clientName": "Acme Corporation",
    "partnerName": "Tech Solutions Inc"
  }'
```

**JavaScript Example**:
```javascript
fetch('https://api.yourdomain.com/api/engagement', {
  method: 'POST',
  headers: {
    'Authorization': 'Bearer <token>',
    'Content-Type': 'application/json'
  },
  body: JSON.stringify({
    name: 'Digital Transformation Project',
    clientName: 'Acme Corporation',
    partnerName: 'Tech Solutions Inc'
  })
})
.then(response => response.json())
.then(data => console.log(data));
```

---

### 4. Update Engagement

Updates an existing engagement.

**Endpoint**: `PUT /api/engagement/{id}`

**URL Parameters**:
- `id` (required): GUID - Unique identifier of the engagement to update

**Request Headers**:
```
Content-Type: application/json
Authorization: Bearer <token>
```

**Request Body**:
```json
{
  "name": "Updated Digital Transformation Project",
  "clientName": "Acme Corporation Inc",
  "partnerName": "Tech Solutions International"
}
```

**Request Body Schema**:
| Field | Type | Required | Min Length | Max Length | Description |
|-------|------|----------|------------|------------|-------------|
| name | string | Yes | 3 | 200 | Updated name of the engagement |
| clientName | string | Yes | 2 | 200 | Updated client name |
| partnerName | string | Yes | 2 | 200 | Updated partner name |

**Full Request**:
```http
PUT /api/engagement/3fa85f64-5717-4562-b3fc-2c963f66afa6 HTTP/1.1
Host: api.yourdomain.com
Authorization: Bearer <token>
Content-Type: application/json
Accept: application/json

{
  "name": "Updated Digital Transformation Project",
  "clientName": "Acme Corporation Inc",
  "partnerName": "Tech Solutions International"
}
```

**Success Response** (200 OK):
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Updated Digital Transformation Project",
  "clientName": "Acme Corporation Inc",
  "partnerName": "Tech Solutions International",
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": "2024-01-17T11:45:00Z"
}
```

**Error Responses**:

```json
// 400 Bad Request - Validation Error
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Name": [
      "Name must be between 3 and 200 characters"
    ]
  }
}

// 404 Not Found
{
  "error": "Not Found",
  "message": "Engagement with ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 not found"
}

// 401 Unauthorized
{
  "error": "Unauthorized",
  "message": "Authentication required"
}

// 403 Forbidden
{
  "error": "Forbidden",
  "message": "You don't have permission to update this engagement"
}

// 500 Internal Server Error
{
  "error": "Internal Server Error",
  "message": "An error occurred while updating the engagement"
}
```

**cURL Example**:
```bash
curl -X PUT \
  https://api.yourdomain.com/api/engagement/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H 'Authorization: Bearer <token>' \
  -H 'Content-Type: application/json' \
  -d '{
    "name": "Updated Digital Transformation Project",
    "clientName": "Acme Corporation Inc",
    "partnerName": "Tech Solutions International"
  }'
```

**ApplyTo Enhancements**:
- [ ] Add support for partial updates (PATCH)
- [ ] Add optimistic concurrency control (ETags)
- [ ] Add version history tracking

---

### 5. Delete Engagement

Deletes an engagement permanently.

**Endpoint**: `DELETE /api/engagement/{id}`

**URL Parameters**:
- `id` (required): GUID - Unique identifier of the engagement to delete

**Request**:
```http
DELETE /api/engagement/3fa85f64-5717-4562-b3fc-2c963f66afa6 HTTP/1.1
Host: api.yourdomain.com
Authorization: Bearer <token>
```

**Success Response** (204 No Content):
```
No body returned
```

**Error Responses**:

```json
// 404 Not Found
{
  "error": "Not Found",
  "message": "Engagement with ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 not found"
}

// 401 Unauthorized
{
  "error": "Unauthorized",
  "message": "Authentication required"
}

// 403 Forbidden
{
  "error": "Forbidden",
  "message": "You don't have permission to delete engagements"
}

// 500 Internal Server Error
{
  "error": "Internal Server Error",
  "message": "An error occurred while deleting the engagement"
}
```

**cURL Example**:
```bash
curl -X DELETE \
  https://api.yourdomain.com/api/engagement/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H 'Authorization: Bearer <token>'
```

**ApplyTo Enhancements**:
- [ ] Implement soft delete instead of hard delete
- [ ] Add confirmation requirement for deletion
- [ ] Add cascade delete rules for related entities

---

### 6. Check Engagement Exists

Checks if an engagement exists without retrieving the full resource.

**Endpoint**: `HEAD /api/engagement/{id}`

**URL Parameters**:
- `id` (required): GUID - Unique identifier of the engagement

**Request**:
```http
HEAD /api/engagement/3fa85f64-5717-4562-b3fc-2c963f66afa6 HTTP/1.1
Host: api.yourdomain.com
Authorization: Bearer <token>
```

**Success Response** (200 OK):
```
No body returned
```

**Error Response** (404 Not Found):
```
No body returned
```

**cURL Example**:
```bash
curl -I \
  https://api.yourdomain.com/api/engagement/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H 'Authorization: Bearer <token>'
```

**Use Cases**:
- Check existence before attempting update
- Validate IDs without transferring data
- Monitor resource availability

---

## Request/Response Examples

### Complete Workflow Example

```bash
# 1. Create a new engagement
POST_RESPONSE=$(curl -X POST \
  https://api.yourdomain.com/api/engagement \
  -H 'Authorization: Bearer <token>' \
  -H 'Content-Type: application/json' \
  -d '{
    "name": "AI Implementation",
    "clientName": "Future Tech Corp",
    "partnerName": "ML Experts Ltd"
  }')

# Extract the ID from response
ENGAGEMENT_ID=$(echo $POST_RESPONSE | jq -r '.id')

# 2. Retrieve the engagement
curl -X GET \
  https://api.yourdomain.com/api/engagement/$ENGAGEMENT_ID \
  -H 'Authorization: Bearer <token>'

# 3. Update the engagement
curl -X PUT \
  https://api.yourdomain.com/api/engagement/$ENGAGEMENT_ID \
  -H 'Authorization: Bearer <token>' \
  -H 'Content-Type: application/json' \
  -d '{
    "name": "Advanced AI Implementation",
    "clientName": "Future Tech Corporation",
    "partnerName": "ML Experts International"
  }'

# 4. Check if engagement exists
curl -I \
  https://api.yourdomain.com/api/engagement/$ENGAGEMENT_ID \
  -H 'Authorization: Bearer <token>'

# 5. Delete the engagement
curl -X DELETE \
  https://api.yourdomain.com/api/engagement/$ENGAGEMENT_ID \
  -H 'Authorization: Bearer <token>'
```

### C# Client Example

```csharp
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class EngagementApiClient
{
    private readonly HttpClient _httpClient;

    public EngagementApiClient(string baseUrl, string token)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }

    public async Task<Engagement> CreateEngagementAsync(CreateEngagementDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/engagement", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Engagement>();
    }

    public async Task<Engagement> GetEngagementAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<Engagement>($"/api/engagement/{id}");
    }

    public async Task<Engagement> UpdateEngagementAsync(Guid id, UpdateEngagementDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/engagement/{id}", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Engagement>();
    }

    public async Task<bool> DeleteEngagementAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"/api/engagement/{id}");
        return response.IsSuccessStatusCode;
    }
}
```

### Python Client Example

```python
import requests
import json

class EngagementApiClient:
    def __init__(self, base_url, token):
        self.base_url = base_url
        self.headers = {
            'Authorization': f'Bearer {token}',
            'Content-Type': 'application/json'
        }

    def create_engagement(self, name, client_name, partner_name):
        payload = {
            'name': name,
            'clientName': client_name,
            'partnerName': partner_name
        }
        response = requests.post(
            f'{self.base_url}/api/engagement',
            headers=self.headers,
            json=payload
        )
        response.raise_for_status()
        return response.json()

    def get_engagement(self, engagement_id):
        response = requests.get(
            f'{self.base_url}/api/engagement/{engagement_id}',
            headers=self.headers
        )
        response.raise_for_status()
        return response.json()

    def update_engagement(self, engagement_id, name, client_name, partner_name):
        payload = {
            'name': name,
            'clientName': client_name,
            'partnerName': partner_name
        }
        response = requests.put(
            f'{self.base_url}/api/engagement/{engagement_id}',
            headers=self.headers,
            json=payload
        )
        response.raise_for_status()
        return response.json()

    def delete_engagement(self, engagement_id):
        response = requests.delete(
            f'{self.base_url}/api/engagement/{engagement_id}',
            headers=self.headers
        )
        response.raise_for_status()
        return response.status_code == 204
```

---

## Error Handling

### Standard Error Response Format

All error responses follow this structure:

```json
{
  "error": "ErrorType",
  "message": "Human-readable error message",
  "details": {
    "field": "Additional context if applicable"
  },
  "timestamp": "2024-01-17T10:30:00Z",
  "path": "/api/engagement/invalid-id"
}
```

### HTTP Status Codes

| Code | Status | Description |
|------|--------|-------------|
| 200 | OK | Request succeeded |
| 201 | Created | Resource created successfully |
| 204 | No Content | Resource deleted successfully |
| 400 | Bad Request | Invalid request data or validation error |
| 401 | Unauthorized | Authentication required or failed |
| 403 | Forbidden | Authenticated but lacking permissions |
| 404 | Not Found | Resource not found |
| 409 | Conflict | Resource conflict (e.g., duplicate) |
| 429 | Too Many Requests | Rate limit exceeded |
| 500 | Internal Server Error | Server error occurred |
| 503 | Service Unavailable | Service temporarily unavailable |

### Validation Errors

Validation errors return detailed information about each field:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Name": [
      "Name is required",
      "Name must be between 3 and 200 characters"
    ],
    "ClientName": [
      "ClientName is required"
    ]
  },
  "traceId": "00-1a2b3c4d5e6f7g8h9i0j-1k2l3m4n-01"
}
```

---

## Rate Limiting

### Current Implementation
⚠️ **No rate limiting** (Development only)

### Production Implementation (ApplyTo)

**Rate Limits**:
- **General**: 100 requests per minute per IP
- **Authentication**: 5 login attempts per minute per IP
- **POST/PUT/DELETE**: 20 requests per minute per user

**Rate Limit Headers**:
```http
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 87
X-RateLimit-Reset: 1642420800
```

**Rate Limit Exceeded Response** (429):
```json
{
  "error": "Too Many Requests",
  "message": "Rate limit exceeded. Please try again later.",
  "retryAfter": 45
}
```

---

## ApplyTo Guidelines

### Pre-Production Checklist

#### API Versioning
- [ ] Implement API versioning (URL or header-based)
  ```csharp
  [ApiVersion("1.0")]
  [Route("api/v{version:apiVersion}/engagement")]
  ```
- [ ] Document versioning strategy
- [ ] Plan deprecation timeline for old versions

#### Pagination
- [ ] Implement pagination for GET all endpoint
  ```
  GET /api/engagement?pageNumber=1&pageSize=20
  ```
- [ ] Add pagination metadata in response
  ```json
  {
    "items": [...],
    "pageNumber": 1,
    "pageSize": 20,
    "totalCount": 150,
    "totalPages": 8
  }
  ```

#### Filtering and Sorting
- [ ] Add filtering capabilities
  ```
  GET /api/engagement?clientName=Acme&partnerName=Tech
  ```
- [ ] Add sorting
  ```
  GET /api/engagement?sortBy=createdAt&sortOrder=desc
  ```

#### Search
- [ ] Implement search functionality
  ```
  GET /api/engagement/search?q=digital
  ```

#### Batch Operations
- [ ] Add bulk create endpoint
- [ ] Add bulk update endpoint
- [ ] Add bulk delete endpoint

#### Caching
- [ ] Implement ETag support
- [ ] Add Cache-Control headers
- [ ] Configure client-side caching

#### Documentation
- [ ] Generate OpenAPI/Swagger documentation
- [ ] Create Postman collection
- [ ] Provide interactive API explorer
- [ ] Document all error scenarios

#### Security
- [ ] Implement authentication (JWT/OAuth2)
- [ ] Implement authorization (RBAC)
- [ ] Add rate limiting
- [ ] Configure CORS properly
- [ ] Enable HTTPS only
- [ ] Implement request validation
- [ ] Add input sanitization

#### Monitoring
- [ ] Add request logging
- [ ] Implement performance tracking
- [ ] Set up error alerting
- [ ] Create API analytics dashboard

#### Testing
- [ ] API integration tests
- [ ] Load testing
- [ ] Security testing (OWASP)
- [ ] Contract testing

### ApplyTo Different Environments

#### Development Environment
```json
{
  "baseUrl": "http://localhost:5000",
  "authentication": false,
  "rateLimiting": false,
  "logging": "verbose",
  "cors": "allow-all"
}
```

#### Staging Environment
```json
{
  "baseUrl": "https://staging-api.yourdomain.com",
  "authentication": true,
  "rateLimiting": true,
  "logging": "detailed",
  "cors": "restricted"
}
```

#### Production Environment
```json
{
  "baseUrl": "https://api.yourdomain.com",
  "authentication": true,
  "rateLimiting": true,
  "logging": "errors-only",
  "cors": "strict-whitelist",
  "https": "enforced",
  "monitoring": "enabled"
}
```

### Migration Checklist

#### Phase 1: Core API (Current)
- [x] CRUD operations
- [x] Input validation
- [x] Error handling
- [x] Logging

#### Phase 2: Security & Performance
- [ ] Authentication implementation
- [ ] Authorization implementation
- [ ] Rate limiting
- [ ] Caching strategy
- [ ] Pagination

#### Phase 3: Advanced Features
- [ ] Search functionality
- [ ] Batch operations
- [ ] Webhooks
- [ ] Event notifications
- [ ] Audit trail

#### Phase 4: Scale & Optimize
- [ ] Database optimization
- [ ] Response compression
- [ ] CDN integration
- [ ] Auto-scaling
- [ ] Global distribution

## Conclusion

This API documentation provides a comprehensive guide for integrating with the Engagement Controller API. The current implementation covers all basic CRUD operations with proper validation and error handling.

### Quick Start Summary

**For Developers**:
1. Use base URL: `http://localhost:5000/api` (dev) or `https://api.yourdomain.com/api` (prod)
2. Set `Content-Type: application/json` header
3. Add `Authorization: Bearer <token>` header (production)
4. Follow request/response schemas documented above
5. Handle standard HTTP status codes

**For Production Deployment**:
1. Complete all items in ApplyTo checklist
2. Implement authentication and authorization
3. Add rate limiting and caching
4. Set up monitoring and alerting
5. Generate OpenAPI documentation
6. Conduct security and load testing

### Support & Resources

- **API Docs**: This document
- **Code Review**: See `CODE_REVIEW.md`
- **Architecture**: See `ARCHITECTURE.md`
- **Security**: See `SECURITY.md`
- **Performance**: See `PERFORMANCE.md`
- **Testing**: See `UNIT_TESTING.md`

**API Status**: ✅ Ready for development/testing, ⚠️ Requires enhancements for production
