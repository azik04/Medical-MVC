using MedicalMVC.Models;
using MedicalMVC.Response;
using MedicalMVC.ViewModel.Accounts;

namespace MedicalMVC.Services.Interfaces;

public interface IUserService
{
    Task<BaseResponse<User>> Register(AccountVM account);
    Task<BaseResponse<User>> LogIn(AccountVM account);
    Task<BaseResponse<ICollection<User>>> GetAllAdmins();
    Task<BaseResponse<User>> RemoveUser(int id);
}
