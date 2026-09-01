namespace Identity.API.Common.Results;

public sealed record SuccessResponse
{
    public int StatusCode { get; }

    public string? Location { get; }

    private SuccessResponse(int statusCode, string? location)
    {
        StatusCode = statusCode;
        Location = location;
    }

    public static SuccessResponse Ok() =>
        new SuccessResponse(statusCode: StatusCodes.Status200OK, location: null);

    public static SuccessResponse Created(string location)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(location);
        return new SuccessResponse(statusCode: StatusCodes.Status201Created, location: location);
    }

    public static SuccessResponse NoContent() => new SuccessResponse(StatusCodes.Status204NoContent, null);
}