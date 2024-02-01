using Application_Security_Assignment.NewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp_Core_Identity.Model;

namespace Application_Security_Assignment.Pages
{
    public class LogoutModel : PageModel
    {
		private readonly SignInManager<User> signInManager;
		private readonly UserManager<User> userManager;
		private readonly AuthDbContext dbContext;
		public LogoutModel(AuthDbContext dbContext, SignInManager<User> signInManager, UserManager<User> userManager) 
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.dbContext = dbContext;
		}

		public void OnGet()
        {
        }

		public async Task<IActionResult> OnPostLogoutAsync()
		{
			var user = await userManager.GetUserAsync(User);
			user.AuthToken = null; // setting the auth token to null
			await userManager.UpdateAsync(user); // updating the user
			var audit = new Audit
			{

				UserID = user.Id,
				UserName = user.UserName,
				Action = "Logged Out",
				Time = DateTime.Now,

			};
			dbContext.Audits.Add(audit); // add the audit to the database
			await dbContext.SaveChangesAsync(); // save the changes to the database
			await signInManager.SignOutAsync();
			HttpContext.Session.Clear(); // clearing the session
			return RedirectToPage("Login");
		}
		public async Task<IActionResult> OnPostDontLogoutAsync()
		{
			return RedirectToPage("Index");
		}
	}
}
