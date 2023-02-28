using Microsoft.Build.Framework;

namespace EpilepsieDB.Web.View.ViewModels
{
    public class Permissions
    {
        [Required]
        public string UserID { get; set; }

        [Required]
        public bool IsSystemadmin { get; set; }

        [Required]
        public bool IsUser { get; set; }

        [Required]
        public bool IsScanCreator { get; set; }

        [Required]
        public bool IsScanDownloader { get; set; }

        [Required]
        public bool IsScanReader { get; set; }
    }
}
