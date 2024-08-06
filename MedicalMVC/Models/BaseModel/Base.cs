namespace MedicalMVC.Models.BaseModel;

public class Base
{
    public int Id { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public bool IsDeleted { get; set; } 
}
