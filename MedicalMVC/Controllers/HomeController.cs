using MedicalMVC.Models;
using MedicalMVC.Services.Interfaces;
using MedicalMVC.ViewModel.Products;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedicalMVC.Controllers;

public class HomeController : Controller
{
    
    private readonly IRequestService _requestService;
    private readonly IProductService _productService;
    public HomeController(IRequestService requestService, IProductService productService, ILogger<HomeController> logger)
    {
        _requestService = requestService;
        _productService = productService;
    }


    //<summary>
    //Index Page
    //Get 4 Latest Created Products
    //For All Users
    //</summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var productResponse = await _productService.GetAllConfirmed();
        var second = await _productService.GetProductsByState(Enum.State.İkinciƏl);

        if (productResponse.StatusCode == Enum.StatusCode.Ok)
        {
            var latestConfirmed = productResponse.Data?
                .OrderByDescending(x => x.AdminConfirmAt)
                .Take(4)
                .ToList();

            var secondHard = second.Data?.Take(4).ToList();

            var vm = new IndexHomeVM
            {
                LatestProduct = latestConfirmed,
                SecondHandProduct = secondHard,
            };
            return View(vm);
        }
        return BadRequest(productResponse);
    }


    //<summary>
    //Contact Page
    //For All Users
    //</summary>
    public async Task<IActionResult> contact()
    {
        return View();
    }


    //<summary>
    //Create Request
    //For All Users
    //</summary>
    [HttpPost]
    public async Task<IActionResult> contact(Request request)
    {
        if (!ModelState.IsValid)
            return View(request);
        
        var data = await _requestService.Create(request);
        if (data.StatusCode == Enum.StatusCode.Ok)
        {
            return RedirectToAction("Index");
        }
        return BadRequest(data.Data);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
