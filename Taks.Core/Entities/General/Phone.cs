using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Taks.Core.Entities.General;

namespace Tasks.Core.Entities.General
{
    [Table("Phones")]
    public class Phone : Base<int>
    {
        [Key]
        public new int Id { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter a valid phone number")]

        public required string Number { get; set; }
        [Required(ErrorMessage = "City code is required")]
        public required string CityCode { get; set; }
        [Required(ErrorMessage = "Country code is required")]
        public required string CountryCode { get; set; }

        public  User? User { get; set; }


    }
}

