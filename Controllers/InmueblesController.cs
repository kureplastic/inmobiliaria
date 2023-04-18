using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;

namespace inmobiliaria.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly RepositorioInmueble repositorioInmueble;
        private readonly RepositorioPropietario repositorioPropietario;

        public InmueblesController()
        {
            repositorioInmueble = new RepositorioInmueble();
            repositorioPropietario = new RepositorioPropietario();
        }
        // GET: Inmuebles
        public ActionResult Index()
        {
            List<Inmueble> Inmuebles = repositorioInmueble.ObtenerInmuebles();
            ViewData["Estado"] = TempData["Estado"];
            ViewData["Mensaje"] = TempData["Mensaje"];
            return View(Inmuebles);
        }

        // GET: Inmuebles/Details/5
        public ActionResult Detalles(int id)
        {
            try{
                var modelo = repositorioInmueble.ObtenerInmueble(id);
                return View(modelo);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // GET: Inmuebles/Create
        public ActionResult Registrar()
        {
            try{
                ViewData["Propietarios"] = repositorioPropietario.ObtenerPropietarios();
                return View();
            }
            catch(Exception ex)
            {
                throw;
            }            
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(Inmueble inmueble)
        {
            try
            {
                // TODO: Add insert logic here
                int res = repositorioInmueble.RegistrarInmueble(inmueble);
                if (res > 0)
                {
                    TempData["Estado"] = true;
                    TempData["Mensaje"] = "Inmueble registrado correctamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "No se pudo registrar el inmueble";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Inmuebles/Edit/5
        public ActionResult Editar(int id)
        {
            try{
                var modelo = repositorioInmueble.ObtenerInmueble(id);
                if(modelo == null){
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "No se pudo obtener el Inmueble deseado";
                    return RedirectToAction("Index");
                }
                ViewData["Propietarios"] = repositorioPropietario.ObtenerPropietarios();
                return View(modelo);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Inmueble inmueble)
        {
            int res = -1;
            try
            {
                res = repositorioInmueble.ActualizarInmueble(inmueble);
                if(res > 0)
                {
                    TempData["Estado"] = true;
                    TempData["Mensaje"] = "Inmueble actualizado correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "No se pudo actualizar el Inmueble";
                    return View(inmueble);
                }
            }
            catch (Exception ex)
            {
               TempData["Estado"] = false;
                TempData["Mensaje"] = "Error al actualizar el Inmueble";
                return View(inmueble);
                 
            }
        }

        // GET: Inmuebles/Delete/5
        public ActionResult Eliminar(int id)
        {
            try{
                var modelo = repositorioInmueble.ObtenerInmueble(id);
                if(modelo == null){
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "No se pudo obtener el Inmueble deseado";
                    return RedirectToAction("Index");
                }
                return View(modelo);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // POST: Inmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, Inquilino inquilino)
        {
            int res = -1;
            try
            {
                // TODO: Add delete logic here
                res = repositorioInmueble.EliminarInmueble(id);
                if(res > 0)
                {
                    TempData["Estado"] = true;
                    TempData["Mensaje"] = "Inmueble eliminado correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "No se pudo eliminar el Inmueble";
                    return View(inquilino);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}