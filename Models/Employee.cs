using Simulation1.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulation1.Models
{
    public class Employee : BaseEntity
    {
       public  string FullName { get; set; }

public Position Position { get; set; }
public string ImageUrl { get; set; }
        public int PositionId { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
