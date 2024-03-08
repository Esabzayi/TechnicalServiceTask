namespace TechnicalServiceTask.Data
{
    public class TechnicalServiceSystem
    {
        public int TechnicalServiceId { get; set; }
        public TechnicalService TechnicalService { get; set; }
        public int SystemId { get; set; }
        public System System { get; set; }
    }
}
