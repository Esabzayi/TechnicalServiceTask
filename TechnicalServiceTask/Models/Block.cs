using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TechnicalServiceTask.Models
{

    public class Block
    {
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}

