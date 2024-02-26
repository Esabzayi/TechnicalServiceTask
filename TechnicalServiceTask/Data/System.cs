using System.Text.Json.Serialization;

namespace TechnicalServiceTask.Data
{
    public class System
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ParentSystemId { get; set; }
        public System? ParentSystem { get; set; }
    }
}
