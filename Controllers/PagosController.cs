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
    public class PagosController : Controller
    {
        private readonly RepositorioPago repositorioPago;
        private readonly RepositorioContrato repositorioContrato;
        public PagosController()
        {
            repositorioPago = new RepositorioPago();
            repositorioContrato = new RepositorioContrato();
        }

        // GET: Pagos
        public ActionResult Index()
        {
            List<Pago> pagos = repositorioPago.ObtenerPagos();
            ViewData["Estado"] = TempData["Estado"];
            ViewData["Mensaje"] = TempData["Mensaje"];
            ViewData["idContrato"] = 0;
            return View(pagos);
        }

        // GET: Pagos/Details/5
        public ActionResult Detalles(int id)
        {
            try{
                var modelo = repositorioPago.ObtenerPago(id);
                if(modelo == null){
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El Pago no existe";
                    return RedirectToAction(nameof(Index));
                }
                return View(modelo);
            }catch{
                throw;
            }
        }

        // GET: Pagos/Create
        public ActionResult Registrar(int id)
        {
            try{
                Contrato contrato = repositorioContrato.ObtenerContrato(id);
                if(contrato == null){
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El Contrato solicitado no existe por lo que no se pueden agregar Pagos";
                    return RedirectToAction(nameof(Index),"Contratos");
                }
                else{
                    ViewData["contrato"] = contrato;
                    return View();
                }

            }catch{
                throw;
            }
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(Pago pago)
        {
            try
            {
                // TODO: Add insert logic here
                int res = repositorioPago.RegistrarPago(pago);
                if(res > 0){
                    TempData["Estado"] = true;
                    TempData["Mensaje"] = "El Pago se registro correctamente";
                    return RedirectToAction("Index");
                }
                else{
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El Pago no se pudo registrar";
                    return View(pago);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: Pagos/Edit/5
        public ActionResult Editar(int id)
        {
            try{
                var modelo = repositorioPago.ObtenerPago(id);
                if(modelo == null){
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El Pago no existe";
                    return RedirectToAction(nameof(Index));
                }
                return View(modelo);
            }catch{
                throw;
            }
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Pago pago)
        {
            int res = -1;
            try
            {
                // TODO: Add update logic here
                res = repositorioPago.ActualizarPago(pago);
                if(res > 0){
                    TempData["Estado"] = true;
                    TempData["Mensaje"] = "El Pago se actualizo correctamente";
                    return RedirectToAction(nameof(Index));
                }
                else{
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El Pago no se pudo actualizar";
                    return View(pago);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Pagos/Delete/5
         [Authorize (Policy = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                var modelo = repositorioPago.ObtenerPago(id);
                if (modelo == null)
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El Pago solicitado no existe";
                    return RedirectToAction("Index");
                }
                return View(modelo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // POST: Pagos/Delete/5
         [Authorize (Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, Pago Pago)
        {
            int res = -1;
            try
            {
                // TODO: Add delete logic here
                res = repositorioPago.EliminarPago(id);
                if (res > 0)
                {
                    TempData["Estado"] = true;
                    TempData["Mensaje"] = "El Pago se elimino correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El Pago no se pudo eliminar";    
                    return View(Pago);
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult PagosPorContrato(int id)
        {
            try
            {
                var modelo = repositorioContrato.ObtenerContrato(id);
                if (modelo == null){
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "No se pudieron obtener los pagos del contrato deseado";
                    return RedirectToAction("Index","Contratos");
                }
                ViewData["idContrato"] = id;
                var pagos = repositorioPago.ObtenerPagosPorContrato(id);
                return View("Index", pagos);
            }catch(Exception ex)
            {
                throw;
            }
        }
    }
}