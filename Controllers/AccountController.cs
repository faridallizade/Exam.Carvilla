using CarVilla.Helpers;
using CarVilla.Models;
using CarVilla.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarVilla.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager , SignInManager<AppUser> signInManager )
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterVm vm)
		{
			if(!ModelState.IsValid) return View();
			AppUser user = new AppUser()
			{
				Name = vm.Name,
				Surname = vm.Surname,
				UserName = vm.Username,
				Email = vm.Email,
			};
			var res = await _userManager.CreateAsync(user,vm.Password);
			if (!res.Succeeded)
			{
				foreach (var error in res.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			await _userManager.AddToRoleAsync(user, UserRole.Member.ToString());
			return RedirectToAction("Login", "Account");
		} 
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async  Task<IActionResult> Login(LoginVm vm)
		{
			if (!ModelState.IsValid) return View();
			var user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
			if(user == null)
			{
				user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
				if(user == null)
				{
					ModelState.AddModelError("", "Username or Email address is incorrect");
				}
			}
			var res = await _signInManager.CheckPasswordSignInAsync(user, vm.Password,false);
			if(!res.Succeeded)
			{
				ModelState.AddModelError("", "Username or Email address is incorrect");
			}
			await _signInManager.SignInAsync(user, false);
			return RedirectToAction("Index", "Home");
		}
		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
		public async Task<IActionResult> CreateRole()
		{
			foreach (var role in Enum.GetValues(typeof(UserRole)))
			{
				var roleExist = await _roleManager.RoleExistsAsync(role.ToString());
				if (!roleExist)
				{
					await _roleManager.CreateAsync(new IdentityRole()
					{
						Name = role.ToString(),
					});
				}
			}
			return RedirectToAction("Index", "Home");
		}
	}
}
