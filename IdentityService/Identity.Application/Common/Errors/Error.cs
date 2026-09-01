namespace Identity.Application.Common.Errors;

public abstract record Error
{
    public string Code { get; init; }

    public string Message { get; init; }

    public abstract ErrorClassification Classification { get; }

    public static readonly Error None = new StandardError(string.Empty, string.Empty, ErrorClassification.NONE);

    protected Error(string Code, string Message)
    {
        this.Code = Code;
        this.Message = Message;
    }

    private record StandardError : Error
    {
        private readonly ErrorClassification _classification;

        public sealed override ErrorClassification Classification => _classification;

        public StandardError(string code, string message, ErrorClassification classification) : base(code, message)
        {
            _classification = classification;
        }
    }

    public abstract record ValidationErrorBase : Error,IValidationError
    {
        public sealed override ErrorClassification Classification => ErrorClassification.VALIDATION;

        protected ValidationErrorBase(string code, string message) : base(code, message)
        {
        }

        public abstract IReadOnlyDictionary<string, string[]> ValidationErrors { get; } 
    }

    public static Error NotFound(string code, string message)
    {
        return new StandardError(code, message, ErrorClassification.NOT_FOUND);
    }

    public static Error Unauthorized(string code, string message)
    {
        return new StandardError(code, message, ErrorClassification.NOT_AUTHORIZED);
    }

    public static Error Unauthenticated(string code, string message)
    {
        return new StandardError(code, message, ErrorClassification.NOT_AUTHENTICATED);
    }

    public static Error Conflict(string code, string message)
    {
        return new StandardError(code, message, ErrorClassification.CONFLICT);
    }
}