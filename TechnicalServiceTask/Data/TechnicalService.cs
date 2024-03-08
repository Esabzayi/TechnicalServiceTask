using System.Text.Json.Serialization;

namespace TechnicalServiceTask.Data
{
    public class TechnicalService
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public int CreatePersonId { get; set; }
        public string CreatePersonNames { get; set; }
        public int ConfirmPersonId { get; set; }
        public string ConfirmPersonNames { get; set; }
        public int ApprovePersonId { get; set; }
        public string ApprovePersonNames { get; set; }
        public int VerifyPersonId { get; set; }
        public string VerifyPersonNames { get; set; }

        public ICollection<TechnicalServiceBlock>? TechnicalServiceBlocks { get; set; }
        public ICollection<TechnicalServiceSystem>? TechnicalServiceSystems { get; set; }
    }
}
