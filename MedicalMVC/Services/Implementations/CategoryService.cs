using MedicalMVC.Context;
using MedicalMVC.Models;
using MedicalMVC.Response;
using MedicalMVC.Services.Interfaces;
using MedicalMVC.ViewModel.Categories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MedicalMVC.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _db; 
    public CategoryService(ApplicationDbContext db)
    {
        _db = db;
    }


    //<summary>
    //Create a new Category 
    //</summary>
    public async Task<BaseResponse<Category>> Create(CreateCategoryVM category)
    {
        try
        {
            if (category == null)
            {
                Log.Warning("Create VM CategoryService == null!!!");

                return new BaseResponse<Category>
                {
                    Description = "Category is null",
                    StatusCode = Enum.StatusCode.Error,
                };
            }

            var data = new Category
            {
                CreateAt = DateTime.Now,
                Name = category.Name,
            };

            await _db.Categories.AddAsync(data);
            await _db.SaveChangesAsync();

            Log.Information("Create Category success!!! -  CategoryService");

            return new BaseResponse<Category>
            {
                Data = data,
                Description = $"Category: {data.Name} has been successfully created",
                StatusCode = Enum.StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            Log.Error("Create CategoryService Error!!!", ex.Message);
            return new BaseResponse<Category>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }
    }


    //<summary>
    //Remove Category by id 
    //</summary>
    public async Task<BaseResponse<Category>> Delete(int id)
    {
        try
        {
            var data = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                Log.Warning("Delete CategoryService not found!!!");

                return new BaseResponse<Category>
                {
                    Description = $"Category with ID{id} = NULL",
                    StatusCode = Enum.StatusCode.Error
                };
            }
            data.IsDeleted = true;
            await _db.SaveChangesAsync();

            Log.Information("Delete Category success!!! -  CategoryService");

            return new BaseResponse<Category>
            {
                Data = data,
                Description = $"Category: {data.Name} has been successfully removed",
                StatusCode = Enum.StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            Log.Error("Delate CategoryService Error!!!", ex.Message);
            return new BaseResponse<Category>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }

    }


    //<summary>
    //Get Category by id 
    //</summary>
    public async Task<BaseResponse<Category>> GetById(int id)
    {
        try
        {
            var data = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (data == null)
            {
                Log.Warning("GetById CategoryService not found!!!");

                return new BaseResponse<Category>
                {
                    Description = $"Category with ID{id} = NULL",
                    StatusCode = Enum.StatusCode.Error
                };
            }

            Log.Information("GetById Category success!!! -  CategoryService");

            return new BaseResponse<Category>
            {
                Data = data,
                Description = $"Category: {data.Name} has been successfully found",
                StatusCode = Enum.StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            Log.Error("GetById CategoryService Error!!!", ex.Message);
            return new BaseResponse<Category>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }
    }



    //<summary>
    //Get All Categories 
    //</summary>
    public async Task<BaseResponse<ICollection<Category>>> GetAll()
    {
        try
        {
            var data = await _db.Categories.Where(x => !x.IsDeleted).ToListAsync();
            if (data == null)
            {
                return new BaseResponse<ICollection<Category>>
                {
                    Description = $"Category = NULL",
                    StatusCode = Enum.StatusCode.Ok
                };
            }

            Log.Information("GetAll Category success!!! -  CategoryService");

            return new BaseResponse<ICollection<Category>>
            {
                Data = data,
                Description = $"Category have been successfully found",
                StatusCode = Enum.StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            Log.Error("Get All CategoryService Error!!!", ex.Message);
            return new BaseResponse<ICollection<Category>>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }
    }



    //<summary>
    //Updaet a Category by id 
    //</summary>   
    public async Task<BaseResponse<Category>> Update(int id, CreateCategoryVM category)
    {
        try
        {
            var data =await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                Log.Warning("Update CategoryService not found!!!");
                return new BaseResponse<Category>
                {
                    Description = $"Category with ID{id} = NULL",
                    StatusCode = Enum.StatusCode.Error
                };
            }
            data.UpdateAt = DateTime.Now;
            data.Name = category.Name;
            _db.Categories.Update(data);
            await _db.SaveChangesAsync();

            Log.Information("GetAll Category success!!! -  CategoryService");

            return new BaseResponse<Category>
            {
                Data = data,
                Description = $"Category: {data.Name} has been successfully Update",
                StatusCode = Enum.StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            Log.Error("Update CategoryService Error!!!", ex.Message);
            return new BaseResponse<Category>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }
    }
}
