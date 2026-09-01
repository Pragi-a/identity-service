namespace Identity.API.Common.MiddleWare;

public interface ICorrelationContext
{
    Guid CorrelationId { get; }
}