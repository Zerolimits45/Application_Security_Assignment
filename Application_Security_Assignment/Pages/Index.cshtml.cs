using Application_Security_Assignment.NewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_Core_Identity.Model;

namespace Application_Security_Assignment.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private UserManager<User> userManager { get; }
        private SignInManager<User> signInManager { get; }
        public IndexModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public async Task OnGet()
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                if (user.AuthToken == HttpContext .Session.GetString("AuthToken"))

                {
					HttpContext.Session.SetString("Email", user.Email);
					HttpContext.Session.SetString("Name", user.FullName);
					HttpContext.Session.SetString("Gender", user.Gender);
					HttpContext.Session.SetString("PhoneNumber", user.PhoneNumber);
					HttpContext.Session.SetString("DeliveryAddress", user.DeliveryAddress);
					HttpContext.Session.SetString("AboutMe", user.AboutMe);
					var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
					var protector = dataProtectionProvider.CreateProtector("MySecretKey");

					HttpContext.Session.SetString("CreditCard", protector.Unprotect(user.CreditCard)); // decrypting the credit card data

				}
				else
                {
					await signInManager.SignOutAsync();
					HttpContext.Session.Clear(); // clearing the session
					Response.Redirect("Login");
				}
				
				
			}

		}
    }
}