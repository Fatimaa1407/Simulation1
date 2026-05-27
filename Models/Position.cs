using Simulation1.Models.Base;

namespace Simulation1.Models
{
    public class Position : BaseEntity
    {
        public string  Name { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
