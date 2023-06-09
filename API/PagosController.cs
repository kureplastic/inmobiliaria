using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria.Models;

/*
metodos necesarios
    -ObtenerPagos(Contrato)
*/

namespace inmobiliaria.API
{
    [Route("API/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PagosController : Controller
    {
        private readonly DataContext contexto;
        public PagosController(DataContext contexto)
		{
			this.contexto = contexto;
		}

        [HttpGet("ObtenerPagos/{id}")]
		public async Task<IActionResult> ObtenerPagos( int id)
		{
			try
			{
				var usuario = User.Identity.Name;
				return Ok(contexto.Pagos.Where(e => e.ContratoId == id && e.contrato.propietario.Email == usuario));
                }
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
    }
}