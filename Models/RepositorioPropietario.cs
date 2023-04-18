using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Models;


public class RepositorioPropietario
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public RepositorioPropietario(){
        
    }
    public List<Propietario> ObtenerPropietarios(){
        List<Propietario> propietarios = new List<Propietario>();
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"SELECT id, dni, apellido, nombre, telefono, email 
                        FROM propietarios";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader()){
                    while (reader.Read()){
                        var tel = reader["telefono"];
                        Propietario propietario = new Propietario
                        {
                            Id = reader.GetInt32("id"),
                            Dni = reader.GetString("dni"),
                            Apellido = reader.GetString("apellido"),
                            Nombre = reader.GetString("nombre"),
                            Telefono = tel == DBNull.Value ? null : reader.GetString("telefono"),
                            Email = reader.GetString("email")
                        };
                        propietarios.Add(propietario);
                    }
                }
            }
            connection.Close();
        }
        return propietarios;
    }

    public int RegistrarPropietario(Propietario propietario){
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"INSERT INTO propietarios (dni, apellido, nombre, telefono, email)
                        VALUES (@dni, @apellido, @nombre, @telefono, @email);
                        SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@dni", propietario.Dni);
                command.Parameters.AddWithValue("@apellido", propietario.Apellido);
                command.Parameters.AddWithValue("@nombre", propietario.Nombre);
                command.Parameters.AddWithValue("@telefono", propietario.Telefono);
                command.Parameters.AddWithValue("@email", propietario.Email);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                propietario.Id = res;
            }
        }
        return res;
    }

    public int EliminarPropietario(int id){
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"DELETE FROM propietarios WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteNonQuery());
                connection.Close();
            }
        }
        return res;
    } 

    public int ActualizarPropietario(Propietario propietario){
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"UPDATE propietarios SET apellido = @apellido, nombre = @nombre, telefono = @telefono, email = @email WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", propietario.Id);
                command.Parameters.AddWithValue("@apellido", propietario.Apellido);
                command.Parameters.AddWithValue("@nombre", propietario.Nombre);
                command.Parameters.AddWithValue("@telefono", propietario.Telefono);
                command.Parameters.AddWithValue("@email", propietario.Email);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteNonQuery());
                connection.Close();
            }
        }
        return res;
    }
    
    public Propietario ObtenerPropietario(int id){
        Propietario? p = null;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT 
					id, nombre, apellido, dni, telefono, email
					FROM Propietarios
					WHERE id=@id";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@id", id);
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
                        var telef = reader["telefono"];
						p = new Propietario
						{
							Id = reader.GetInt32(nameof(Propietario.Id)),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = telef == DBNull.Value ? null : reader.GetString("telefono"),
							Email = reader.GetString("Email"),
						};
					}
					connection.Close();
				}
			}
			return p;
    }
}