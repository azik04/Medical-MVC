using MedicalMVC.Models;
using MedicalMVC.Response;
using MedicalMVC.ViewModel.Categories;

namespace MedicalMVC.Services.Interfaces;

public interface IRequestService
{
    Task<BaseResponse<ICollection<Request>>> GetAll();
    Task<BaseResponse<Request>> Delete(int id);
    Task<BaseResponse<Request>> Create(Request category);
}
