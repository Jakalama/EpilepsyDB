using System.ComponentModel.DataAnnotations;

namespace EpilepsieDB.Web.View.ViewModels
{
    public class Invite
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

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
