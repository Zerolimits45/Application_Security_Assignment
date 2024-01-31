using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.Metadata;

namespace Application_Security_Assignment.NewModels
{
    public class Register // model for the form
    {
        [Required]
		[DataType(DataType.Text)]
        public string FullName { get; set; }

        [Required]
		[DataType(DataType.CreditCard)] //sanitation
		public string CreditCard { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string Gender { get; set; }

		[Required]
		[DataType(DataType.PhoneNumber)] //sanitation
		[RegularExpression(@"^\d{8}$", ErrorMessage = "Mobile number is invalid.")]
		public string MobileNo { get; set; }

		[Required]
        [DataType(DataType.Text)]
        public string DeliveryAddress { get; set; }

		[Required]
        [DataType(DataType.EmailAddress)] //sanitation
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
		[MinLength(12, ErrorMessage = "Password must be at least 12 characters.")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$", // server side validation
		ErrorMessage = "Password must include at least one lowercase letter, one uppercase letter, one digit, and one special character.")]
		public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }

		[DataType(DataType.Upload)]
		public IFormFile Photo { get; set; } // get the file from the form

		[Required]
		[RegularExpression(@"^[^\s].*", ErrorMessage = "About Me should not start with a space.")]
		public string AboutMe { get; set; }
	}


}
