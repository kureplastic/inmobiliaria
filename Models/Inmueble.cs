namespace inmobiliaria.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Inmueble {
    //* Un Inmueble debe tener: dirección, tipo(local, depósito, casa, departamento, etc.), cantidad de ambientes y precio del mismo. Además debe tener un estado, el cual establece si el inmueble puede ser ofertado o se encuentra deshabilitado.
    [Display(Name = "Código")]
    public int Id { get; set; }
    [Required]
    public string Direccion { get; set; }
    [Required]
    public string Tipo { get; set; }
    [Required]
    public int Ambientes { get; set; }
    public decimal Precio { get; set; } 
    public Boolean Estado { get; set; }
    [Required]
    [Display(Name = "Dueño")]
	public int PropietarioId { get; set; }
    [ForeignKey(nameof(PropietarioId))]
    public Propietario propietario { get; set; }


    public Inmueble (int id, string direccion, string tipo, int ambientes, decimal precio, Boolean estado, Propietario propietario)
    {
        Id = id;
        Direccion = direccion;
        Tipo = tipo;
        Ambientes = ambientes;
        Precio = precio;
        Estado = estado;
        this.propietario = propietario;
    }

    public Inmueble()
    {
        Id = 0;
        Direccion = "";
        Tipo = "";
        Ambientes = 0;
        Precio = 0;
        Estado = false;
        propietario = new Propietario();
    }

    public override string ToString() => $"{Direccion} ({Tipo}) ";
}
