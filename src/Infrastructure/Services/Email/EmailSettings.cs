namespace Services.Email;

public class EmailSettings
{
    public required string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public required string FromEmail { get; set; }
    public required string FromName { get; set; }
    public required string AppPassword { get; set; }
}
