using Identity.API.Common.MiddleWare;

namespace Identity.API.Common.Correlation;

internal sealed class CorrelationContext : ICorrelationContext, ICorrelationContextInitializer
{
    public Guid CorrelationId { get; private set; }

    public void SetCorrelationId(Guid correlationId)
    {
        if (CorrelationId != Guid.Empty)
        {
            return;
        }

        CorrelationId = correlationId;
    }
}