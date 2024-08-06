using MedicalMVC.Models;
using MedicalMVC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedicalMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService service,  ICategoryService categoryService, ILogger<ProductController> logger)
        {
            _service = service;
            _categoryService = categoryService;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> notconfirmed()
        {
            var productResponse = await _service.GetNotConfirmed();

            if (productResponse.StatusCode == Enum.StatusCode.Ok)
            {
                return View(productResponse.Data);
            }
            return BadRequest(productResponse);
        }


        //<summary>
        //Confirm Product Page
        //For Admin
        //</summary>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> confirm(int id)
        {
            var productResponse = await _service.GetById(id);

            if (productResponse.StatusCode == Enum.StatusCode.Ok)
            {
                var product = productResponse.Data;
                var categoriesResponse = await _categoryService.GetAll();
                var categories = categoriesResponse.Data;
                var catbyid = await _categoryService.GetById(product.CategoryId);
                var result = catbyid.Data;
                var model = new ConfirmViewModel
                {
                    ID = product.Id,
                    UserEmail = product.UserEmail,
                    Description = product.Description,
                    UserName = product.UserName,
                    UserPhone = product.UserPhone,
                    Categories = categories,
                    CategoryId = product.CategoryId,
                    Category = result,
                    Owners = product.Owners,
                    State = product.State
                };
                return View(model);
            }
            return BadRequest(productResponse);
        }


        //<summary>
        //Confirm Product(adding some stuff)
        //For Admin
        //</summary>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> confirm(ConfirmViewModel product)
        {
            var cat = await _categoryService.GetAll();
            var catbyid = await _categoryService.GetById(product.CategoryId);
            product.Categories = cat.Data;
            product.Category = catbyid.Data;
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            var data = await _service.UpdateConfirm(product);
            if (data.StatusCode == Enum.StatusCode.Ok)
            {
                return RedirectToAction("NotConfirmed", "Product");
            }
            return BadRequest(data);
        }


        //<summary>
        //Remove Product By Id
        //For Admin
        //</summary>
        [Authorize]
        public async Task<IActionResult> remove(int id)
        {
            var data = await _service.Delete(id);
            if (data.StatusCode == Enum.StatusCode.Ok)
            {
                return RedirectToAction("index", "Home");
            }
            return BadRequest(data);

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
