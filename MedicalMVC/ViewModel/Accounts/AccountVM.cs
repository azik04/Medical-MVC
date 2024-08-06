using System.ComponentModel.DataAnnotations;

namespace MedicalMVC.ViewModel.Accounts;

public class AccountVM
{
    [Required(ErrorMessage ="User Name Cant be empty")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password Cant be empty")]
    public string UserPassword { get; set; }
}
