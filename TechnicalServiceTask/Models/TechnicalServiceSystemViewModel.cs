using TechnicalServiceTask.Data;

namespace TechnicalServiceTask.Models
{
    public class TechnicalServiceSystemViewModel
    {
        public int TechnicalServiceId { get; set; }
        public TechnicalServiceViewModel? TechnicalService { get; set; }

        public int SystemId { get; set; }
        public SystemViewModel? System { get; set; }

        public ICollection<TechnicalServiceViewModel>? TechnicalServiceSystems { get; set; }
    }
}
