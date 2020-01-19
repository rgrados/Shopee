namespace Shopee.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Helpers;
    using Models;
    using Mappers;

    public class AccountController : Controller
    {
        private readonly IUserHelper userHelper;

        public AccountController(IUserHelper userHelper)
        {
            this.userHelper = userHelper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Failed to login.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userHelper.GetUserByEmailAsync(model.Username);

                if (user == null)
                {
                    var userResult = await userHelper.AddUserAsync(model.ToUser(), model.Password);

                    if (userResult != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    var loginResult = await userHelper.LoginAsync(model.ToLoginViewModel());

                    if (loginResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, "The user couldn't be login.");
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, "The username is already registered.");
            }

            return View(model);
        }

        public async Task<IActionResult> ChangeUser()
        {
            var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
            return View(user.ToChangeUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {
                    var response = await userHelper.UpdateUserAsync(model.ToUpdateUser(user));
                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return View(model);
        }
    }
}