using AuthService.Domain.Users;

namespace AuthService.Domain.UnitTests.TestData;

internal static class TestEmail
{
    public const string Raw = "Hiker@Example.COM";

    public const string Normalized = "hiker@example.com";

    public const string PseudonymValue = "deleted+0123456789abcdef@ascenda.invalid";

    public static Email Create() => Email.Create(Raw).Value;

    public static Email Pseudonym() => Email.Create(PseudonymValue).Value;
}
