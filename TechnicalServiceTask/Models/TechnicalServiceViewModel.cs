
using TechnicalServiceTask.Data;

namespace TechnicalServiceTask.Models
{
    public class TechnicalServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<int>? BlockIds { get; set; }
        public List<int>? SystemIds { get; set; }

        public int EmployeeIds { get; set; }

        public ICollection<BlockViewModel>? Blocks { get; set; }
        public ICollection<SystemViewModel>? Systems { get; set; }
        public ICollection<EmployeeViewModel>? Employees { get; set; }
        public DateTime CreationTime { get; set; }
        public ICollection<Activity> Activities { get; set; }
    }
    public class TechnicalServiceDto
    {
        public int TechnicalServiceId { get; set; }
        public string TechnicalServiceName { get; set; }
        public string TechnicalServiceDescription { get; set; }
        public string ResponsiblePersonName { get; set; }
        public string BlockCode { get; set; }
        public string SystemCode { get; set; }

        public ICollection<Activity> Activities { get; set; }
    }
}
