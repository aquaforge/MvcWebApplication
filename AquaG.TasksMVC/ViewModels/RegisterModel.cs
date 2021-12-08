using System.ComponentModel.DataAnnotations;

namespace AquaG.TasksMVC.ViewModels
{
    public class RegisterModel : LoginModel
    {
        [DataType(DataType.Password)] [MaxLength(50)] [Compare("Password", ErrorMessage = "Повтор пароля не совпадает")] public string ConfirmPassword { get; set; }
    }
}
