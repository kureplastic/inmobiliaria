using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;

namespace inmobiliaria.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RepositorioPropietario repositorioPropietario;
    private readonly RepositorioInquilino repositorioInquilino;
    private readonly RepositorioInmueble repositorioInmueble;
    private readonly RepositorioContrato repositorioContrato;


    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        repositorioPropietario = new RepositorioPropietario();
        repositorioInquilino = new RepositorioInquilino();
        repositorioInmueble = new RepositorioInmueble();
        repositorioContrato = new RepositorioContrato();
    }

    public IActionResult Index()
    {
        ViewData["propietarios"] = repositorioPropietario.ObtenerPropietarios();
        ViewData["inquilinos"] = repositorioInquilino.ObtenerInquilinos();
        ViewData["inmuebles"] = repositorioInmueble.ObtenerInmuebles();
        ViewData["contratos"] = repositorioContrato.ObtenerContratos();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
