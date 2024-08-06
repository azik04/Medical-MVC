using MedicalMVC.Context;
using MedicalMVC.Models;
using MedicalMVC.Response;
using MedicalMVC.Services.Interfaces;
using MedicalMVC.ViewModel.Accounts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MedicalMVC.Services.Implementations;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _db;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserService(ApplicationDbContext db, IHttpContextAccessor contextAccessor)
    {
        _db = db;
        _contextAccessor = contextAccessor;
    }


    //<summary>
    //Get All Admins
    //</summary>
    public async Task<BaseResponse<ICollection<User>>> GetAllAdmins()
    {
        try
        {
            var data = await _db.Users.Where(x=> !x.IsDeleted).ToListAsync();
            if (data == null)
            {
                return new BaseResponse<ICollection<User>>
                {
                    Description = "Users == null",
                    StatusCode = Enum.StatusCode.Ok
                };
            }
            Log.Information("GetAllAdmins UserService Success!!!");
            return new BaseResponse<ICollection<User>>
            {
                Data = data,
                Description = "Users have been found",
                StatusCode = Enum.StatusCode.Ok
            };
        }
        catch(Exception ex)
        {
            Log.Error("GetAllAdmins UserService Error!!!", ex.Message);

            return new BaseResponse<ICollection<User>>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }
    }


    //<summary>
    //LogIN (Authenticate) User
    //</summary>
    public async Task<BaseResponse<User>> LogIn(AccountVM account)
    {
        try
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName == account.UserName && !x.IsDeleted);
            if (user == null)
            {
                Log.Warning("User not exist!!");
                return new BaseResponse<User>
                {
                    Description = "Username or Password is wrong",
                    StatusCode = Enum.StatusCode.Error 
                };
            }

            if (account.UserPassword != user.UserPassword)
            {
                Log.Warning("Password is wrong!!");

                return new BaseResponse<User>
                {
                    Description = "Username or Password is wrong",
                    StatusCode = Enum.StatusCode.Error 
                };
            }
            
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, user.UserName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true
            };

            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            Log.Information("LogIn UserService Success!!!");
            return new BaseResponse<User>
            {
                Description = "Login successful",
                StatusCode = Enum.StatusCode.Ok,
                Data = user
            };
        }
        catch (Exception ex)
        {
            Log.Error("LogIn UserService Error!!!", ex.Message);
            return new BaseResponse<User>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }
    }


    //<summary>
    //Register (Create) User
    //</summary>
    public async Task<BaseResponse<User>> Register(AccountVM account)
    {
        try
        {
            if (account == null)
            {
                Log.Warning("User name is busy - UserService!!!");

                return new BaseResponse<User>
                {
                    Description = "Account is null",
                    StatusCode = Enum.StatusCode.Error
                };
            }

            var user = new User
            {
                CreateAt = DateTime.Now,
                UserName = account.UserName,
                UserPassword = account.UserPassword
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            Log.Information("Register UserService Success!!!");
            return new BaseResponse<User>
            {
                Data = user,
                Description = "User successfully created",
                StatusCode = Enum.StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            Log.Error("Register UserService Error!!!", ex.Message);

            return new BaseResponse<User>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }
    }


    //<summary>
    //Remove User by Id
    //</summary>
    public async Task<BaseResponse<User>> RemoveUser(int id)
    {
        try
        {
            var data = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                Log.Warning("User not found - UserService!!!");
                return new BaseResponse<User>
                {
                   
                    Description = "Account is null",
                    StatusCode = Enum.StatusCode.Error
                };
            }
            data.IsDeleted = true;
            await _db.SaveChangesAsync();
            Log.Information("RemmoveUser success UserService Error!!!");
            return new BaseResponse<User>
            {
                Data = data,
                Description = "User successfully created",
                StatusCode = Enum.StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            Log.Error("RemoveUser UserService Error!!!", ex.Message);
            return new BaseResponse<User>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }
    }
}