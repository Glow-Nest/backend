namespace Services.Email;

public class EmailSettings
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string FromEmail { get; set; }
    public string FromName { get; set; }
    public string AppPassword { get; set; }
}
