using System.Web;
using Application_Security_Assignment.NewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Security_Assignment.Pages
{
    [ValidateAntiForgeryToken]
    public class RegisterModel : PageModel
    {
        private UserManager<User> userManager { get; }
        private SignInManager<User> signInManager { get; }
        [BindProperty]
        public Register RModel { get; set; }
        public RegisterModel(UserManager<User> userManager,SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
				var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
				var protector = dataProtectionProvider.CreateProtector("MySecretKey");
				var extension = Path.GetExtension(RModel.Photo.FileName).ToLower();

				if (extension != ".jpg" && extension != ".jpeg")
				{
					ModelState.AddModelError("RModel.Photo", "Only .jpg and .jpeg files are allowed");
					return Page();
				}


				var user = new User()
                {
                    UserName = RModel.Email,
                    Email = RModel.Email,
                    FullName = RModel.FullName,
                    CreditCard = protector.Protect(RModel.CreditCard), // encrypting the credit card data
                    Gender = RModel.Gender,
                    DeliveryAddress = RModel.DeliveryAddress,
                    PhoneNumber = RModel.MobileNo,
                    Photo = RModel.Photo.FileName,
                    AboutMe = HttpUtility.HtmlEncode(RModel.AboutMe) // encoding the about me data

				};
                var result = await userManager.CreateAsync(user, RModel.Password); // automatically hash and salt the password (password data protection)
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToPage("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }


        public void OnGet()
        {
        }
    }
}
