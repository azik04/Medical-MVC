using MedicalMVC.Models;
using MedicalMVC.Response;
using MedicalMVC.ViewModel.Categories;

namespace MedicalMVC.Services.Interfaces;

public interface ICategoryService
{
    Task<BaseResponse<ICollection<Category>>> GetAll(); 
    Task<BaseResponse<Category>> GetById(int id);
    Task<BaseResponse<Category>> Update(int id, CreateCategoryVM category);
    Task<BaseResponse<Category>> Delete(int id);
    Task<BaseResponse<Category>> Create(CreateCategoryVM category);
}
