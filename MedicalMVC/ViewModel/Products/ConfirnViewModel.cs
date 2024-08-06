using MedicalMVC.Enum;
using MedicalMVC.Models;

public class ConfirmViewModel
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Control { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string AgeGroup { get; set; }
    public string Country { get; set; }
    public string Brand { get; set; }
    public int CategoryId { get; set; }
    public string UserName { get; set; }
    public string UserPhone { get; set; }
    public string UserEmail { get; set; }
    public string Owners { get; set; }
    public State State { get; set; }
    public ICollection<Category>? Categories { get; set; }
    public Category? Category { get; set; }
}