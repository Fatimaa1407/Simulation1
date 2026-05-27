using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulation1.Areas.Admin.ViewModels.Employee
{
    public record CreateEmployeeVM
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name can not exceed 20 characters")]
        [MinLength(3, ErrorMessage = "Name should contain at least 3 characters")]
        public string FullName { get; set; }
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Position is required")]
        public int PositionId { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
