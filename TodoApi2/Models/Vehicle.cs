using System.ComponentModel.DataAnnotations;

namespace TodoApi2.Models
{
   
    public class Vehicule
    {
        [Key]
        public long vehicule_id { get; set; }
        public string? vehicule_desc { get; set; }
    }
}
