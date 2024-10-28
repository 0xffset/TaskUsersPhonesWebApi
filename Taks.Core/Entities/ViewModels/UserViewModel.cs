using System.ComponentModel.DataAnnotations;

namespace Tasks.Core.Entities.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }

        public List<PhoneViewModel> Phones { get; set; }


    }

    public class UserCreateViewModel
    {
        [Required, StringLength(100, MinimumLength = 2)]
        public string? FullName { get; set; }

        [Required, StringLength(20, MinimumLength = 2)]
        public string? UserName { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]

        public string? Password { get; set; }

    }

    public class UserUpdateViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(100, MinimumLength = 2)]
        public string? FullName { get; set; }

        [Required, StringLength(20, MinimumLength = 2)]
        public string? UserName { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

    }
}
