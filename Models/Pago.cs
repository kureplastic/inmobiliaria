namespace inmobiliaria.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Pago {
    //El Pago debería ser una clase, con datos del alquiler (contrato), número de pago, fecha de pago e importe.

    [Key]
    [Display(Name = "Código")]
    public int Id { get; set; }
    [Required]
    [Display(Name = "Código de Contrato")]
    public int ContratoId { get; set; }
    [ForeignKey(nameof(ContratoId))]
    public Contrato contrato { get; set; }
    [Required]
    [Display(Name = "Numero de Pago")]
    public int numPago { get; set; }
    [Required]
    [Display(Name = "Fecha de Pago")]
    public DateTime fechaPago { get; set; }
    [Required]
    [Display(Name = "Importe")]
    public decimal importe { get; set; }
}