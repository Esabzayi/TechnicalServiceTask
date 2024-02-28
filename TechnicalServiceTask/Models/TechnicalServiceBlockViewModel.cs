using TechnicalServiceTask.Data;

namespace TechnicalServiceTask.Models
{
    public class TechnicalServiceBlockViewModel
    {
        public int TechnicalServiceId { get; set; }
        public TechnicalServiceViewModel? TechnicalService { get; set; }
        public int BlockId { get; set; }
        public BlockViewModel? Block { get; set; }
    }
}
