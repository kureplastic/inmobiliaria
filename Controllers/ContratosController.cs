using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;

namespace inmobiliaria.Controllers
{
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
            try{
                var modelo = repositorioContrato.ObtenerContrato(id);
                if(modelo == null){
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El contrato solicitado no existe";
                    return RedirectToAction("Index");
                }
                return View(modelo);
            }catch(Exception ex)
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
            //para el control de las fechas correctas
            //traer todos los contratos por contrato.InmuebleId
            //setear un datetime con fechaActual
            // Control if(contrato.FechaInicio < contrato.FechaFin)
            // control if(contrato.FechaInicio > fechaActual && contrato.FechaFin > fechaActual)
            // para cada contrato habilitadod del listado de contratos,
            //      comparar que tanto fechaInicio como FechaFin de cada uno
            //      no esten entre contrato.FechaInicio y contrato.FechaFin 
            try
            {
                // TODO: Add insert logic here
                int res = repositorioContrato.RegistrarContrato(contrato);
                if(res > 0){
                    TempData["Estado"] = true;
                    TempData["Mensaje"] = "El contrato se registro correctamente";
                    return RedirectToAction(nameof(Index));
                }
                else{
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El contrato no se registro correctamente";
                    return View(contrato);
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
                if(modelo == null){
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
        public ActionResult Editar(int id, Contrato  contrato)
        {
            int res = -1;
            try
            {
                res = repositorioContrato.ActualizarContrato(contrato);
                if(res > 0){
                    TempData["Estado"] = true;
                    TempData["Mensaje"] = "El contrato se actualizo correctamente";
                    return RedirectToAction(nameof(Index));
                }
                else{
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
        public ActionResult Eliminar(int id)
        {
            try{
                var modelo = repositorioContrato.ObtenerContrato(id);
                if(modelo == null){
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El contrato solicitado no existe";
                    return RedirectToAction("Index");
                }
                return View(modelo);
            }catch(Exception ex)
            {
                throw;
            }
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, Contrato contrato)
        {
            int res = -1;
            try
            {
                // TODO: Add delete logic here
                res = repositorioContrato.EliminarContrato(id);
                if(res > 0){
                    TempData["Estado"] = true;
                    TempData["Mensaje"] = "El contrato se elimino correctamente";
                    return RedirectToAction(nameof(Index));
                }
                else{
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
    }
}