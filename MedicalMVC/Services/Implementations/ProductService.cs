using MedicalMVC.Context;
using MedicalMVC.Enum;
using MedicalMVC.Models;
using MedicalMVC.Response;
using MedicalMVC.Services.Interfaces;
using MedicalMVC.ViewModel.Products;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MedicalMVC.Services.Implementations;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _db;
    public ProductService(ApplicationDbContext db)
    {
        _db = db;
    }



    //<summary>
    //Create a new Product and a new Photo for Product 
    //</summary>
    public async Task<BaseResponse<Product>> CreateForUsers(RequestViewModel product)
    {
        try
        {
            if (product == null)
            {
                Log.Warning("CreateForUsers VM ProductService == null!!!");

                return new BaseResponse<Product>
                {
                    Description = "Product is null",
                    StatusCode = Enum.StatusCode.Error,
                };
            }

            var data = new Product
            {
                UserName = product.Name,
                UserEmail = product.Email,
                UserPhone = product.Phone,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Owners = product.Owners,
                State = product.State
            };

            await _db.Products.AddAsync(data);
            await _db.SaveChangesAsync();

            foreach (var item in product.Photos)
            {
                if (item == null || item.Length == 0) continue;

                var uploadDirectory = Path.Combine("wwwroot", "img");
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                var saveFilePath = Path.Combine(uploadDirectory, fileName);

                await using (var stream = new FileStream(saveFilePath, FileMode.Create))
                {
                    await item.CopyToAsync(stream);
                }

                var roomPhotoEntity = new ProductPhoto
                {
                    ProductId = data.Id,
                    CreateAt = DateTime.Now,
                    PhotoName = fileName
                };

                await _db.Photos.AddAsync(roomPhotoEntity);
            }

            await _db.SaveChangesAsync();

            Log.Information("CreateForUsers Product success!!! -  ProductService");

            return new BaseResponse<Product>
            {
                Data = data,
                Description = $"Product: {data.UserName} has been successfully created",
                StatusCode = Enum.StatusCode.Ok
            };
        }
        catch (Exception ex)
        {

            Log.Error("Create ProductService Errorr!!!", ex.Message);
            return new BaseResponse<Product>
            {
                Description = "Error during creating product",
                StatusCode = Enum.StatusCode.Error
            };
        }
    }


    //<summary>
    //Remove Product by Id and Photo by ProductId
    //</summary>
    public async Task<BaseResponse<Product>> Delete(int id)
    {
        try
        {
            var data = await _db.Products.SingleOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                Log.Warning("Delete ProductService not found!!!");

                return new BaseResponse<Product>
                {
                    Description = $"Product with ID{id} = NULL",
                    StatusCode = Enum.StatusCode.Error
                };
            }
            var phn = await _db.Photos.Where(x => x.ProductId == id).ToListAsync();
            foreach (var item in phn)
            {
                item.IsDeleted = true;
                await _db.SaveChangesAsync();
            }
            data.IsDeleted = true;
            await _db.SaveChangesAsync();

            Log.Information("Delete Product success!!! -  ProductService");

            return new BaseResponse<Product>
            {
                Data = data,
                Description = $"Product: {data.Name} has been successfully removed",
                StatusCode = Enum.StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            Log.Error("Delate ProductService Errorr!!!", ex.Message);
            return new BaseResponse<Product>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }

    }
    
    
    //<summary>
    //Get Product and its Photos(all Photos) by ID thats was Confirmed By Admin
    //</summary>
    public async Task<BaseResponse<Product>> GetById(int id)
    {
        try
        {
            var data = await _db.Products.SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted );
            if (data == null)
            {
                Log.Warning("GetById ProductService not found!!!");

                return new BaseResponse<Product>
                {
                    Description = $"Product with ID {id} not found",
                    StatusCode = Enum.StatusCode.Error
                };
            }
            data.Photos = await _db.Photos.Where(x => x.ProductId == id).ToListAsync();
            data.Category = await _db.Categories.SingleOrDefaultAsync(x => x.Id == data.CategoryId);

            Log.Information("GetById Product success!!! -  ProductService");

            return new BaseResponse<Product>
            {
                Data = data,
                Description = $"Product: {data.Name} has been successfully found",
                StatusCode = Enum.StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            Log.Error("GetById ProductService Errorr!!!", ex.Message);
            return new BaseResponse<Product>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }
    }


    //<summary>
    //Get All Confitmed by Admin Products and its Photo(Count: 1)
    //using Pagination
    //</summary>
    public async Task<PagedResponse<ICollection<Product>>> GetAllConfirmed(int page = 1, int pageSize = 15)
    {
        try
        {
            var totalProducts = await _db.Products.CountAsync(x => !x.IsDeleted && x.IsConfirm);
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var data = await _db.Products
                          .Where(x => !x.IsDeleted && x.IsConfirm)
                          .Skip((page - 1) * pageSize)
                          .Take(pageSize)
                          .ToListAsync();

            if (data.Count == 0 || !data.Any())
            {
                return new PagedResponse<ICollection<Product>>
                {
                    Description = "No products found",
                    StatusCode = StatusCode.Ok,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    TotalCount = totalProducts
                };
            }

            foreach (var item in data)
            {
                item.Photos = _db.Photos.Where(x => x.ProductId == item.Id).Take(1).ToList();
                item.Category = _db.Categories.SingleOrDefault(x => x.Id == item.CategoryId);
            }

            Log.Information("GetAllConfirmed Product success!!! -  ProductService");

            return new PagedResponse<ICollection<Product>>
            {
                Data = data,
                Description = "Confirmed Products have been successfully found",
                StatusCode = StatusCode.Ok,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalCount = totalProducts
            };
        }
        catch (Exception ex)
        {
            Log.Error("GetAllConfirmed ProductService Errorr!!!", ex.Message);
            return new PagedResponse<ICollection<Product>>
            {
                Description = ex.Message,
                StatusCode = StatusCode.Error
            };
        }
    }


    //<summary>
    //Get All Confitmed by Admin Products and its Photo(Count: 1) by Category
    //using Pagination
    //</summary>
    public async Task<PagedResponse<ICollection<Product>>> GetByCatAll(int id, int page = 1, int pageSize = 15)
    {
        try
        {
            var totalProducts = await _db.Products.CountAsync(x => !x.IsDeleted && x.IsConfirm && x.CategoryId == id);
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            var data = await _db.Products
                         .Where(x => !x.IsDeleted && x.IsConfirm && x.CategoryId == id)
                         .Skip((page - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync();

            if (data.Count == 0 || !data.Any())
            {
                return new PagedResponse<ICollection<Product>>
                {
                    Description = "No products found",
                    StatusCode = StatusCode.Ok,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    TotalCount = totalProducts
                };
            }

            foreach (var item in data)
            {
                item.Photos = await _db.Photos.Where(x => x.ProductId == item.Id).Take(1).ToListAsync();
                item.Category = await _db.Categories.SingleOrDefaultAsync(x => x.Id == item.CategoryId);
            }

            Log.Information("GetByCatAll Product success!!! -  ProductService");

            return new PagedResponse<ICollection<Product>>
            {
                Data = data,
                Description = "Products by Category have been successfully found",
                StatusCode = StatusCode.Ok,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalCount = totalProducts
            };
        }
        catch (Exception ex)
        {
            Log.Error("GetByCatAll ProductService Errorr!!!", ex.Message);
            return new PagedResponse<ICollection<Product>>
            {
                Description = ex.Message,
                StatusCode = StatusCode.Error
            };
        }
    }


    //<summary>
    //Get All Not Confitmed by Admin Products and its Photo(Count: 1)
    //using Pagination
    //</summary>
    public async Task<PagedResponse<ICollection<Product>>> GetNotConfirmed(int page = 1, int pageSize = 15)
    {
        try
        {
            var totalProducts = await _db.Products.CountAsync(x => !x.IsDeleted && !x.IsConfirm);
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var data =await _db.Products
                          .Where(x => !x.IsDeleted && !x.IsConfirm)
                          .Skip((page - 1) * pageSize)?
                          .Take(pageSize)
                          .ToListAsync();

            if (data.Count == 0 || !data.Any())
            {
                return new PagedResponse<ICollection<Product>>
                {
                    Description = "No products found",
                    StatusCode = StatusCode.Ok,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    TotalCount = totalProducts
                };
            }

            foreach (var item in data)
            {
                item.Photos =await _db.Photos.Where(x => x.ProductId == item.Id).Take(1).ToListAsync();
                item.Category =await _db.Categories.SingleOrDefaultAsync(x => x.Id == item.CategoryId);
            }

            Log.Information("GetNotConfirmed Product success!!! -  ProductService");

            return new PagedResponse<ICollection<Product>>
            {
                Data = data,
                Description = "Not Confirmed(by Admin) Products have been successfully found",
                StatusCode = StatusCode.Ok,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalCount = totalProducts
            };
        }
        catch (Exception ex)
        {
            Log.Error("Get Not Confirmed ProductService Errorr!!!", ex.Message);
            return new PagedResponse<ICollection<Product>>
            {
                Description = ex.Message,
                StatusCode = StatusCode.Error
            };
        }
    }



    //<summary>
    //Admin Updates asn Add Product Values ang gets it Confitmed
    //using Pagination
    //</summary>
    public async Task<BaseResponse<Product>> UpdateConfirm(ConfirmViewModel product)
    {
        try
        {
            var data = await _db.Products.SingleOrDefaultAsync(x => x.Id == product.ID);

            if (data == null)
            {
                Log.Warning("GetById ProductService not found!!!");

                return new BaseResponse<Product>
                {
                    Description = $"Product with ID {product.ID} not found",
                    StatusCode = Enum.StatusCode.Error
                };
            }
            data.AgeGroup = product.AgeGroup;
            data.UpdateAt = DateTime.Now;
            data.Brand = product.Brand;
            data.Control = product.Control;
            data.Name = product.Name;
            data.Country = product.Country;
            data.CategoryId = product.CategoryId;
            data.AdminConfirmAt = DateTime.Now;
            data.IsConfirm = true;
            data.UserPhone = product.UserPhone;
            data.UserName = product.UserName;
            data.UserEmail = product.UserEmail;
            data.Type = product.Type;
            data.Control = product.Control;
            data.Owners = product.Owners;
            await _db.SaveChangesAsync();

            Log.Information("UpdateConfirm Product success!!! -  ProductService");

            return new BaseResponse<Product>
            {
                Data = data,
                Description = $"Product: {data.Name} has been successfully Updated and Confirmed",
                StatusCode = Enum.StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            Log.Error("Update ProductService Errorr!!!", ex.Message);
            return new BaseResponse<Product>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }
    }


    //<summary>
    //Get All Products by State
    //using Pagination
    //</summary>
    public async Task<PagedResponse<ICollection<Product>>> GetProductsByState(State state, int page = 1, int pageSize = 15)
    {
        try
        {
            var totalProducts = await _db.Products.CountAsync(p => p.State == state && !p.IsDeleted && p.IsConfirm);;
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var data = await _db.Products
                          .Where(p => p.State == state && !p.IsDeleted && p.IsConfirm)
                          .Skip((page - 1) * pageSize)
                          .Take(pageSize)
                          .ToListAsync();

            if (data.Count == 0 || !data.Any())
            {
                return new PagedResponse<ICollection<Product>>
                {
                    Description = "No products found",
                    StatusCode = StatusCode.Ok,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    TotalCount = totalProducts
                };
            }

            foreach (var item in data)
            {
                item.Photos = await _db.Photos.Where(x => x.ProductId == item.Id).Take(1).ToListAsync();
                item.Category = await _db.Categories.SingleOrDefaultAsync(x => x.Id == item.CategoryId);
            }

            Log.Information("GetProductsByState Product success!!! -  ProductService");

            return new PagedResponse<ICollection<Product>>
            {
                Data = data,
                Description = "Products by State have been successfully found",
                StatusCode = StatusCode.Ok,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalCount = totalProducts
            };
        }
        catch (Exception ex)
        {
            Log.Error("GetProductsByState ProductService Error!!!", ex.Message);
            return new PagedResponse<ICollection<Product>>
            {
                Description = ex.Message,
                StatusCode = Enum.StatusCode.Error
            };
        }
    }
}
