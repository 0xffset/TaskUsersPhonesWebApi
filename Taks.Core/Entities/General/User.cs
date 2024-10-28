using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Tasks.Core.Entities.General
{
    public class User : IdentityUser<int>
    {

        [Required, StringLength(maximumLength: 100, MinimumLength = 2)]
        public required string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public required string Email { get; set; }
        public bool IsActive { get; set; }

        public int? EntryBy { get; set; }
        public DateTime? Created { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? Updated { get; set; }

        public DateTime? LastLogin { get; set; }

        public ICollection<Phone>? Phones { get; set; }


    }
}
