using MedicalMVC.Models;
using MedicalMVC.Services.Interfaces;
using MedicalMVC.ViewModel.Accounts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedicalMVC.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _service;

    public AccountController(IUserService service, ILogger<AccountController> logger)
    {
        _service = service;
    }


    //<summary>
    //LogIn Page
    //For Admins
    //</summary>
    public async Task<IActionResult> login()
    {
        return View();
    }


    //<summary>
    //LogIn (Authorized) User
    //For Admins
    //</summary>
    [HttpPost]
    public async Task<IActionResult> login(AccountVM account)
    {
        if (!ModelState.IsValid)
            return View(account);
        
        var data = await _service.LogIn(account);
        if (data.StatusCode == Enum.StatusCode.Ok)
        {
            return RedirectToAction("Home", "Admin");
        }

        return View(account);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}