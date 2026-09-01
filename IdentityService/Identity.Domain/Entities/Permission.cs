namespace Identity.Domain.Entities;

public sealed class Permission
{
    public Guid Id { get; private set; }
    public string Code { get; private set; }
    public string DisplayName { get; private set; }
    public string Description { get; private set; }


    private Permission(Guid id, string code, string displayName, string description)
    {
        Id = id;
        Code = code;
        DisplayName = displayName;
        Description = description;
    }


    public static Permission Create(string code, string displayName, string description)
    {
        ValidateCode(code);

        var finalCode = NormalizeCode(code);

        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Invalid display name for Permission");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Invalid description for Permission");


        return new Permission(
            Guid.NewGuid(),
            finalCode,
            displayName.Trim(),
            description.Trim()
        );
    }

    public void RenameDisplayName(string newDisplayName)
    {
        if (string.IsNullOrWhiteSpace(newDisplayName))
            throw new ArgumentException("Display name cannot be empty.");

        newDisplayName = newDisplayName.Trim();
        if (newDisplayName == DisplayName) return;
        DisplayName = newDisplayName;
    }

    public void UpdateDescription(string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
            throw new ArgumentException("Description cannot be empty.");
        
        newDescription = newDescription.Trim();
        if (newDescription == Description) return;
        Description = newDescription;
    }


    private static string NormalizeCode(string code)
    {
        var codeDetails = code.Split(':');

        var firstPart = codeDetails[0].ToLowerInvariant().Trim();
        var secondPart = codeDetails[1].ToLowerInvariant().Trim();

        var finalCode = firstPart + ":" + secondPart;
        return finalCode;
    }

    private static void ValidateCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Permission code cannot be empty.");

        if (code.Split(':').Length != 2)
            throw new ArgumentException("Invalid permission code.");
    }
}