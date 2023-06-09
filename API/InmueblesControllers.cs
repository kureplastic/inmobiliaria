using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

/*
metodos necesarios:
	-ObtenerPropiedades(propietario)
	-ObtenerInmueble(id)
	-ActualizarInmueble(inmueble)
*/
namespace inmobiliaria.API
{
	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class InmueblesController : Controller
	{
		private readonly DataContext contexto;

		public InmueblesController(DataContext contexto)
		{
			this.contexto = contexto;
		}

		// GET: api/<controller>/ObtenerPorPropietario
		[HttpGet("ObtenerPorPropietario")]
		public async Task<IActionResult> ObtenerPorPropietario()
		{
			try
			{
				var usuario = User.Identity.Name;
				return Ok(contexto.Inmuebles.Include(e => e.propietario).Where(e => e.propietario.Email == usuario));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		
		// GET api/<controller>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			try
			{
				var usuario = User.Identity.Name;
				return Ok(contexto.Inmuebles.Include(e => e.propietario).Where(e => e.propietario.Email == usuario).Single(e => e.Id == id));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT api/<controller>/ModficarDisponibilidad/{id}
        [HttpPut("ModificarDisponibilidad/{id}")]
        public IActionResult ModificarDisponibilidad(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                var inmuebleModificar = contexto.Inmuebles.Include(inmueble => inmueble.propietario).Where(inmueble => inmueble.propietario.Email == usuario).Single(inmueble => inmueble.Id == id);
				inmuebleModificar.Estado = !inmuebleModificar.Estado;
				contexto.SaveChanges();
				return Ok(inmuebleModificar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
	}
}