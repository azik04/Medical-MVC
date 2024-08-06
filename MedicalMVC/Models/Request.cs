using MedicalMVC.Models.BaseModel;

namespace MedicalMVC.Models;

public class Request : Base
{
    public string Name { get; set;}
    public string Email { get; set;}
    public string Phone { get; set;}
    public string Message { get; set;}
}
