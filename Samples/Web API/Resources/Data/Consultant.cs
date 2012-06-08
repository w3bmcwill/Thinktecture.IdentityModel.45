using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Resources
{
    public class Consultant
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Country { get; set; }
        
        [Required]
        public string EmailAddress { get; set; }

        public string Owner { get; set; }
    }
}
