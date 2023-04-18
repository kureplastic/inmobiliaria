using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Models;

public class RepositorioInquilino
{

    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    public List<Inquilino> ObtenerInquilinos()
    {
        List<Inquilino> inquilinos = new List<Inquilino>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT id, dni, nombre, apellido, lugarTrabajo, telefono, email, nombreGarante, dniGarante, telefonoGarante, emailGarante
                        FROM inquilinos";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inquilino inquilino = new Inquilino
                        {
                            Id = reader.GetInt32("id"),
                            Dni = reader.GetString("dni"),
                            Nombre = reader.GetString("nombre"),
                            Apellido = reader.GetString("apellido"),
                            LugarTrabajo = reader.GetString("lugarTrabajo"),
                            Telefono = reader.GetString("telefono"),
                            Email = reader.GetString("email"),
                            NombreGarante = reader.GetString("nombreGarante"),
                            DniGarante = reader.GetString("dniGarante"),
                            TelefonoGarante = reader.GetString("telefonoGarante"),
                            EmailGarante = reader.GetString("emailGarante")
                        };
                        inquilinos.Add(inquilino);
                    }
                }
            }
            connection.Close();
        }
        return inquilinos;
    }
    public Inquilino ObtenerInquilino(int id)
    {
        Inquilino? inquilino = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT id, dni, nombre, apellido, lugarTrabajo, telefono, email, nombreGarante, dniGarante, telefonoGarante, emailGarante
                        FROM inquilinos
                        WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        inquilino = new Inquilino
                        {
                            Id = reader.GetInt32("id"),
                            Dni = reader.GetString("dni"),
                            Nombre = reader.GetString("nombre"),
                            Apellido = reader.GetString("apellido"),
                            LugarTrabajo = reader.GetString("lugarTrabajo"),
                            Telefono = reader.GetString("telefono"),
                            Email = reader.GetString("email"),
                            NombreGarante = reader.GetString("nombreGarante"),
                            DniGarante = reader.GetString("dniGarante"),
                            TelefonoGarante = reader.GetString("telefonoGarante"),
                            EmailGarante = reader.GetString("emailGarante")
                        };
                    }
                }
            }
            connection.Close();
        }
        return inquilino;
    }

    public int RegistrarInquilino(Inquilino inquilino)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"INSERT INTO inquilinos (dni, nombre, apellido, lugarTrabajo, telefono, email, nombreGarante, dniGarante, telefonoGarante, emailGarante)
                        VALUES (@dni, @nombre, @apellido, @lugarTrabajo, @telefono, @email, @nombreGarante, @dniGarante, @telefonoGarante, @emailGarante);
                        SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@dni", inquilino.Dni);
                command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
                command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
                command.Parameters.AddWithValue("@lugarTrabajo", inquilino.LugarTrabajo == null ? "" : inquilino.LugarTrabajo);
                command.Parameters.AddWithValue("@telefono", inquilino.Telefono);
                command.Parameters.AddWithValue("@email", inquilino.Email == null ? "" : inquilino.Email);
                command.Parameters.AddWithValue("@nombreGarante", inquilino.NombreGarante);
                command.Parameters.AddWithValue("@dniGarante", inquilino.DniGarante);
                command.Parameters.AddWithValue("@telefonoGarante", inquilino.TelefonoGarante);
                command.Parameters.AddWithValue("@emailGarante", inquilino.EmailGarante == null ? "" : inquilino.EmailGarante);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                inquilino.Id = res;
            }
        }
        return res;
    }

    public int ActualizarIquilino(Inquilino inquilino)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"UPDATE inquilinos
                        SET nombre = @nombre, apellido = @apellido, lugarTrabajo = @lugarTrabajo, telefono = @telefono, email = @email,
                        nombreGarante = @nombreGarante, dniGarante = @dniGarante, telefonoGarante = @telefonoGarante, emailGarante = @emailGarante
                        WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", inquilino.Id);
                command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
                command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
                command.Parameters.AddWithValue("@lugarTrabajo", inquilino.LugarTrabajo == null ? "" : inquilino.LugarTrabajo);
                command.Parameters.AddWithValue("@telefono", inquilino.Telefono);
                command.Parameters.AddWithValue("@email", inquilino.Email == null ? "" : inquilino.Email);
                command.Parameters.AddWithValue("@nombreGarante", inquilino.NombreGarante);
                command.Parameters.AddWithValue("@dniGarante", inquilino.DniGarante);
                command.Parameters.AddWithValue("@telefonoGarante", inquilino.TelefonoGarante);
                command.Parameters.AddWithValue("@emailGarante", inquilino.EmailGarante == null ? "" : inquilino.EmailGarante);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteNonQuery());
                connection.Close();
            }
        }
        return res;
    }

    public int EliminarInquilino(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"DELETE FROM inquilinos WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteNonQuery());
                connection.Close();
            }
        }
        return res;
    }

}