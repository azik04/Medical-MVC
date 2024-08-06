using MedicalMVC.Models;
using MedicalMVC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedicalMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RequestController : Controller
    {
        private readonly IRequestService _requestService;
        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }
        //<summary>
        //Remove All Requests Page
        //For Admin
        //</summary>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> index()
        {
            var data = await _requestService.GetAll();

            if (data.StatusCode == Enum.StatusCode.Ok)
            {
                return View(data.Data);
            }
            return BadRequest(data);
        }


        //<summary>
        //Remove Request By Id
        //For Admin
        //</summary>
        [Authorize]
        public async Task<IActionResult> remove(int id)
        {
            var data = await _requestService.Delete(id);
            if (data.StatusCode == Enum.StatusCode.Ok)
            {
                return RedirectToAction("index" , "home");
            }
            return BadRequest(data.Data);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
