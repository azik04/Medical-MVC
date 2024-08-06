using MedicalMVC.Models;

namespace MedicalMVC.ViewModel.Products;

public class IndexHomeVM
{
    public ICollection<Product> LatestProduct { get; set; }
    public ICollection<Product> SecondHandProduct { get; set; }
}
