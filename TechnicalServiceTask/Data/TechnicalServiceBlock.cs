namespace TechnicalServiceTask.Data
{
    public class TechnicalServiceBlock
    {
        public int TechnicalServiceId { get; set; }
        public TechnicalService? TechnicalService { get; set; }

        public int BlockId { get; set; }
        public Block? Block { get; set; }
    }
}
