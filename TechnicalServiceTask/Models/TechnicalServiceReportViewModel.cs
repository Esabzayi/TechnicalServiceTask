namespace TechnicalServiceTask.Models
{
    public class TechnicalServiceReportViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Blocks { get; set; }
        public List<string> Systems { get; set; }
        public List<string> ResponsiblePersons { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
