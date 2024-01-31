using Microsoft.AspNetCore.Identity;

namespace Application_Security_Assignment.NewModels
{
	public class User : IdentityUser // adding columns to the identity user table // dont include email and password as they are already included in the identity user table
	{
		public string FullName { get; set; }

		public string CreditCard { get; set; }

		public string Gender { get; set; }

		public string DeliveryAddress { get; set; }

		public string Photo { get; set; }

		public string AboutMe { get; set; }

		public string? AuthToken { get; set; } // logged in Auth token of session, not logged in = null
	}
}
