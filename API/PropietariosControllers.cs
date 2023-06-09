using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using inmobiliaria.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MimeKit;


/*
metodos necesarios
	-login()
	-obtenerUsuarioActual()
	-ActualizarPerfil(propietario)
*/
namespace inmobiliaria.API
{
	[Route("API/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
	public class PropietariosController : ControllerBase  //
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public PropietariosController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			environment = env;
		}
		// GET: api/<controller>
		[HttpGet]
		public async Task<ActionResult<Propietario>> Get()
		{
			try
			{
				/*contexto.Inmuebles
					.Include(x => x.Duenio)
					.Where(x => x.Duenio.Nombre == "")//.ToList() => lista de inmuebles
					.Select(x => x.Duenio)
					.ToList();//lista de propietarios*/
				var usuario = User.Identity.Name;
				/*contexto.Contratos.Include(x => x.Inquilino).Include(x => x.Inmueble).ThenInclude(x => x.Duenio)
					.Where(c => c.Inmueble.Duenio.Email....);*/
				/*var res = contexto.Propietarios.Select(x => new { x.Nombre, x.Apellido, x.Email })
					.SingleOrDefault(x => x.Email == usuario);*/
				return await contexto.Propietarios.SingleOrDefaultAsync(x => x.Email == usuario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// GET api/<controller>/MiPerfil
		[HttpGet("MiPerfil")]
		public async Task<IActionResult> MiPerfil()
		{
			try
			{
				var entidad = await contexto.Propietarios.SingleOrDefaultAsync(propietario => propietario.Email == User.Identity.Name);
				return entidad != null ? Ok(entidad) : BadRequest("Error en identidad");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// GET api/<controller>/GetAll
		[AllowAnonymous]
		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				return Ok(await contexto.Propietarios.ToListAsync());
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// POST api/<controller>/login
		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] LoginView loginView)
		{
			try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: loginView.Clave,
					salt: System.Text.Encoding.ASCII.GetBytes(config["SALT"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));
				var propietario = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == loginView.Email);
				if (propietario == null || loginView.Clave != "vacia") //testear con |loginView.Clave != "vacia"|   en vez de   | propietario.Clave != hashed |
				{
					return BadRequest("Nombre de usuario o clave incorrecta");
				}
				else
				{
					var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, propietario.Email),
						new Claim("Id", propietario.Id.ToString()),
					};

					var token = new JwtSecurityToken(
						issuer: config["TokenAuthentication:Issuer"],
						audience: config["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(600),
						signingCredentials: credenciales
					);
					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// POST api/<controller>
		[HttpPost]
		public async Task<IActionResult> Post([FromForm] Propietario entidad)
		{
			try
			{
				if (ModelState.IsValid)
				{
					await contexto.Propietarios.AddAsync(entidad);
					contexto.SaveChanges();
					return CreatedAtAction(nameof(Get), new { id = entidad.Id}, entidad);
				}
				return BadRequest();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT api/<controller>/5
		[HttpPut("ActualizarPerfil")]
		public async Task<IActionResult> ActualizarPerfil([FromForm] Propietario obtenido)
		{
			try
			{
				//recuperar propietario
				var original = contexto.Propietarios.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
				if (original != null)
				{
					//modificar password
					Console.WriteLine(obtenido.Clave); 
					if(obtenido.Clave == "vacia"){
						Console.WriteLine("aca fue vacio"); 
					}else{
						original.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
							password: obtenido.Clave,
							salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
							prf: KeyDerivationPrf.HMACSHA1,
							iterationCount: 1000,
							numBytesRequested: 256 / 8));
					}
					//modificar otros campos
					original.Apellido = obtenido.Apellido;
					original.Nombre = obtenido.Nombre;
					original.Dni = obtenido.Dni;
					original.Telefono = obtenido.Telefono;
					await contexto.SaveChangesAsync();
					//recuperar el perfil cambiado
					var cambiado = contexto.Propietarios.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
					return Ok(cambiado);
				}
				return NotFound();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// DELETE api/<controller>/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var p = await contexto.Propietarios.FindAsync(id);
					if (p == null)
						return NotFound();
					contexto.Propietarios.Remove(p);
					await contexto.SaveChangesAsync();
					return Ok(p);
				}
				return BadRequest();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
