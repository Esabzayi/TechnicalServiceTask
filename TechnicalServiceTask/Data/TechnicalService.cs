using System.Text.Json.Serialization;

namespace TechnicalServiceTask.Data
{
    public class TechnicalService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<int>? BlockIds { get; set; }
        public List<int>? SystemIds { get; set; }

        public int EmployeeIds { get; set; }


        public ICollection<TechnicalServiceBlock>? TechnicalServiceBlocks { get; set; }
        public ICollection<TechnicalServiceSystem>? TechnicalServiceSystems { get; set; }
        public ICollection<Block>? Blocks { get; set; }
        public ICollection<System>? Systems { get; set; }
        public ICollection<Employee>? Employees { get; set; }
        public DateTime CreationTime { get; set; }
        public ICollection<Activity> Activities { get; set; }
    }
}
