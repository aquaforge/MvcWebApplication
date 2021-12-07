using System.ComponentModel.DataAnnotations;

namespace AquaG.TasksMVC.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан Email")] [EmailAddress] public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")] [DataType(DataType.Password)] [MaxLength(50)] public string Password { get; set; }

        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}