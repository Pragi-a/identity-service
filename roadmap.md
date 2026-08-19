## 🎯 Master Roadmap Checklist

### 1. Business & Domain Concepts
- [✅] **Authentication** (JWT generation, login flow)
- [✅] **Authorization** (RBAC, Permission-based handlers, dynamic policies)
- [ ] **User Management** (CRUD operations, profile updates, status toggles)
- [ ] **Role Management** (Custom roles, permission mapping)
- [ ] **Password Reset** (Forgot password token, reset verification)
- [ ] **Email Verification** (Token generation, activation flow)
- [ ] **Current User Context** (`ICurrentUser` abstraction)
- [ ] **Audit Trail** (Created/Updated metadata capture)
- [ ] **Soft Delete** (Global query filters, state management)
- [ ] **Domain Events** (In-process event dispatching)
- [ ] **Automated Testing** (Unit & Integration test suites)

-- Current Pending Items
    1. Seeding the Version while Creating an User
    2. Resource Based Authorization
    3. Exception Handling
    4. Repeated code in the Controllers
    5. IUnitOfWork
    6. ICurrentUser

---

### 2. Architecture & Patterns

#### A. Read Side & CQRS Optimization
- [ ] Implement explicit separation: `Query` $\rightarrow$ `Query Service` $\rightarrow$ `Projection` $\rightarrow$ `Read DTO`
- [ ] Apply `AsNoTracking()` on read queries to eliminate tracking overhead
- [ ] Map domain models directly to Read DTOs via `.Select()` projections
- [ ] Add standardized pagination wrappers (`PageNumber`, `PageSize`, `TotalCount`)
- [ ] Implement dynamic filtering and sorting extensions
- [ ] Enforce dedicated query response models per endpoint (avoid entity materialization)

#### B. Transaction Boundaries & Unit of Work
- [ ] Audit `DbContext` usage vs. explicit `IUnitOfWork` implementation
- [ ] Define atomic transaction scopes across multi-repository operations
- [ ] Document clear guidelines on when explicit transactions are required vs. redundant

#### C. Event-Driven Core & Outbox Pattern
- [ ] Design in-process `IDomainEvent` interface and publisher
- [ ] Implement EF Core `OutboxMessage` table persistence inside atomic business transactions
- [ ] Create background `OutboxProcessor` (Worker Service) for guaranteed message publishing
- [ ] Separate internal **Domain Events** from external **Integration Events**

---

### 3. Security Architecture

#### A. Token Management & Rotation
- [ ] Implement Refresh Token rotation (revoking previous token upon single use)
- [ ] Implement **Token Family** tracking to detect token reuse attacks
- [ ] Implement automatic token family revocation upon detected replay attacks

#### B. Revocation & Session Controls
- [ ] Add explicit logout endpoint (revoking refresh token & blacklisting access tokens)
- [ ] Add "Logout All Devices" / "Revoke All Sessions" endpoint
- [ ] Invalidate active sessions upon password reset or account suspension
- [ ] Store revoked token footprints in Redis with automated TTL expiration

#### C. Fine-Grained Authorization
- [ ] Build `ICurrentUser` interface (`UserId`, `Email`, `IReadOnlyCollection<string> Permissions`)
- [ ] Decouple HTTP context / claims dependencies from core domain and application layers
- [ ] Implement Resource-Based Authorization handlers (e.g., Ownership checks: `Resource.UserId == CurrentUser.Id`)

---

### 4. Persistence & Data Architecture

#### A. Concurrency & Integrity
- [ ] Implement Optimistic Concurrency controls (`xmin` system column in PostgreSQL or rowversion)
- [ ] Handle `DbUpdateConcurrencyException` with centralized retry/conflict resolution logic

#### B. Interceptors & Query Filters
- [ ] Create EF Core `SaveChangesInterceptor` for automatic `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy` population
- [ ] Configure EF Core Global Query Filters for soft-deleted entities (`!IsDeleted`)
- [ ] Document guidelines for bypass scenarios (`IgnoreQueryFilters()`)

#### C. Database Performance & Indexing
- [ ] Add composite indexes for high-frequency security join tables (`UserRoles`, `RolePermissions`)
- [ ] Index Foreign Keys and unique constraints (`Email`, `NormalizedUsername`)
- [ ] Analyze execution plans (`EXPLAIN ANALYZE`) for authorization queries to prevent $N+1$ issues

---

### 5. Distributed Systems & Microservices Integration

#### A. Messaging & Event Bus
- [ ] Configure RabbitMQ producer for publishing domain integration events (`UserCreated`, `UserPasswordReset`)
- [ ] Implement strong message contract versioning
- [ ] Configure Dead-Letter Queues (DLQ) and exponential backoff retry policies

#### B. Reliability & Operations
- [ ] Implement **Idempotent Consumer** handling using processed message logs
- [ ] Add `CorrelationId` propagation across HTTP headers and message envelopes
- [ ] Wire up **OpenTelemetry** for logs, metrics, and distributed tracing across boundary calls

