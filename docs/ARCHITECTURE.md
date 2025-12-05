# Architecture Documentation - Engagement Controller

## Table of Contents
1. [Overview](#overview)
2. [Architectural Patterns](#architectural-patterns)
3. [System Components](#system-components)
4. [Data Flow](#data-flow)
5. [Design Decisions](#design-decisions)
6. [Scalability Considerations](#scalability-considerations)
7. [ApplyTo Guidelines](#applyto-guidelines)

## Overview

The Engagement Controller implements a clean, layered architecture following ASP.NET Core best practices. The system manages engagement entities with associated client and partner information through a RESTful API.

### Architecture Style
- **Pattern**: Layered Architecture (N-Tier)
- **API Style**: RESTful
- **Communication**: Synchronous HTTP/HTTPS
- **Data Storage**: In-memory (prototype), designed for database integration

## Architectural Patterns

### 1. Layered Architecture

```
┌─────────────────────────────────────┐
│     Presentation Layer              │
│  (EngagementController)             │
│  - HTTP Request/Response Handling   │
│  - Input Validation                 │
│  - Error Response Formatting        │
└─────────────────┬───────────────────┘
                  │
┌─────────────────▼───────────────────┐
│     Business Logic Layer            │
│  (IEngagementService)               │
│  - Business Rules                   │
│  - Data Validation                  │
│  - Transaction Management           │
└─────────────────┬───────────────────┘
                  │
┌─────────────────▼───────────────────┐
│     Data Access Layer               │
│  (EngagementService - In-Memory)    │
│  - CRUD Operations                  │
│  - Data Persistence                 │
│  - Query Operations                 │
└─────────────────────────────────────┘
```

### 2. Dependency Injection Pattern

**Container Configuration** (Startup.cs example):
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IEngagementService, EngagementService>();
    services.AddControllers();
    services.AddLogging();
}
```

**Benefits:**
- Loose coupling between components
- Easy unit testing with mocks
- Flexibility to swap implementations
- Centralized configuration

### 3. Repository Pattern (Future Enhancement)

**Current**: Service directly manages data
**Recommended**: Add repository layer

```csharp
public interface IEngagementRepository
{
    Task<IEnumerable<Engagement>> GetAllAsync();
    Task<Engagement> GetByIdAsync(Guid id);
    Task<Engagement> AddAsync(Engagement engagement);
    Task<Engagement> UpdateAsync(Engagement engagement);
    Task<bool> DeleteAsync(Guid id);
}
```

### 4. DTO Pattern

```
Client Request → CreateEngagementDto → Service → Engagement Entity
Client Response ← Engagement Entity ← Service
```

**Purpose:**
- Prevent over-posting attacks
- Decouple API contracts from domain models
- Enable versioning without breaking changes

## System Components

### Component Diagram

```
┌───────────────────────────────────────────────────────────┐
│                        API Layer                          │
│  ┌─────────────────────────────────────────────────────┐  │
│  │         EngagementController                        │  │
│  │  - GetAllEngagements()                              │  │
│  │  - GetEngagementById(id)                            │  │
│  │  - CreateEngagement(dto)                            │  │
│  │  - UpdateEngagement(id, dto)                        │  │
│  │  - DeleteEngagement(id)                             │  │
│  └────────────────────┬────────────────────────────────┘  │
└───────────────────────┼────────────────────────────────────┘
                        │
                        │ Dependency
                        ▼
┌───────────────────────────────────────────────────────────┐
│                    Service Layer                          │
│  ┌─────────────────────────────────────────────────────┐  │
│  │         IEngagementService (Interface)              │  │
│  └─────────────────────┬───────────────────────────────┘  │
│                        │                                   │
│  ┌─────────────────────▼───────────────────────────────┐  │
│  │         EngagementService (Implementation)          │  │
│  │  - In-memory storage (List<Engagement>)             │  │
│  │  - Thread safety (lock mechanism)                   │  │
│  │  - Business logic                                   │  │
│  └─────────────────────────────────────────────────────┘  │
└───────────────────────────────────────────────────────────┘
                        │
                        │ Uses
                        ▼
┌───────────────────────────────────────────────────────────┐
│                    Domain Layer                           │
│  ┌─────────────────────────────────────────────────────┐  │
│  │         Engagement (Entity)                         │  │
│  │  - Id: Guid                                         │  │
│  │  - Name: string                                     │  │
│  │  - ClientName: string                               │  │
│  │  - PartnerName: string                              │  │
│  │  - CreatedAt: DateTime                              │  │
│  │  - UpdatedAt: DateTime                              │  │
│  └─────────────────────────────────────────────────────┘  │
│                                                           │
│  ┌─────────────────────────────────────────────────────┐  │
│  │         DTOs                                        │  │
│  │  - CreateEngagementDto                              │  │
│  │  - UpdateEngagementDto                              │  │
│  └─────────────────────────────────────────────────────┘  │
└───────────────────────────────────────────────────────────┘
```

### Cross-Cutting Concerns

```
┌─────────────────────────────────────────────────────────┐
│              Cross-Cutting Concerns                     │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │   Logging    │  │  Validation  │  │    Error     │  │
│  │ (ILogger<T>) │  │ (DataAnnot.) │  │   Handling   │  │
│  └──────────────┘  └──────────────┘  └──────────────┘  │
└─────────────────────────────────────────────────────────┘
```

## Data Flow

### Create Engagement Flow

```
1. HTTP POST Request
   │
   ├─→ [EngagementController.CreateEngagement]
   │   │
   │   ├─→ Validate ModelState
   │   │   ├─ Valid? Continue
   │   │   └─ Invalid? Return 400 BadRequest
   │   │
   │   ├─→ Call IEngagementService.CreateEngagementAsync(dto)
   │   │   │
   │   │   ├─→ [EngagementService]
   │   │   │   ├─→ Create new Engagement entity
   │   │   │   ├─→ Generate GUID
   │   │   │   ├─→ Set timestamps
   │   │   │   ├─→ Add to in-memory store (thread-safe)
   │   │   │   └─→ Return created entity
   │   │   │
   │   │   └─→ Return to controller
   │   │
   │   └─→ Return 201 Created with Location header
   │
   └─→ HTTP Response (201 Created)
```

### Read Engagement Flow

```
1. HTTP GET Request (/api/engagement/{id})
   │
   ├─→ [EngagementController.GetEngagementById]
   │   │
   │   ├─→ Call IEngagementService.GetEngagementByIdAsync(id)
   │   │   │
   │   │   ├─→ [EngagementService]
   │   │   │   ├─→ Query in-memory store
   │   │   │   └─→ Return entity or null
   │   │   │
   │   │   └─→ Return to controller
   │   │
   │   ├─→ Check if entity exists
   │   │   ├─ Exists? Return 200 OK with data
   │   │   └─ Not found? Return 404 NotFound
   │   │
   │   └─→ HTTP Response
   │
   └─→ HTTP Response (200 OK or 404 NotFound)
```

### Update Engagement Flow

```
1. HTTP PUT Request (/api/engagement/{id})
   │
   ├─→ [EngagementController.UpdateEngagement]
   │   │
   │   ├─→ Validate ModelState
   │   │
   │   ├─→ Call IEngagementService.UpdateEngagementAsync(id, dto)
   │   │   │
   │   │   ├─→ [EngagementService]
   │   │   │   ├─→ Find existing engagement
   │   │   │   ├─→ Update properties
   │   │   │   ├─→ Update timestamp
   │   │   │   └─→ Return updated entity or null
   │   │   │
   │   │   └─→ Return to controller
   │   │
   │   ├─→ Check result
   │   │   ├─ Success? Return 200 OK with updated data
   │   │   └─ Not found? Return 404 NotFound
   │   │
   │   └─→ HTTP Response
   │
   └─→ HTTP Response (200 OK or 404 NotFound)
```

### Delete Engagement Flow

```
1. HTTP DELETE Request (/api/engagement/{id})
   │
   ├─→ [EngagementController.DeleteEngagement]
   │   │
   │   ├─→ Call IEngagementService.DeleteEngagementAsync(id)
   │   │   │
   │   │   ├─→ [EngagementService]
   │   │   │   ├─→ Find engagement
   │   │   │   ├─→ Remove from store
   │   │   │   └─→ Return true/false
   │   │   │
   │   │   └─→ Return to controller
   │   │
   │   ├─→ Check result
   │   │   ├─ Success? Return 204 NoContent
   │   │   └─ Not found? Return 404 NotFound
   │   │
   │   └─→ HTTP Response
   │
   └─→ HTTP Response (204 NoContent or 404 NotFound)
```

## Design Decisions

### 1. Why In-Memory Storage?

**Decision**: Use `List<Engagement>` with lock-based synchronization

**Rationale**:
- Simplifies prototype and demonstration
- No external dependencies (database)
- Fast for testing and development
- Easy to understand and maintain

**Trade-offs**:
- ❌ Data lost on application restart
- ❌ Not suitable for production
- ❌ Limited scalability
- ✅ Fast development cycle
- ✅ Easy testing

**Migration Path**: Replace with Entity Framework Core + SQL Server/PostgreSQL

### 2. Why Async/Await Throughout?

**Decision**: All service methods return `Task<T>`

**Rationale**:
- Non-blocking I/O operations
- Better scalability under load
- Consistent with ASP.NET Core best practices
- Prepares for database integration

**Note**: Current in-memory implementation uses `Task.FromResult()` but structure supports true async operations.

### 3. Why Separate DTOs?

**Decision**: `CreateEngagementDto` and `UpdateEngagementDto` instead of using `Engagement` directly

**Rationale**:
- Prevents over-posting attacks
- API contract independent of domain model
- Enables versioning
- Clearer validation rules

### 4. Why GUID for IDs?

**Decision**: Use `Guid` instead of `int` or `long`

**Rationale**:
- Globally unique (distributed systems friendly)
- Prevents enumeration attacks
- No collision risk
- Client-side generation possible

**Trade-offs**:
- Larger storage (16 bytes vs 4/8 bytes)
- Slower indexing in databases
- Not human-readable

### 5. Why Controller-Service Pattern?

**Decision**: Separate controller from business logic

**Rationale**:
- Single Responsibility Principle
- Easier to test business logic
- Reusable service layer
- Clearer code organization

## Scalability Considerations

### Horizontal Scaling

**Current State**: Not stateless due to in-memory storage

**Required Changes for Scaling**:
```csharp
1. Replace in-memory storage with database
2. Use stateless session management
3. Implement distributed caching (Redis)
4. Add load balancer
```

**Deployment Architecture**:
```
                    ┌─────────────┐
                    │ Load Balancer│
                    └──────┬───────┘
                           │
         ┌─────────────────┼─────────────────┐
         │                 │                 │
    ┌────▼────┐      ┌────▼────┐      ┌────▼────┐
    │  API    │      │  API    │      │  API    │
    │Instance1│      │Instance2│      │Instance3│
    └────┬────┘      └────┬────┘      └────┬────┘
         │                 │                 │
         └─────────────────┼─────────────────┘
                           │
                    ┌──────▼───────┐
                    │   Database   │
                    │ (SQL Server) │
                    └──────────────┘
```

### Vertical Scaling

**Optimization Points**:
1. Async I/O operations (✅ Implemented)
2. Connection pooling (database dependent)
3. Response caching
4. Query optimization

### Performance Targets

| Metric | Target | Current |
|--------|--------|---------|
| Response Time (p95) | < 200ms | ~5ms (in-memory) |
| Throughput | > 1000 req/s | Limited by CPU |
| Concurrent Users | > 10,000 | N/A |
| Availability | 99.9% | Depends on hosting |

## ApplyTo Guidelines

### Prerequisites Checklist

Before applying this architecture to production:

- [ ] **Database Setup**
  - [ ] Choose database (SQL Server, PostgreSQL, MySQL)
  - [ ] Design database schema
  - [ ] Implement Entity Framework Core DbContext
  - [ ] Create migration scripts

- [ ] **Security**
  - [ ] Add authentication (JWT, OAuth2)
  - [ ] Add authorization policies
  - [ ] Configure CORS
  - [ ] Enable HTTPS
  - [ ] Add rate limiting

- [ ] **Monitoring**
  - [ ] Configure Application Insights / ELK
  - [ ] Add health check endpoints
  - [ ] Implement metrics collection
  - [ ] Set up alerts

- [ ] **Infrastructure**
  - [ ] Define hosting environment (Azure, AWS, on-prem)
  - [ ] Configure CI/CD pipeline
  - [ ] Set up environments (Dev, Staging, Prod)
  - [ ] Plan backup and recovery

### Migration Steps

1. **Phase 1: Database Integration**
   ```csharp
   // Add EF Core DbContext
   public class ApplicationDbContext : DbContext
   {
       public DbSet<Engagement> Engagements { get; set; }
   }
   ```

2. **Phase 2: Repository Pattern**
   ```csharp
   public class EngagementRepository : IEngagementRepository
   {
       private readonly ApplicationDbContext _context;
       // Implementation
   }
   ```

3. **Phase 3: Caching Layer**
   ```csharp
   services.AddDistributedRedisCache(options =>
   {
       options.Configuration = "redis:6379";
   });
   ```

4. **Phase 4: Security**
   ```csharp
   [Authorize]
   [ApiController]
   public class EngagementController : ControllerBase
   ```

### Architecture Evolution

```
Current (MVP)          Phase 1              Phase 2              Phase 3
─────────────          ───────              ───────              ───────
Controller        →    Controller      →    Controller      →    Controller
    ↓                      ↓                    ↓                    ↓
Service           →    Service         →    Service         →    Service
    ↓                      ↓                    ↓                    ↓
In-Memory              Repository           Repository       →   Repository
                           ↓                    ↓                    ↓
                       Database         →   Database         →   Database
                                               ↓                    ↓
                                           Caching          →   Caching
                                                                    ↓
                                                                Event Bus
```

## Conclusion

This architecture provides a solid foundation for an engagement management system. It follows industry best practices, maintains clean separation of concerns, and is designed for easy evolution as requirements grow. The modular structure allows for incremental enhancement without major refactoring.

**Key Strengths**:
- Clean separation of concerns
- Testable design
- RESTful API standards
- Scalable foundation

**Next Steps**:
1. Implement database persistence
2. Add authentication and authorization
3. Implement comprehensive logging
4. Set up monitoring and alerting
