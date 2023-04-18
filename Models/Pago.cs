namespace inmobiliaria.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Pago {
    //El Pago debería ser una clase, con datos del alquiler (contrato), número de pago, fecha de pago e importe.

    [Key]
    [Display(Name = "Código")]
    public int Id { get; set; }
    [Required]
    public int ContratoId { get; set; }
    [ForeignKey(nameof(ContratoId))]
    public Contrato contrato { get; set; }
    [Required]
    public int numPago { get; set; }
    [Required]
    public DateTime fechaPago { get; set; }
    [Required]
    public decimal importe { get; set; }
}