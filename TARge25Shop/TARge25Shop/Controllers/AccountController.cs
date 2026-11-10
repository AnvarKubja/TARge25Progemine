using TARge25Shop.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models.Account;
using RegisterViewModel = TARge25Shop.Models.Account.RegisterViewModel;

namespace TARge25Shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailServices _emailServices;

        public AccountController
            (
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailServices emailServices
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailServices = emailServices;
        }

        // Kuvab registreerimisvormi lehe
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // Töötleb vormilt tulnud andmed (andmete salvestamine)
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Name = model.Name,
                    Email = model.Email,
                    City = model.City
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                    EmailTokenDto newsignup = new();
                    newsignup.Token = token;
                    newsignup.Body = $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>";
                    newsignup.Subject = "Confirm your email";
                    newsignup.To = user.Email;

                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    _emailServices.SendEmailToken(newsignup, token);
                    List<string> errordatas =
                        [
                            "Area", "Accounts",
                            "Issue", "Success",
                            "StatusMessage", "Registration Success",
                            "ActedOn", $"{model.Email}",
                            "CreatedAccountData", $"{model.Email}\n{model.City}\n[password hidden]\n[password hidden]"
                        ];
                    ViewBag.ErrorData = errordatas;
                    ViewBag.ErrorTitle = "Registration Success";
                    ViewBag.ErrorMessage = "Please check your email to confirm your account.";

                    return View("ConfirmationEmailMessage");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            // Kui andmed pole valiidsed, kuva vorm uuesti koos veateadetega
            return View();
        }
    }
}
