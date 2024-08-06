using MedicalMVC.Models;
using MedicalMVC.Services.Interfaces;
using MedicalMVC.ViewModel.Accounts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedicalMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        //<summary>
        //Get All Admin
        //For Admin
        //</summary>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> admins()
        {
            var data = await _userService.GetAllAdmins();
            if (data.StatusCode == Enum.StatusCode.Ok)
            {
                return View(data.Data);
            }
            return BadRequest(data.Data);

        }


        //<summary>
        //Remove Admin
        //For Admin
        //</summary>
        [Authorize]
        public async Task<IActionResult> remove(int id)
        {
            var data = await _userService.RemoveUser(id);
            if (data.StatusCode == Enum.StatusCode.Ok)
            {
                return RedirectToAction("index","home");
            }
            return BadRequest(data.Data);
        }



        //<summary>
        //Register Page
        //For Admins
        //</summary>
        public async Task<IActionResult> registrate()
        {
            return View();
        }


        //<summary>
        //Register (Create) User 
        //For Admins
        //</summary>
        [HttpPost]
        public async Task<IActionResult> registrate(AccountVM account)
        {
            if (!ModelState.IsValid)
                return View(account);

            var data = await _userService.Register(account);
            if (data.StatusCode == Enum.StatusCode.Ok)
            {
                return RedirectToAction("index", "home");
            }

            return BadRequest(data);
        }


        //<summary>
        //LogOut User
        //For Admins
        //</summary>
        [HttpPost]
        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
