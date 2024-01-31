using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application_Security_Assignment.NewModels;
using System.Net;
using System.Text.Json;
using System.Runtime.CompilerServices;
using WebApp_Core_Identity.Model;

namespace Application_Security_Assignment.Pages
{
	[ValidateAntiForgeryToken]
	public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }

		private readonly SignInManager<User> signInManager;

		private readonly UserManager<User> UserManager;

		private readonly AuthDbContext dbContext;
		public LoginModel(AuthDbContext dbContext, SignInManager<User> signInManager, UserManager<User> userManager)
		{
			this.signInManager = signInManager;
			this.UserManager = userManager;
			this.dbContext = dbContext;
		}
		public void OnGet()
        {
        }
		public bool CheckCaptcha()
		{
			string Response = Request.Form["g-recaptcha-response"];
			bool valid = false;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LfP3WApAAAAABM4PwYWBZY5UFDboTSdRM3IcyoM&response=" + Response);
			try
			{
				using (WebResponse wResponse = request.GetResponse())
				{
					using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
					{
						string jsonResponse = readStream.ReadToEnd();
						var data = JsonSerializer.Deserialize<CaptchaResponse>(jsonResponse);
						valid = Convert.ToBoolean(data.success);
					}
				}
				return valid;
			}
			catch (WebException ex)
			{
				throw ex;
			}

		}
		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				if (!CheckCaptcha())
				{
					ModelState.AddModelError("", "Captcha is not valid");
					return Page();
				}
				var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,
				LModel.RememberMe, true);
				if (identityResult.Succeeded)
				{
					var user = await UserManager.FindByEmailAsync(LModel.Email); // find the user by email
					string guid = Guid.NewGuid().ToString();
					if (user !=null)
					{
						HttpContext.Session.SetString("AuthToken", guid); // set the AuthToken to the session
						user.AuthToken = guid; // set the AuthToken to the user
						await UserManager.UpdateAsync(user); // update the user
					}
					var audit = new Audit
					{
						
						UserID = user.Id,
						Action = "Login",
						Time = DateTime.Now,
						
					};
					dbContext.Audits.Add(audit); // add the audit to the database
					await dbContext.SaveChangesAsync(); // save the changes to the database

					return RedirectToPage("Index");
				}
				else if (identityResult.IsLockedOut)
				{
					ModelState.AddModelError("", "Account is locked out"); // if the account is locked out
				}
				else
				{
					ModelState.AddModelError("", "Username or Password incorrect");
				}
			}
			return Page();
		}
	}
}
