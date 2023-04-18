namespace inmobiliaria.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Contrato{
    //*El Contrato debe tener un inquilino, un propietario, un inmueble, fecha inicio, fecha fin, monto mensual.

    [Key]
    [Display(Name = "Código")]
    public int Id { get; set; }
    [Required]
    [Display(Name = "Inquilino")]
    public int InquilinoId { get; set; }
    [ForeignKey(nameof(InquilinoId))]
    public Inquilino inquilino { get; set; }
    [Required]
    [Display(Name = "Propietario")]
    public int PropietarioId { get; set; }
    [ForeignKey(nameof(PropietarioId))]
    public Propietario propietario { get; set; }
    [Required]
    [Display(Name = "Inmueble")]
    public int InmuebleId { get; set; }
    [ForeignKey(nameof(InmuebleId))]
    public Inmueble inmueble { get; set; }
    [Required]
    [Display(Name = "Inicio de contrato")]
    public DateTime FechaInicio { get; set; }
    [Required]
    [Display(Name = "Caducidad de contrato")]
    public DateTime FechaFin { get; set; }
    [Required]
    [Display(Name = "Monto mensual")]
    public decimal MontoMensual { get; set; }
}