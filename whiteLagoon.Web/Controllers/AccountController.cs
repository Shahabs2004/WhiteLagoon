using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using whiteLagoon.Web.ViewModels;

namespace whiteLagoon.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;

        }
        public IActionResult Login(string returnUrl=null)
        {
            // Set the value of returnUrl to the result of Url.Content("~/") if it is null
            returnUrl ??= Url.Content("~/");
            LoginVM loginVM = new()
            {
                   RedirectUrl = returnUrl
            };
            return View(loginVM);
        }
        public IActionResult Register()
        {
            if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            { _roleManager.CreateAsync(new IdentityRole("Admin")).Wait();}
            if (!_roleManager.RoleExistsAsync("Customer").GetAwaiter().GetResult())
            {_roleManager.CreateAsync(new IdentityRole("Customer")).Wait();}
            RegisterVM registerVM = new()
            {
                RoleList = _roleManager.Roles.Select(x => new SelectListItem { Text = x.Name, Value = x.Name })
            };
            return View(registerVM);
        }
    }
}
