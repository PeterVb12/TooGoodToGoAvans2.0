using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TooGoodToGoAvans.Domain.Models;
using TooGoodToGoAvans.DomainService;
using TooGoodToGoAvans.Infrastructure;
using TooGoodToGoAvans.UI.Models;

namespace TooGoodToGoAvans.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IStudentRepository studentRepository;
        private readonly IStaffMemberRepository staffMemberRepository;
        private readonly ICanteenRepository canteenRepository;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IStudentRepository studentRepository,
            IStaffMemberRepository staffMemberRepository,
            ICanteenRepository canteenRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.studentRepository = studentRepository;
            this.staffMemberRepository = staffMemberRepository;
            this.canteenRepository = canteenRepository;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            var model = new RegisterViewModel()
            {
                Role = UserRole.Student,
                Canteens = await canteenRepository.GetAllCanteensAsync() 
            };

            if (model.Canteens == null || !model.Canteens.Any())
            {
                throw new Exception("Geen kantines gevonden.");
            }

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
            if (!await roleManager.RoleExistsAsync(role))
            {
                ModelState.AddModelError("", "Specified role does not exist");
            }

            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            var user = new IdentityUser()
            {
                UserName = registerVM.EmailAddress,
                Email = registerVM.EmailAddress
            };

            var registerResult = await userManager.CreateAsync(user, registerVM.Password);

            if (registerResult.Succeeded)
            {
                await userManager.AddClaimAsync(user, new Claim("UserType", "user"));
                await userManager.AddToRoleAsync(user, role);

                if (registerVM.Role == UserRole.Student)
                {
                    var student = new Student(
                        id: Guid.NewGuid(),
                        name: registerVM.Name,
                        birthdate: registerVM.Birthdate.Value,
                        studentId: registerVM.StudentId,
                        emailAddress: registerVM.EmailAddress,
                        studentCity: registerVM.City,
                        phonenumber: registerVM.Phonenumber,
                        userId: "123"
                    );
                    await studentRepository.AddAsync(student);
                }
                else if (registerVM.Role == UserRole.StaffMember)
                {
                    var staffMember = new StaffMember(
                        staffMemberId: Guid.NewGuid(),
                        name: registerVM.Name,
                        employeeNumber: registerVM.StaffMemberId,
                        workLocation: await canteenRepository.GetCanteenByIdAsync(registerVM.SelectedCanteenId), 
                        staffmemberCity: registerVM.City
                    );
                    await staffMemberRepository.AddAsync(staffMember);
                }

                return RedirectToAction(nameof(Login), new { returnUrl = registerVM.ReturnUrl ?? "/" });
            }

            foreach (var error in registerResult.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return View(registerVM);
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "")
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
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
                    var result = await signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError("", "Invalid name or password");
            return View(loginVM);
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
                    return View("Index", loginVM);
                }
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
