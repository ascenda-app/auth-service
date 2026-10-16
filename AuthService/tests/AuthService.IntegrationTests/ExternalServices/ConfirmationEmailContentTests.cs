using AuthService.Infrastructure.ExternalServices;
using FluentAssertions;
using MimeKit;
using Xunit;

namespace AuthService.IntegrationTests.ExternalServices;

public sealed class ConfirmationEmailContentTests
{
    private const string Recipient = "hiker@example.com";
    private const string RawToken = "raw-token";

    [Fact]
    public void ToMimeMessage_AddressesTheMessageWithTheConfiguredSender()
    {
        using MimeMessage message = ConfirmationEmailContent.ToMimeMessage(Options(), Recipient, RawToken);

        message.From.Mailboxes.Single().Name.Should().Be("Ascenda");
        message.From.Mailboxes.Single().Address.Should().Be("no-reply@localhost");
        message.To.Mailboxes.Single().Address.Should().Be(Recipient);
        message.Subject.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void ToMimeMessage_PutsTheConfirmationLinkInBothBodies()
    {
        using MimeMessage message = ConfirmationEmailContent.ToMimeMessage(Options(), Recipient, RawToken);

        message.HtmlBody.Should().Contain(RawToken);
        message.TextBody.Should().Contain(RawToken);
    }

    [Fact]
    public void For_EscapesTheTokenInTheLink()
    {
        ConfirmationEmailContent content = ConfirmationEmailContent.For(Options(), "a+b/c=");

        content.Html.Should().Contain("a%2Bb%2Fc%3D").And.NotContain("a+b/c=");
        content.Text.Should().Contain("a%2Bb%2Fc%3D");
    }

    [Fact]
    public void For_AnnouncesTheConfiguredTokenLifetime()
    {
        ConfirmationEmailContent content = ConfirmationEmailContent.For(Options(), RawToken);

        content.Text.Should().Contain("24 horas");
    }

    private static EmailConfirmationOptions Options() => new()
    {
        FromAddress = "no-reply@localhost",
        FromName = "Ascenda",
        ConfirmationLinkTemplate = "https://localhost/confirm-email?token={token}"
    };
}
