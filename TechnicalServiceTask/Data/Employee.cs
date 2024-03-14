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


        [JsonIgnore]
        public ICollection<TechnicalService> CreatedTechnicalServices { get; set; }

        [JsonIgnore]
        public ICollection<TechnicalService> ConfirmedTechnicalServices { get; set; }

        [JsonIgnore]
        public ICollection<TechnicalService> ApprovedTechnicalServices { get; set; }

        [JsonIgnore]
        public ICollection<TechnicalService> VerifiedTechnicalServices { get; set; }
    }
}
