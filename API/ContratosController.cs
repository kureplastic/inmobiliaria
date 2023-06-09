using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;

/*
metodos necesarios
    -ObtenerContratoVigente(inmueble)
    -ObtenerPropiedadesAlquiladas(propietario)
	-ObtenerInquilino(Inmueble)
*/

namespace inmobiliaria.API
{
    [Route("API/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContratosController : Controller
    {
        private readonly DataContext contexto;

		public ContratosController(DataContext contexto)
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
				return Ok(contexto.Contratos
                    .Include(e => e.propietario).Where(e => e.propietario.Email == usuario)
                    .Include(e => e.inquilino).Where(e => e.inquilino.Id == e.InquilinoId)
                    .Include(e => e.inmueble).Where(e => e.inmueble.Id == e.InmuebleId));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        [HttpGet("ObtenerInmueblesAlquilados")]
		public async Task<IActionResult> ObtenerInmueblesAlquilados()
		{
			try
			{
				var usuario = User.Identity.Name;
				var contratos = contexto.Contratos.Include(e => e.inmueble.propietario).Where(e => e.propietario.Email == usuario && (e.FechaInicio <= DateTime.Today && DateTime.Today <= e.FechaFin ));
                var inmuebles = contratos.Select(e => e.inmueble).ToList();
                return Ok(inmuebles);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        [HttpGet("ObtenerContratoVigente/{id}")]
		public async Task<IActionResult> ObtenerContratoVigente(int id)
		{
			try
			{
				var usuario = User.Identity.Name;
				var contrato = contexto.Contratos
				.Include(e => e.propietario).Where(e => e.propietario.Email == usuario && (e.FechaInicio <= DateTime.Today && DateTime.Today <= e.FechaFin ))
				.Include(e => e.inquilino).Where(e => e.inquilino.Id == e.InquilinoId)
				.Include(e => e.inmueble).Where(e => e.inmueble.Id == e.InmuebleId)
				.OrderBy(e => e.FechaInicio)
				.LastOrDefault(e => e.InmuebleId == id);
                return Ok(contrato);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("ObtenerInquilino/{id}")]
		public async Task<IActionResult> ObtenerInquilino(int id)
		{
			try
			{
				var usuario = User.Identity.Name;
				var contrato = contexto.Contratos
				.Include(e => e.propietario).Where(e => e.propietario.Email == usuario)
				.Include(e => e.inquilino).Where(e => e.inquilino.Id == e.InquilinoId)
				.Include(e => e.inmueble).Where(e => e.inmueble.Id == e.InmuebleId)
				.OrderBy(e => e.FechaInicio)
				.LastOrDefault(e => e.InmuebleId == id);
                return Ok(contrato.inquilino);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


    }

}