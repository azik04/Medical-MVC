using MedicalMVC.Models.BaseModel;

namespace MedicalMVC.Models;

public class ProductPhoto :Base
{
    public string PhotoName { get; set; }
    public int ProductId { get; set; }
    public Product Products { get; set; }
}
