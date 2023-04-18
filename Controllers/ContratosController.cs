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
            return View();
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contratos/Edit/5
        public ActionResult Editar(int id)
        {
            return View();
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contratos/Delete/5
        public ActionResult Eliminar(int id)
        {
            return View();
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}