using System.ComponentModel.DataAnnotations;

namespace SIGAA.ViewModels
{
    public class LoginViewModel
    {
        public LoginViewModel() {
            this.EmailUtepsa = "";
            this.Contrasena = "";        
         }

        [Required(ErrorMessage = "Debe ingresar valores al campo {0} porque es Obligatorio.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "La dirección de email no es válido")]
        [Display(Name = "Email :")]
        public string EmailUtepsa { set; get; }

        [Required(ErrorMessage = "Debe ingresar valores al campo {0} porque es Obligatorio.")]
        [StringLength(30, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña :")]
        public string Contrasena { get; set; }
    }
}