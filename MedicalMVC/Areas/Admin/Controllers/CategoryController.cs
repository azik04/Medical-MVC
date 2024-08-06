using MedicalMVC.Models;
using MedicalMVC.Services.Interfaces;
using MedicalMVC.ViewModel.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedicalMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
        }


        //<summary>
        //Get All Categories Page
        //For Admin
        //</summary>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> index()
        {
            var data = await _categoryService.GetAll();

            if (data.StatusCode == Enum.StatusCode.Ok)
            {
                return View(data.Data);
            }
            return BadRequest(data);

        }

        //<summary>
        //Create a Category Page
        //For Admin
        //</summary>
        [Authorize]
        public async Task<IActionResult> newcategory()
        {
            return View();
        }


        //<summary>
        //Create a new Category
        //For Admin
        //</summary>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> newcategory(CreateCategoryVM category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            var data = await _categoryService.Create(category);
            if (data.StatusCode == Enum.StatusCode.Ok)
            {
                return RedirectToAction("index","home");
            }
            return BadRequest(data);
        }




        //<summary>
        //Remove Category by Id
        //For Admin
        //</summary>
        [Authorize]
        public async Task<IActionResult> remove(int id)
        {
            var data = await _categoryService.Delete(id);
            if (data.StatusCode == Enum.StatusCode.Ok)
            {
                return RedirectToAction("index","home");
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
