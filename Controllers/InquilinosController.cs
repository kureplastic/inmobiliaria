using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;

namespace inmobiliaria.Controllers
{
    public class InquilinosController : Controller
    {
        private readonly RepositorioInquilino repositorioInquilino;

        public InquilinosController()
        {
            repositorioInquilino = new RepositorioInquilino();
        }
        // GET: Inquilinos
        public ActionResult Index()
        {
            try
            {
                List<Inquilino> Inquilinos = repositorioInquilino.ObtenerInquilinos();
                ViewData["Estado"] = TempData["Estado"];
                ViewData["Mensaje"] = TempData["Mensaje"];
                return View(Inquilinos);
            }
            catch (Exception ex)
            {
                throw;//error, enviar mensaje, concatenar error, estado false.
            }
        }

        // GET: Inquilinos/Details/5
        public ActionResult Detalles(int id)
        {
            try
            {
                var modelo = repositorioInquilino.ObtenerInquilino(id);
                return View(modelo);
            }
            catch (Exception ex)
            {
                throw;//error, enviar mensaje, concatenar error, estado false.
            }
        }

        // GET: Inquilinos/Create
        public ActionResult Registrar()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(Inquilino inquilino)
        {
            try
            {
                // TODO: Add insert logic here
                int res = repositorioInquilino.RegistrarInquilino(inquilino);
                if (res > 0)
                {
                    TempData["Estado"] = true;
                    TempData["Mensaje"] = "Se registro correctamente al inquilino";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "Error al registrar al inquilino";
                    return View(inquilino);
                }
            }
            catch
            {
                throw;
            }
        }

        // GET: Inquilinos/Edit/5
        public ActionResult Editar(int id)
        {

            try{
                var modelo = repositorioInquilino.ObtenerInquilino(id);
                if(modelo == null){
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "No se pudo obtener el Inquilino deseado";
                    return RedirectToAction("Index");
                }
                return View(modelo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Inquilino inquilino)
        {
            Inquilino Inquilino = new Inquilino();
            try
            {
                repositorioInquilino.ActualizarIquilino(inquilino);
                TempData["Estado"] = true;
                TempData["Mensaje"] = "Se actualizo correctamente el Inquilino";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Estado"] = false;
                TempData["Mensaje"] = "Error al actualizar al inquilino";
                return View(inquilino);
            }
        }

        // GET: Inquilinos/Delete/5
        public ActionResult Eliminar(int id)
        {
            try{
                var modelo = repositorioInquilino.ObtenerInquilino(id);
                if(modelo == null){
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "No se pudo obtener el Inquilino deseado";
                    return RedirectToAction("Index");
                }
                return View(modelo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // POST: Inquilinos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, Inquilino inquilino)
        {
            try
            {
                repositorioInquilino.EliminarInquilino(id);
                TempData["Estado"] = true;
                TempData["Mensaje"] = "Se elimino correctamente el Inquilino";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult Buscar(IFormCollection collection){
            List<Inquilino> inquilinos = new List<Inquilino>();
            if(collection["Inquilinos"] != ""){
                int id = int.Parse(collection["Inquilinos"]);
                inquilinos.Add(repositorioInquilino.ObtenerInquilino(id));
                ViewData["buscar"] = "buscar";
                return View("Index", inquilinos);
            }else{
                TempData["Estado"] = false;
                TempData["Mensaje"] = "Debe seleccionar un Inquilino";
                return RedirectToAction("Index");
            }
        }
    } 
}