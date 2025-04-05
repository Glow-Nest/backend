namespace Services.Email;

public static class EmailMessageBuilder
{
    public static string BuildOtpMessage(string recipientName, string otpCode, TimeSpan expiresIn)
    {
        return $"""
                Hello {recipientName},

                Your One-Time Password (OTP) is: {otpCode}

                This code will expire in {expiresIn.TotalMinutes} minutes. Please use it promptly.

                If you did not request this code, please ignore this email.

                Best regards,
                GlowNest Team
                """;
    }

    public static string BuildGenericMessage(string greetingName, string subjectLine, string messageBody)
    {
        return $"""
                Hello {greetingName},

                {messageBody}

                Best regards,
                GlowNest Team
                """;
    }
}