---

### 6. Resilience & Fault Tolerance

- [ ] Implement Polly resilience policies (Retry, Circuit Breaker, Timeout)
- [ ] Isolate external email provider dependencies from primary HTTP requests (asynchronous execution)
- [ ] Implement API Rate Limiting per client IP / Authenticated User ID

---

### 7. API Design & Operational Excellence

#### A. Error Handling & Standardization
- [ ] Replace custom error models with RFC 7807 `ProblemDetails` response format
- [ ] Implement centralized exception handling middleware / `IExceptionHandler`

#### B. API Metadata & Lifecycle
- [ ] Implement API Versioning scheme (`/api/v1/`, `/api/v2/` or header-based)
- [ ] Complete OpenAPI (Swagger) documentation with precise HTTP status codes (`400`, `401`, `403`, `404`, `409`)
- [ ] Add ASP.NET Core Health Checks (`/health/ready`, `/health/live`) targeting PostgreSQL and Redis

---

### 8. Testing Strategy

- [ ] **Unit Tests:** Business logic rules, domain entity validation, authorization policy requirements
- [ ] **Integration Tests:** Endpoint verification using `WebApplicationFactory`
- [ ] **Database Integration Tests:** Real PostgreSQL instances executed via **Testcontainers**
- [ ] **Security Tests:** Route-level authentication and authorization handler assertions

---

### 9. Infrastructure & Deployment

- [ ] Write production-grade multi-stage `Dockerfile`
- [ ] Configure Docker Compose environment (Identity Service + PostgreSQL + Redis + RabbitMQ)
- [ ] Configure automated database migrations on container initialization / startup pipeline
- [ ] Set up environment-based secrets management
- [ ] Configure ECS deployment task definitions and horizontal autoscaling rules

---

## 📌 Phase Completion Checklist

- [x] **Phase 1: Security Foundations** (Auth, JWT, RBAC Core)
- [ ] **Phase 2: Data Persistence & Advanced Architecture** (CQRS Read-side, Unit of Work, Interceptors)
- [ ] **Phase 3: Domain & Integration Events** (Outbox Pattern, RabbitMQ Messaging)
- [ ] **Phase 4: Resilience, Observability & Test Automation** (Polly, OpenTelemetry, Testcontainers)
- [ ] **Phase 5: Production Deployment & Containerization** (Docker, Health Checks, ECS)

Additional Items
1. Logging
2. Observability
3. IClock abstraction




Identity Service — Pending Topics
1. Authorization — remaining
JWT permission staleness / permission changes after token issuance
Authorization caching and consistency
Resource-based authorization
RBAC vs permission-based authorization — when each is appropriate
ABAC — when it becomes useful
Final authorization architecture review

2. API Security
CORS
CSRF — when it matters for our authentication model
Rate limiting / throttling
Brute-force protection
Security headers
Input/security boundaries
Authentication vs authorization failure semantics (401 vs 403)

3. Error Handling & API Resilience
Global exception handling
Exception → Result / Problem Details mapping
Consistent API error contract
Logging strategy
Correlation IDs
Handling unexpected infrastructure failures

4. Persistence / EF Core — deeper topics
Optimistic concurrency — we've touched it, but can go deeper if needed
EF Core tracking vs AsNoTracking
Query performance
Transactions and isolation levels
N+1 queries
Database constraints vs application validation
Indexing strategy

5. Testing
Unit testing domain logic
Handler testing
Behavior/pipeline testing
Integration testing with PostgreSQL
Testing transactions and rollback
Authorization integration tests
Testcontainers

6. Observability
Structured logging
Metrics
Distributed tracing
OpenTelemetry
Request/operation correlation
Health checks

7. Architecture / Production concerns
Dependency boundaries and project structure review
Vertical Slice Architecture review
CQRS boundaries
Outbox Pattern
Domain Events
Idempotency
Background processing
Distributed-system considerations
8. Deployment / AWS
Docker production setup
Configuration/secrets
AWS deployment architecture
Database migrations
Health checks
CI/CD
Monitoring in AWS
Already covered / intentionally skipped

Covered:

Result pattern
Validation behavior
MediatR pipeline
Unit of Work
Explicit transactions
Transaction behavior
Deactivation
RBAC/permissions foundation
Dynamic permission policies
JWT permissions

Skipped in this branch:

Refresh-token rotation
Token families
Reuse detection
Session revocation


I think this is what we chose before

--Cache Pattern
Key: authz:user:{userId}
Value: ["user:view", "user:update", "user:delete"]


    Permission/Role change
            ↓
    Outbox event
            ↓
    Event consumer
            ↓
    Get affected users
            ↓
    Invalidate user caches
            ↓
    Next request → DB
            ↓
Rebuild effective permissions
            ↓
        Cache