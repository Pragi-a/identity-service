namespace Identity.API.Common.Correlation;

public interface ICorrelationContextInitializer
{
    void SetCorrelationId(Guid correlationId);
}