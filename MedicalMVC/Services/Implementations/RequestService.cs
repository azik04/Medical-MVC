using MedicalMVC.Context;
using MedicalMVC.Models;
using MedicalMVC.Response;
using MedicalMVC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MedicalMVC.Services.Implementations;

public class RequestService : IRequestService
{
    private readonly ApplicationDbContext _db;
    public RequestService(ApplicationDbContext db)
    {
        _db = db;
    }


    //<summary>
    //Create Request
    //</summary>
    public async Task<BaseResponse<Request>> Create(Request request)
    {
        try
        {
            if (request == null)
            {
                return new BaseResponse<Request>
                {
                    Description = "Request is null",
                    StatusCode = Enum.StatusCode.Error,
                };
            }
            var data = new Request
            {
                CreateAt = DateTime.Now,
                Email = request.Email,
                Message = request.Message,
                Name = request.Name,
                Phone = request.Phone,
            };
            await _db.Requests.AddAsync(data);
            await _db.SaveChangesAsync();
            Log.Information("Create Reques success RequestService!!!");
            return new BaseResponse<Request>
            {
                Data = data,
                Description = $"Request: {data.Name} has been successfully created",
                StatusCode = Enum.StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            Log.Error("Create RequestService Error!!!", ex.Message);
            return new BaseResponse<Request>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }
    }


    //<summary>
    //Remove Request by Id 
    //</summary>
    public async Task<BaseResponse<Request>> Delete(int id)
    {
        try
        {
            var data = await _db.Requests.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                return new BaseResponse<Request>
                {
                    Description = "Request is null",
                    StatusCode = Enum.StatusCode.Error,
                };
            }
            data.IsDeleted = true;
            await _db.SaveChangesAsync();
            Log.Information("Remove Reques success RequestService!!!");
            return new BaseResponse<Request>
            {
                Data = data,
                Description = $"Request: {data.Name} has been successfully  Removed",
                StatusCode = Enum.StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            Log.Error("Delete RequestService Error!!!", ex.Message);
            return new BaseResponse<Request>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }
    }


    //<summary>
    //Get All Requests 
    //</summary>
    public async Task<BaseResponse<ICollection<Request>>> GetAll()
    {
        try
        {
            var data = await _db.Requests.Where(x => !x.IsDeleted).ToListAsync();
            if (data == null)
            {
                return new BaseResponse<ICollection<Request>>
                {
                    Description = "Request is null",
                    StatusCode = Enum.StatusCode.Ok,
                };
            }
            Log.Information("GetAll Reques success RequestService!!!");
            return new BaseResponse<ICollection<Request>>
            {
                Data = data,
                Description = $"Requests (Count: {data.Count}) has been successfully Found",
                StatusCode = Enum.StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            Log.Error("GetAll RequestService Error!!!", ex.Message);

            return new BaseResponse<ICollection<Request>>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }
    }
}
