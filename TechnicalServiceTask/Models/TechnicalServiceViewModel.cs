
using TechnicalServiceTask.Data;

namespace TechnicalServiceTask.Models
{
    public class TechnicalServiceViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatePersonId { get; set; }
        public int ConfirmPersonId { get; set; }
        public int ApprovePersonId { get; set; }
        public int VerifyPersonId { get; set; }
        public List<int> BlockIds { get; set; }
        public List<int> SystemIds { get; set; }
    }
}
