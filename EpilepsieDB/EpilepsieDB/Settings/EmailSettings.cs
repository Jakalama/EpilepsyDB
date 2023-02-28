namespace EpilepsieDB.Settings
{
    public class EmailSettings
    {
        public bool Disabled { get; set; }
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string Password { get; set; }
    }
}
