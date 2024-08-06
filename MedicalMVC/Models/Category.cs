using MedicalMVC.Models.BaseModel;

namespace MedicalMVC.Models;

public class Category : Base
{
    public string Name { get; set; }
    public ICollection<Product> Products { get; set; }
}
