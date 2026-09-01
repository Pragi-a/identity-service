                    HTTP Request
                         │
                         ▼
              Correlation Middleware
                         │
                         ├── establish CorrelationId
                         │
                         ├── update ICorrelationContext
                         │
                         └── Begin logging scope
                                  │
                                  ▼
                       ┌─────────────────────┐
                       │ Application         │
                       │                     │
                       │ Handler             │
                       │ Domain              │
                       │ EF Core             │
                       │ Infrastructure      │
                       └──────────┬──────────┘
                                  │
                                  ▼
                               ILogger
                                  │
                                  ▼
                         Structured Log Event




                         Activity.Current
                               │
                         TraceId / SpanId
                               │
                               ▼
                            ILogger
                               │
                ┌──────────────┴──────────────┐
                ▼                             ▼
          CorrelationId                  TraceId/SpanId
          application                    observability
