namespace Identity.API.Common.MiddleWare;

public sealed class CorrelationContext : ICorrelationContext
{
    public Guid CorrelationId { get;  }
}