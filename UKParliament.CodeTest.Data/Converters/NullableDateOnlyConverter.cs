using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace UKParliament.CodeTest.Data.Converters;

/// <summary>
/// Converts <see cref="DateOnly?" /> to <see cref="DateTime?"/> and vice versa.
/// Converter from https://github.com/dotnet/efcore/issues/24507#issuecomment-891034323
/// Can be removed after next EF release as per the issue since it should be implemented in EF natively
/// </summary>
public class NullableDateOnlyConverter : ValueConverter<DateOnly?, DateTime?>
{
    /// <summary>
    /// Creates a new instance of this converter.
    /// </summary>
    public NullableDateOnlyConverter() : base(
        d => d == null 
            ? null 
            : new DateTime?(d.Value.ToDateTime(TimeOnly.MinValue)),
        d => d == null 
            ? null 
            : new DateOnly?(DateOnly.FromDateTime(d.Value)))
    { }
}