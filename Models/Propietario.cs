namespace inmobiliaria.Models;

using System.ComponentModel.DataAnnotations;

public class Propietario
{
  //El Propietario debe tener: DNI, apellido, nombre, teléfono, email.
    [Key]
	[Display(Name = "Código")]
    public int Id { get; set; }
    [Required]
    [Display(Name = "DNI")]
    public string Dni { get; set; }
    [Required]
    public string Apellido { get; set; }
    [Required]
    public string Nombre { get; set; }
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }
    [Required]
    public string Email { get; set; }


  public Propietario(){
        Id = 0;
        Dni = "";
        Apellido = "";
        Nombre = "";
        Telefono = "";
        Email = "";
    }
    
   public override string ToString() => $"{Apellido} {Nombre} ({Dni})";
}