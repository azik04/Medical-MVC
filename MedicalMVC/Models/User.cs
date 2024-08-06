using MedicalMVC.Models.BaseModel;

namespace MedicalMVC.Models;

public class User : Base
{
    public string UserName { get; set; }
    public string UserPassword { get; set; }
}
