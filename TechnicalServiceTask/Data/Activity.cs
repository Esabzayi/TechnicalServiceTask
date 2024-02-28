using System.Text.Json.Serialization;

namespace TechnicalServiceTask.Data
{
    public class Activity
    {
        [JsonIgnore]
        public int TechnicalServiceId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
