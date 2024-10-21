using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TooGoodToGoAvans.Domain.Models;
using TooGoodToGoAvans.DomainService;
using TooGoodToGoAvans.UI.Models;


namespace TooGoodToGoAvans.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IStudentRepository StudentRepository;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IStudentRepository StudentRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.StudentRepository = StudentRepository;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            var model = new RegisterViewModel() { Role = UserRole.Student };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel registerVM)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("", "Already signed in.");
            }

            var role = registerVM.Role.ToString();
            if (!roleManager.RoleExistsAsync(role).Result)
            {
                ModelState.AddModelError("", "Specified role does not exist");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = new IdentityUser()
            {
                UserName = registerVM.EmailAddress,
                Email = registerVM.EmailAddress
            };

            var registerResult = await userManager.CreateAsync(user, registerVM.Password);

            if (registerResult.Succeeded)
            {
                // Een rol is een claim. Claims zijn algemener, rollen zijn iets makkelijker te gebruiken -->
                await userManager.AddClaimAsync(user, new Claim("UserType", "user"));
                await userManager.AddToRoleAsync(user, registerVM.Role.ToString());

                //Student Student = new()
                //{
                //    Name = registerVM.Name,
                //    Role = registerVM.Role,
                //    IdentityId = user.Id,
                //};
                //StudentRepository.Add(Student);

                return RedirectToAction(nameof(Login), new { returnUrl = registerVM.ReturnUrl ?? "/" });
            }

            foreach (var error in registerResult.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "")
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl, });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {

                var user = await userManager.FindByNameAsync(loginVM.Name);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user,
                        loginVM.Password, false, false)).Succeeded)
                    {
                        return RedirectToAction(nameof(DetailsAsync));
                        //return Redirect(loginVM?.ReturnUrl ?? "/Guest/List");
                    }
                }
            }

            ModelState.AddModelError("", "Invalid name or password");
            return View();
        }

        public async Task<IActionResult> DetailsAsync()
        {
            if (User != null)
            {
                var identityUser = await userManager.FindByNameAsync(User?.Identity?.Name ?? "");
                if (identityUser != null)
                {
                    LoginViewModel loginVM = new()
                    {
                        Name = identityUser.UserName ?? "onbekend",
                        EmailAddress = identityUser.Email
                    };
                    return View(loginVM);
                }
            }

            return RedirectToAction("Login", "Account");
        }


    }
}
