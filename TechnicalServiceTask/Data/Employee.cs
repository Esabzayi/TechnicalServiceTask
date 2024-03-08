using System.Text.Json.Serialization;

namespace TechnicalServiceTask.Data
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public string PIN { get; set; }
    }
}
