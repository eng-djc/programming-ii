using System.ComponentModel.DataAnnotations;

namespace Zara.Models;

public class LoginFuncionario
{
    [Required(ErrorMessage = "Ingrese el usuario.")]
    [MaxLength(100)]
    public string Usuario { get; set; } = "";

    [Required(ErrorMessage = "Ingrese la clave.")]
    [DataType(DataType.Password)]
    public string Clave { get; set; } = "";
}
