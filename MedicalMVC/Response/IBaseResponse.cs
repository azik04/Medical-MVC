using MedicalMVC.Enum;

namespace MedicalMVC.Response;

public interface IBaseResponse<T>
{
    T Data {  get; set; }
    string Description { get; set; }
    StatusCode StatusCode { get; set; }
}
