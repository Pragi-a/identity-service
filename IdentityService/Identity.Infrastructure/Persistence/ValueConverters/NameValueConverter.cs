using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identity.Infrastructure.Persistence.ValueConverters;

public sealed class NameValueConverter()
    : ValueConverter<Name, string>(name => name.Value, s => new Name(s));