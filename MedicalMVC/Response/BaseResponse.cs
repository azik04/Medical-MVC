using MedicalMVC.Enum;

namespace MedicalMVC.Response;

public class BaseResponse<T>
{
    public T Data { get; set; }
    public string Description { get; set; }
    public StatusCode StatusCode { get; set; }
}
