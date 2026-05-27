using System.ComponentModel.DataAnnotations;

namespace Simulation1.Areas.Admin.ViewModels.Position
{
    public record UpdatePositionVM
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name can not exceed 20 characters")]
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
