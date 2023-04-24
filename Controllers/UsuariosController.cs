using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using inmobiliaria.Models;

namespace inmobiliaria.Controllers
{

    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private static string MISALT = "Este es mi salt para hashear passwords, muy largo salt!";
        private readonly RepositorioUsuario repositorioUsuario;

        public UsuariosController(IWebHostEnvironment environment)
        {
            this.environment = environment;
            repositorioUsuario = new RepositorioUsuario();
        }
        [Authorize(Policy = "Administrador")]
        // GET: Usuarios
        public ActionResult Index()
        {
            try
            {
                var usuarios = repositorioUsuario.ObtenerUsuarios();
                ViewData["Estado"] = TempData["Estado"];
                ViewData["Mensaje"] = TempData["Mensaje"];
                return View(usuarios);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        // GET: Usuarios/Perfil
        public ActionResult Perfil()
        {
            ViewData["Estado"] = TempData["Estado"];
            ViewData["Mensaje"] = TempData["Mensaje"]; 
            try
            {
                var modelo = repositorioUsuario.ObtenerUsuarioPorEmail(User.Identity.Name);
                if (modelo == null)
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El Usuario no existe";
                    return View("Index", "Home");
                }
                return View("Detalles", modelo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Authorize(Policy = "Administrador")]
        // GET: Usuarios/Detalles/5
        public ActionResult Detalles(int id)
        {
            try
            {
                var modelo = repositorioUsuario.ObtenerUsuario(id);
                if (modelo == null)
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El Usuario no existe";
                    return View("Index");
                }
                return View(modelo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        [Authorize(Policy = "Administrador")]
        // GET: Usuarios/Create
        public ActionResult Registrar()
        {
            ViewData["roles"] = Usuario.ObtenerRoles();
            return View();
        }

        [Authorize(Policy = "Administrador")]
        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(Usuario usuario)
        {
            if (repositorioUsuario.ObtenerUsuarioPorEmail(usuario.Email) != null)
            {
                TempData["Estado"] = false;
                TempData["Mensaje"] = "El usuario ya se encuentra registrado con dicho email";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                try
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                    password: usuario.Clave,
                                    salt: System.Text.Encoding.ASCII.GetBytes(MISALT),
                                    prf: KeyDerivationPrf.HMACSHA1,
                                    iterationCount: 1000,
                                    numBytesRequested: 256 / 8));
                    usuario.Clave = hashed;
                    int res = repositorioUsuario.RegistrarUsuario(usuario);
                    if (res > 0)
                    {
                        TempData["Estado"] = true;
                        TempData["Mensaje"] = "Usuario registrado correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Estado"] = false;
                        TempData["Mensaje"] = "No se pudo registrar el usuario";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    ViewBag.Roles = Usuario.ObtenerRoles();
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "No se pudo registrar el usuario: " + ex.Message;
                    return RedirectToAction(nameof(Index));
                }
            }

        }
        [Authorize]
        // GET: Usuarios/Edicion
        public ActionResult Edicion()
        {
             ViewData["Estado"] = TempData["Estado"];
            ViewData["Mensaje"] = TempData["Mensaje"];
            try
            {
                var modelo = repositorioUsuario.ObtenerUsuarioPorEmail(User.Identity.Name);
                if (modelo == null)
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El Usuario no existe";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("Editar", modelo);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [Authorize(Policy = "Administrador")]
        // GET: Usuarios/Edicion/5
        public ActionResult Editar(int id)
        {
            ViewData["Estado"] = TempData["Estado"];
            ViewData["Mensaje"] = TempData["Mensaje"];             
            try
            {
                var modelo = repositorioUsuario.ObtenerUsuario(id);
                if (modelo == null)
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El Usuario no existe";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(modelo);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        // POST: Usuarios/Editar/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Usuario usuario)
        {
            int res = -1;
            try
            {
                var user = repositorioUsuario.ObtenerUsuario(id);
                user.Nombre = usuario.Nombre;
                user.Apellido = usuario.Apellido;
                user.Email = usuario.Email;
                if(User.IsInRole("Administrador")){user.Rol = usuario.Rol;}
                res = repositorioUsuario.ActualizarUsuario(user);
                if (res > 0)
                {
                    TempData["Estado"] = true;
                    TempData["Mensaje"] = "Usuario modificado correctamente";
                    return User.IsInRole("Administrador") ?  RedirectToAction("Index") : RedirectToAction("Perfil");
                }
                else
                {
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "Error al modificar el usuario";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                throw;
            }
        }

        // POST: Usuarios/ModificarPassword
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarPassword(IFormCollection form){
            var usuario = repositorioUsuario.ObtenerUsuario(Convert.ToInt32(form["Id"]));
            if(form["NuevaPass"] != form["RepetirPass"]){
                TempData["Estado"] = false;
                TempData["Mensaje"] = "Error, las contraseñas no coinciden";
                return RedirectToAction("Editar",usuario);
            }
            else{
                //hashear la clave vieja que viene
                string hashedViejaPass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: form["ViejaPass"],
                        salt: System.Text.Encoding.ASCII.GetBytes(MISALT),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                //compararla con la clave hasehada que poseo en usuario.Clave
                if(hashedViejaPass == usuario.Clave){
                    //si coninciden realizar el hasheo de nuevapass
                    string hashedNuevaPass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: form["NuevaPass"],
                        salt: System.Text.Encoding.ASCII.GetBytes(MISALT),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    // enviar la pass a la DB
                    try{
                        usuario.Clave = hashedNuevaPass;
                        int res = repositorioUsuario.ActualizarUsuario(usuario);
                        if (res > 0){
                            TempData["Estado"] = true;
                            TempData["Mensaje"] = "Contraseña modificada correctamente";
                            if(User.IsInRole("Administrador")){
                                return RedirectToAction(nameof(Index));
                            }else{
                                return RedirectToAction("Perfil", "Usuarios");
                            }
                        }else{
                            TempData["Estado"] = false;
                            TempData["Mensaje"] = "Error al modificar la contraseña";
                            return View("Editar",usuario);
                        }
                    }catch(Exception ex){
                        throw;
                    }
                }
                else{
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "Error, contraseña incorrecta";
                    return View("Editar",usuario);
                } 
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarFoto(IFormCollection form){
            var usuario = repositorioUsuario.ObtenerUsuario(Convert.ToInt32(form["Id"]));
            if(form.Files.Count > 0){
                try{
                    var random = Guid.NewGuid();
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
                    string fileName = random + Path.GetExtension(form.Files["AvatarFile"].FileName);//falta
                    string pathCompleto = Path.Combine(path, fileName);
                    usuario.RutaAvatar = Path.Combine("/Uploads", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
					{
						form.Files["AvatarFile"].CopyTo(stream);//aca tiene que ser el form file
					}
					int res = repositorioUsuario.ActualizarUsuario(usuario);
                    if (res > 0){
                        TempData["Estado"] = true;
                        TempData["Mensaje"] = "Avatar modificado correctamente";
                        if(User.IsInRole("Administrador")){
                                return RedirectToAction(nameof(Index));
                            }else{
                                return RedirectToAction("Perfil", "Usuarios");
                            }
                    }else{
                        TempData["Estado"] = false;
                        TempData["Mensaje"] = "Error al modificar el avatar";
                        return RedirectToAction("Editar",usuario);
                    }
                }
                catch (Exception ex){
                    throw;
                }
            }
            else{
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "No selecciono ningun archivo";
                    return (User.IsInRole("Administrador")) ? RedirectToAction("Editar",usuario) : RedirectToAction("Edicion",usuario);
            }
        }

        [Authorize(Policy="Administrador")]
        // GET: Usuarios/Eliminar/5
        public ActionResult Eliminar(int id)
        {
            ViewData["Estado"] = TempData["Estado"];
            ViewData["Mensaje"] = TempData["Mensaje"]; 
            try{
                var usuario = repositorioUsuario.ObtenerUsuario(id);
                if(usuario == null){
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "El Usuario no existe";
                    return RedirectToAction(nameof(Index));
                }
                else{
                    return View(usuario);
                }
            }catch(Exception ex){
                throw;
            }
            
        }

        // POST: Usuarios/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy="Administrador")]
        public ActionResult Eliminar(int id, Usuario usuario)
        {
            int res = -1;
            try
            {
                var user = repositorioUsuario.ObtenerUsuario(id);
                if(user.RutaAvatar != ""){
                    var ruta = Path.Combine(environment.WebRootPath, "\\", user.RutaAvatar.Split('/').Last());
				if (System.IO.File.Exists(ruta))
					System.IO.File.Delete(ruta);
                }
                // TODO: Add delete logic here
                res = repositorioUsuario.EliminarUsuario(id);
                if (res > 0){
                    TempData["Estado"] = true;
                    TempData["Mensaje"] = "Usuario eliminado correctamente";
                    return RedirectToAction(nameof(Index));
                }else{
                    TempData["Estado"] = false;
                    TempData["Mensaje"] = "Error al eliminar el usuario";
                    return View(usuario);
                }
            }
            catch
            {
                throw;
            }
        }

        [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        // POST: Usuarios/Login/
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(MISALT),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var user = repositorioUsuario.ObtenerUsuarioPorEmail(login.Usuario);
                    if (user == null || user.Clave != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }
                    else
                    {


                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Nombre" , user.Nombre + " " + user.Apellido),
                        new Claim(ClaimTypes.Role, user.RolNombre),
                    };

                        var claimsIdentity = new ClaimsIdentity(
                                claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity));
                        TempData.Remove("returnUrl");
                        return Redirect(returnUrl);
                    }
                }
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: /salir
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: /Restringido
        [AllowAnonymous]
        public ActionResult Restringido()
        {
            return View();
        }
    }
}