using System.ComponentModel.DataAnnotations;

namespace EpilepsieDB.Models
{
    public abstract class BaseModel
    {
        [Key]
        public int ID { get; set; }
    }
}
