using MedicalMVC.Enum;
using MedicalMVC.Models;
using MedicalMVC.Services.Interfaces;
using MedicalMVC.ViewModel.Products;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedicalMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService service, ICategoryService categoryService, ILogger<ProductController> logger)
        {

            _service = service;
            _categoryService = categoryService;
        }


        //<summary>
        //Get All Confirmed Products Page
        //For All Users
        //</summary>
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var productResponse = await _service.GetAllConfirmed(page);
            var categoryResponse = await _categoryService.GetAll();

            if (productResponse.StatusCode == Enum.StatusCode.Ok && categoryResponse.StatusCode == Enum.StatusCode.Ok)
            {
                var vm = new GetAllVM
                {
                    Categories = categoryResponse.Data,
                    Products = productResponse.Data,
                    CurrentPage = productResponse.CurrentPage,
                    TotalPages = productResponse.TotalPages,
                    PageSize = productResponse.PageSize,
                    TotalCount = productResponse.TotalCount
                };
                return View(vm);
            }
            return BadRequest(productResponse);
        }


        //<summary>
        //Get All Confirmed Products By Alphabet Page
        //For All Users
        //</summary>
        [HttpGet]
        public async Task<IActionResult> alphabet(int page = 1)
        {
            var productResponse = await _service.GetAllConfirmed(page);
            var categoryResponse =await _categoryService.GetAll();

            if (productResponse.StatusCode == Enum.StatusCode.Ok)
            {
                var data = productResponse.Data.OrderBy(x=>x.Name).ToList();
                var vm = new GetAllVM
                {
                    Categories = categoryResponse.Data,
                    Products = data,
                    CurrentPage = productResponse.CurrentPage,
                    TotalPages = productResponse.TotalPages,
                    PageSize = productResponse.PageSize,
                    TotalCount = productResponse.TotalCount
                };
                return View(vm);
            }
            return BadRequest(productResponse);
        }


        //<summary>
        //Get All Confirmed Products By State Page
        //For All Users
        //</summary>
        [HttpGet]
        public async Task<IActionResult> bystate(State state)
        {
            var productResponse = await _service.GetProductsByState(state);
            var categoryResponse =await _categoryService.GetAll();

            if (productResponse.StatusCode == Enum.StatusCode.Ok)
            {
                var vm = new GetAllVM
                {
                    Categories = categoryResponse.Data,
                    Products = productResponse.Data,
                    CurrentPage = productResponse.CurrentPage,
                    TotalPages = productResponse.TotalPages,
                    PageSize = productResponse.PageSize,
                    TotalCount = productResponse.TotalCount
                };
                return View(vm);
            }
            return BadRequest(productResponse);
        }



        //<summary>
        //Get All Products by Category
        //For All Users
        //</summary>
        [HttpGet]
        public async Task<IActionResult> bycategory(int Id, int page = 1)
        {
            var productResponse = await _service.GetByCatAll(Id, page);
            var categoryResponse = await _categoryService.GetAll();

            if (productResponse.StatusCode == Enum.StatusCode.Ok)
            {
                var vm = new GetAllVM
                {
                    Categories = categoryResponse.Data,
                    Products = productResponse.Data,
                    CurrentPage = productResponse.CurrentPage,
                    TotalPages = productResponse.TotalPages,
                    PageSize = productResponse.PageSize,
                    TotalCount = productResponse.TotalCount
                };
                return View(vm);
            }
            return BadRequest(productResponse);
        }


        //<summary>
        //Get All Latest Confirmed Products Page
        //For All Users
        //</summary>
        [HttpGet]
        public async Task<IActionResult> latest(int page = 1)
        {
            var productResponse = await _service.GetAllConfirmed(page);
            var categoryResponse = await _categoryService.GetAll();

            if (productResponse.StatusCode == Enum.StatusCode.Ok)
            {
                var data = productResponse.Data.OrderByDescending(x => x.AdminConfirmAt).ToList();
                var vm = new GetAllVM
                {
                    Categories = categoryResponse.Data,
                    Products = data,
                    CurrentPage = productResponse.CurrentPage,
                    TotalPages = productResponse.TotalPages,
                    PageSize = productResponse.PageSize,
                    TotalCount = productResponse.TotalCount
                };
                return View(vm);
            }
            return BadRequest(productResponse);
        }


        //<summary>
        //Get Product by ID Page
        //For All Users
        //</summary>
        [HttpGet]
        public async Task<IActionResult> id(int id)
        {
            var data = await _service.GetById(id);
            if (data.StatusCode == Enum.StatusCode.Ok)
            {
                return View(data.Data);
            }
            return BadRequest(data.Data);
        }


        //<summary>
        //Create Not Confirmed Product Page
        //For All Users
        //</summary>
        [HttpGet]
        public async Task<IActionResult> request()
        {
            var response = await _categoryService.GetAll();

            if (response.StatusCode == Enum.StatusCode.Ok)
            {
                var viewModel = new RequestViewModel
                {
                    Categories = response.Data.ToList()
                };

                return View(viewModel);
            }
            return BadRequest(response.Data);
            
        }


        //<summary>
        //Create Not Confirmed Product(needs Admin to confirm) 
        //For All Users
        //</summary>
        [HttpPost]
        public async Task<IActionResult> request([FromForm] RequestViewModel product)
        {
            var cat = await _categoryService.GetAll();
            product.Categories = cat.Data;
            if (!ModelState.IsValid)
                return View(product);

            var data = await _service.CreateForUsers(product);
            if (data.StatusCode == Enum.StatusCode.Ok)
            {
                return RedirectToAction("index", "home");
            }
            return BadRequest(data);
        }
        [HttpPost]
        public async Task<IActionResult> remove(int id)
        {
            var cat = await _service.Delete(id);
            if (cat.StatusCode == Enum.StatusCode.Ok)
            {
                return RedirectToAction("index", "home");
            }
            return BadRequest(cat);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
