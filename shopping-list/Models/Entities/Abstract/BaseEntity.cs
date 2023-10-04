
using System.ComponentModel.DataAnnotations.Schema;

namespace shopping_list.Models.Entities.Abstract
{
    public class BaseEntity 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string CreatedUser { get; set; }
    }
}
