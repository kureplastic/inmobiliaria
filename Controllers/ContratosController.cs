using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria.Controllers
{
    [Authorize]
    public class ContratosController : Controller
    {
        private readonly RepositorioContrato repositorioContrato;
        private readonly RepositorioInmueble repositorioInmueble;
        private readonly RepositorioPropietario repositorioPropietario;
        private readonly RepositorioInquilino repositorioInquilino;
        public ContratosController()
        {
            repositorioContrato = new RepositorioContrato();
            repositorioInmueble = new RepositorioInmueble();
            repositorioPropietario = new RepositorioPropietario();
            repositorioInquilino = new RepositorioInquilino();
        }
        // GET: Contratos
        public ActionResult Index()
        {
            List<Contrato> Contratos = repositorioContrato.ObtenerContratos();
            ViewData["Estado"] = TempData["Estado"];
            ViewData["Mensaje"] = TempData["Mensaje"];
            return View(Contratos);
        }

        // GET: Contratos/Details/5
        public ActionResult Detalles(int id)
        {
            try
            {
                var modelo = repositorioContrato.ObtenerContrato(id);
                if (modelo == null)
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El contrato solicitado no existe";
                    return RedirectToAction("Index");
                }
                return View(modelo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: Contratos/Create
        public ActionResult Registrar()
        {
            List<Inmueble> Inmuebles = repositorioInmueble.ObtenerInmuebles();
            List<Propietario> Propietarios = repositorioPropietario.ObtenerPropietarios();
            List<Inquilino> Inquilinos = repositorioInquilino.ObtenerInquilinos();
            ViewData["Estado"] = TempData["Estado"];
            ViewData["Mensaje"] = TempData["Mensaje"];  
            ViewData["Inmuebles"] = Inmuebles;
            ViewData["Propietarios"] = Propietarios;
            ViewData["Inquilinos"] = Inquilinos;
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(Contrato contrato)
        {
            try
            {
                // TODO: Add insert logic here
                Propietario p = repositorioInmueble.ObtenerInmueble(contrato.InmuebleId).propietario;
                contrato.PropietarioId = p.Id;
                if (ValidarFechasContrato(contrato))
                {
                    int res = repositorioContrato.RegistrarContrato(contrato);
                    if (res > 0)
                    {
                        TempData["Estado"] = true;
                        TempData["Mensaje"] = "El contrato se registro correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Estado"] = false;
                        TempData["Mensaje"] = "El contrato no se registro correctamente";
                        return View(contrato);
                    }
                }else
                {
                    TempData["Estado"] = false;
                    return RedirectToAction(nameof(Registrar), contrato);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Contratos/Edit/5
        public ActionResult Editar(int id)
        {
            try
            {
                var modelo = repositorioContrato.ObtenerContrato(id);
                if (modelo == null)
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El contrato solicitado no existe";
                    return RedirectToAction("Index");
                }
                ViewData["Inquilinos"] = repositorioInquilino.ObtenerInquilinos();
                return View(modelo);
            }
            catch
            {
                return View();
            }
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Contrato contrato)
        {
            int res = -1;
            try
            {
                res = repositorioContrato.ActualizarContrato(contrato);
                if (res > 0)
                {
                    TempData["Estado"] = true;
                    TempData["Mensaje"] = "El contrato se actualizo correctamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El contrato no se pudo actualizar";
                    return View(contrato);
                }
            }
            catch (Exception ex)
            {
                TempData["Estado"] = false;
                TempData["Mensaje"] = "Error al actualizar el contrato" + ex;
                return View(contrato);
            }
        }

        // GET: Contratos/Delete/5
        [Authorize (Policy = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                var modelo = repositorioContrato.ObtenerContrato(id);
                if (modelo == null)
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El contrato solicitado no existe";
                    return RedirectToAction("Index");
                }
                return View(modelo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // POST: Contratos/Delete/5
         [Authorize (Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, Contrato contrato)
        {
            int res = -1;
            try
            {
                // TODO: Add delete logic here
                res = repositorioContrato.EliminarContrato(id);
                if (res > 0)
                {
                    TempData["Estado"] = true;
                    TempData["Mensaje"] = "El contrato se elimino correctamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El contrato no se pudo eliminar";
                    return View(contrato);
                }
            }
            catch
            {
                return View();
            }
        }

        public Boolean ValidarFechasContrato(Contrato contrato)
        {
            var fechaActual = DateTime.Now;
            var contratos = repositorioContrato.ObtenerContratosPorInmueble(contrato.InmuebleId);

            if (DateTime.Compare(contrato.FechaInicio, fechaActual) < 0 || DateTime.Compare(contrato.FechaFin, fechaActual) < 0)
            {
                TempData["Mensaje"] = "Las fechas no pueden ser menores a la fecha actual";
                return false;
            }
            if(DateTime.Compare(contrato.FechaInicio, contrato.FechaFin) > 0){
                TempData["Mensaje"] = "Las fecha de inicio del contrato no pueden ser mayor que la fecha final del contrato";
                return false;
            }
            foreach(var c in contratos){//que la fecha de inicio del contrato a crear no este dentro de las fecha de inicio y de fin de ningun otro contrato.
                if(c.FechaInicio <= contrato.FechaInicio && c.FechaFin >= contrato.FechaInicio){
                    TempData["Mensaje"] = "esta fecha se encuentra ocupada por otro contrato";
                    return false;
                }//que la fecha de fin del contrato a crear no este dentro de las fechas de inicio y de fin de ningun otro contrato.
                if(c.FechaInicio <= contrato.FechaFin && c.FechaFin >= contrato.FechaFin){
                    TempData["Mensaje"] = "esta fecha se encuentra ocupada por otro contrato";
                    return false;
                }
            }
            return true;
        }
    }
}