namespace TechnicalServiceTask.Models
{
    public class SystemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ParentSystemId { get; set; }
        public SystemViewModel? ParentSystem { get; set; }
    }
}
