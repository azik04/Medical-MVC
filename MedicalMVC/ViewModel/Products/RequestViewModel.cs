using MedicalMVC.Enum;
using MedicalMVC.Models;

namespace MedicalMVC.ViewModel.Products;

public class RequestViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int CategoryId { get; set; }
    public ICollection<Category>? Categories { get; set; }
    public State State { get; set; }
    public string Owners { get; set; }
    public ICollection<IFormFile> Photos { get; set; }
}
