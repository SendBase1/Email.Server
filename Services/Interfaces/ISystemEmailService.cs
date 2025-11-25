namespace Email.Server.Services.Interfaces;

/// <summary>
/// Service for sending system emails (verification, password reset, etc.)
/// These emails are sent from the platform's own verified domain, not tenant domains.
/// </summary>
public interface ISystemEmailService
{
    /// <summary>
    /// Sends an email verification link to the user
    /// </summary>
    Task SendVerificationEmailAsync(string toEmail, string userId, string verificationToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a password reset link to the user
    /// </summary>
    Task SendPasswordResetEmailAsync(string toEmail, string resetToken, CancellationToken cancellationToken = default);
}
