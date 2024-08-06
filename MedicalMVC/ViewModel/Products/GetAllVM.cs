using MedicalMVC.Models;

namespace MedicalMVC.ViewModel.Products;

public class GetAllVM
{
    public ICollection<Category> Categories { get; set; }
    public ICollection<Product> Products { get; set; }
    public ProductPhoto? ProductPhoto { get; set; }
    public int? CurrentPage { get; set; }
    public int? TotalPages { get; set; }
    public int? PageSize { get; set; }
    public int? TotalCount { get; set; }
}