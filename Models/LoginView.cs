using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria.Models
{
	public class LoginView
	{
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[DataType(DataType.Password)]
		public string Clave { get; set; }
	}
}