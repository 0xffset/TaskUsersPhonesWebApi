using System.ComponentModel.DataAnnotations;

namespace Tasks.Core.Entities.ViewModels
{
    public class PhoneViewModel
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public string? CityCode { get; set; }
        public string? CountryCode { get; set; }
    }

    public class PhoneCreateViewModel
    {
        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public required string Number { get; set; }
        [Required(ErrorMessage = "City code is required")]

        public required string CityCode { get; set; }
        [Required(ErrorMessage = "Country code is required")]

        public required string CountryCode { get; set; }

        [Required(ErrorMessage = "User id is required")]
        public required int UserId { get; set; }

    }

    public class PhoneUpdateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string? Number { get; set; }

        [Required(ErrorMessage = "City code is required")]
        public string? CityCode { get; set; }
        [Required(ErrorMessage = "Country code is required")]

        public string? CountryCode { get; set; }
    }


}

