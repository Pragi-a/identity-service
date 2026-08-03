using System.Linq.Expressions;
using Identity.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identity.Infrastructure.Persistence.ValueConverters;

public sealed class EmailValueConverter()
    : ValueConverter<Email, string>(email => email.Value, value => new Email(value));