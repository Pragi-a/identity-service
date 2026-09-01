### Error/Exception Handling

                        Application
                            │
            ┌─────────────┴─────────────┐
            │                           │
        Expected outcome            Unexpected failure
            │                           │
            ▼                           ▼
            Result<T>                 Exception
            │                           │
            └─────────────┬─────────────┘
                            ▼
                        API Boundary
                            │
                            ▼
                    HTTP Response                



### Handling Error on the API side

                    Endpoint
                       │
                       ▼
                Application Handler
                       │
                       ▼
                    Result<T>
                       │
              ┌────────┴────────┐
              │                 │
           Failure           Success
              │                 │
              ▼                 ▼
       ErrorToHttpMapper    Endpoint decides
              │             200/201/204/etc.
              ▼
          HttpError
              │
              ▼
       ProblemDetails

       builder.Services.AddScoped<IErrorToHttpMapper, ErrorToHttpMapper>();
builder.Services.AddScoped<IHttpErrorResponseMapper, HttpErrorResponseMapper>();
builder.Services.AddScoped<ISuccessResponseMapper, SuccessResponseMapper>();

register the endpoint filter