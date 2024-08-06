using MedicalMVC.Enum;
using MedicalMVC.Models;
using MedicalMVC.Response;
using MedicalMVC.ViewModel.Products;

namespace MedicalMVC.Services.Interfaces
{
    public interface IProductService
    {
        Task<PagedResponse<ICollection<Product>>> GetNotConfirmed(int page = 1, int pageSize = 15);
        Task<PagedResponse<ICollection<Product>>> GetAllConfirmed(int page = 1, int pageSize = 15);
        Task<PagedResponse<ICollection<Product>>> GetByCatAll(int id, int page = 1, int pageSize = 15);
        Task<PagedResponse<ICollection<Product>>> GetProductsByState(State state, int page = 1, int pageSize = 15);



        Task<BaseResponse<Product>> GetById(int id);
        Task<BaseResponse<Product>> UpdateConfirm(ConfirmViewModel product);
        Task<BaseResponse<Product>> Delete(int id);
        Task<BaseResponse<Product>> CreateForUsers(RequestViewModel product);

    }
}
