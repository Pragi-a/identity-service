namespace Identity.API.Common.Correlation;

public interface ICorrelationContext
{
    public Guid CorrelationId { get; }
}