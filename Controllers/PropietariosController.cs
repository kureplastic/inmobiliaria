using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria.Controllers;

public class PropietariosController : Controller
{
    private readonly RepositorioPropietario repositorioPropietario;
    public PropietariosController()
    {
        repositorioPropietario = new RepositorioPropietario();
    }
    public IActionResult Index()
    {
        List<Propietario> Propietarios = repositorioPropietario.ObtenerPropietarios();
        ViewData["Estado"] = TempData["Estado"];
        ViewData["Mensaje"] = TempData["Mensaje"];
        return View(Propietarios);
    }

    public ActionResult Detalles(int id)
    {
        try
        {
            var modelo = repositorioPropietario.ObtenerPropietario(id);
            return View(modelo);
        }
        catch (Exception ex)
        {//poner breakpoints para detectar errores
            throw;
        }
    }


    public IActionResult Registrar()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {//poner breakpoints para detectar errores
            throw;
        }
    }
    [HttpPost]
    public IActionResult Registrar(Propietario propietario)
    {
        int res = repositorioPropietario.RegistrarPropietario(propietario);
        if (res > 0)
        {
            TempData["Estado"] = true;
            TempData["Mensaje"] = "Se registro correctamente al propietario";
            return RedirectToAction("Index");
        }
        else
        {
            return View();
        }
    }

    // GET: Propietarios/Eliminar/5
    [Authorize(Policy = "Administrador")]
    public ActionResult Eliminar(int id)
    {
        try
        {
            var modelo = repositorioPropietario.ObtenerPropietario(id);
            return View(modelo);
        }
        catch (Exception ex)
        {//poner breakpoints para detectar errores
            throw;
        }
    }

    // POST: Propietario/Delete/5
    [Authorize(Policy = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Eliminar(int id, Propietario propietario)
    {
        try
        {
            repositorioPropietario.EliminarPropietario(id);
            TempData["Estado"] = true;
            TempData["Mensaje"] = "Se elimino correctamente el propietario";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {//poner breakpoints para detectar errores
            throw;
        }
    }

    public ActionResult Editar(int id)
    {
        try
        {
            var modelo = repositorioPropietario.ObtenerPropietario(id);
            return View(modelo);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(int id, Propietario propietario)
    {
        Propietario p = null;

        try
        {
            p = repositorioPropietario.ObtenerPropietario(id);
            p.Nombre = propietario.Nombre;
            p.Apellido = propietario.Apellido;
            p.Dni = propietario.Dni;
            p.Telefono = propietario.Telefono;
            p.Email = propietario.Email;
            repositorioPropietario.ActualizarPropietario(p);
            TempData["Estado"] = true;
            TempData["Mensaje"] = "Se actualizo correctamente el propietario";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public ActionResult Buscar(IFormCollection collection){
            List<Propietario> propietarios = new List<Propietario>();
            if(collection["Propietarios"] != ""){
                int id = int.Parse(collection["Propietarios"]);
                propietarios.Add(repositorioPropietario.ObtenerPropietario(id));
                ViewData["buscar"] = "buscar";
                return View("Index", propietarios);
            }else{
                TempData["Estado"] = false;
                TempData["Mensaje"] = "Debe seleccionar un Propietario";
                return RedirectToAction("Index");
            }
        }
}



