namespace inmobiliaria.Models;
using System.ComponentModel.DataAnnotations;

public class Inquilino{
    //Inquilino debe tener: DNI, nombre, apellido, lugar de trabajo, nombre garante, DNI del garante y telefono, mail de ambos
    [Key]
    [Display(Name = "Código")]
    public int Id { get; set; }
    [Required]
    [Display(Name = "DNI")]
    public string Dni { get; set; }
    [Required]
    public string Nombre { get; set; }
    [Required]
    public string Apellido { get; set; }
    [Display(Name = "Lugar de Trabajo")]
    public string LugarTrabajo { get; set; }
    [Required]
    public string Telefono { get; set; }
    [Display(Name = "Correo")]
    public string Email { get; set; }
    [Required]
    [Display(Name = "Garante")]
    public string NombreGarante { get; set; }
    [Required]
    [Display(Name = "DNI Garante")]
    public string DniGarante { get; set; }
    [Required]
    [Display(Name = "Tel. Garante")]
    public string TelefonoGarante { get; set; }
    [Display(Name = "Correo Garante")]
    public string EmailGarante { get; set; }

    //constructor
    public Inquilino(string dni, string nombre, string apellido, string lugarTrabajo, string telefono, string email, string nombreGarante, string dniGarante, string telefonoGarante, string emailGarante)
    {
        Dni = dni;
        Nombre = nombre;
        Apellido = apellido;
        LugarTrabajo = lugarTrabajo;
        Telefono = telefono;
        Email = email;
        NombreGarante = nombreGarante;
        DniGarante = dniGarante;
        TelefonoGarante = telefonoGarante;
        EmailGarante = emailGarante;
    }
    public Inquilino()
    {
        Dni = "";
        Nombre = "";
        Apellido = "";
        LugarTrabajo = "";
        Telefono = "";
        Email = "";
        NombreGarante = "";
        DniGarante = "";
        TelefonoGarante = "";
        EmailGarante = "";
    }
    
    public override string ToString() => $"{Apellido} {Nombre} ({Dni})";
}