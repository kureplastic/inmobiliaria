using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Models;

public class RepositorioInmueble
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
    
    public List<Inmueble> ObtenerInmuebles(){
        List<Inmueble> inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"SELECT i.Id, Direccion, Tipo, Ambientes, Precio, Estado, PropietarioId,
                        p.Nombre, p.Apellido, p.Dni 
                        FROM inmuebles i
                        INNER JOIN propietarios p 
                        ON i.PropietarioId = p.Id";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read()){
                    Inmueble inmueble = new Inmueble{
                        Id = Convert.ToInt32(reader["Id"]),
                        Direccion = reader["Direccion"].ToString(),
                        Tipo = reader["Tipo"].ToString(),
                        Ambientes = Convert.ToInt32(reader["Ambientes"]),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        Estado = Convert.ToBoolean(reader["Estado"]),
                        PropietarioId = Convert.ToInt32(reader["PropietarioId"]),
                        propietario = new Propietario{
                            Id = Convert.ToInt32(reader["PropietarioId"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Dni = reader["Dni"].ToString()
                        }
                    };
                    inmuebles.Add(inmueble);
                }
                connection.Close();
            }
        }
        return inmuebles;
    }

    public Inmueble ObtenerInmueble(int id){
        Inmueble? inmueble = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"SELECT i.Id, Direccion, Tipo, Ambientes, Precio, Estado, PropietarioId,
                        p.Nombre, p.Apellido, p.Dni 
                        FROM inmuebles i
                        INNER JOIN propietarios p 
                        ON i.PropietarioId = p.Id
                        WHERE i.Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    inmueble = new Inmueble{
                        Id = Convert.ToInt32(reader["Id"]),
                        Direccion = reader["Direccion"].ToString(),
                        Tipo = reader["Tipo"].ToString(),
                        Ambientes = Convert.ToInt32(reader["Ambientes"]),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        Estado = Convert.ToBoolean(reader["Estado"]),
                        PropietarioId = Convert.ToInt32(reader["PropietarioId"]),
                        propietario = new Propietario{
                            Id = Convert.ToInt32(reader["PropietarioId"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Dni = reader["Dni"].ToString()
                        }
                    };
                }
                connection.Close();
            }
        }
        return inmueble;
    }

    public int RegistrarInmueble(Inmueble inmueble){
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"INSERT INTO inmuebles (Direccion, Tipo, Ambientes, Precio, Estado, PropietarioId)
                        VALUES (@Direccion, @Tipo, @Ambientes, @Precio, @Estado, @PropietarioId);
                        SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
                command.Parameters.AddWithValue("@Tipo", inmueble.Tipo);
                command.Parameters.AddWithValue("@Ambientes", inmueble.Ambientes);
                command.Parameters.AddWithValue("@Precio", inmueble.Precio);
                command.Parameters.AddWithValue("@Estado", inmueble.Estado);
                command.Parameters.AddWithValue("@PropietarioId", inmueble.PropietarioId);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res;
    }

    public int ActualizarInmueble(Inmueble inmueble){
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"UPDATE inmuebles SET Direccion = @Direccion, Ambientes = @Ambientes, Precio = @Precio, 
                        Estado = @Estado
                        WHERE Id = @Id";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Id", inmueble.Id);
                command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
                command.Parameters.AddWithValue("@Ambientes", inmueble.Ambientes);
                command.Parameters.AddWithValue("@Precio", inmueble.Precio);
                command.Parameters.AddWithValue("@Estado", inmueble.Estado);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteNonQuery());
                connection.Close();
            }
        }
        return res;
    }

    public int EliminarInmueble(int id){
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"DELETE FROM inmuebles WHERE Id = @id";
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

    public List<Inmueble> ObtenerInmueblesPorPropietario(int id){
        List<Inmueble> inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"SELECT i.Id, Direccion, Tipo, Ambientes, Precio, Estado, PropietarioId,
                        p.Nombre, p.Apellido, p.Dni 
                        FROM inmuebles i
                        INNER JOIN propietarios p 
                        ON i.PropietarioId = p.Id
                        WHERE p.Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read()){
                    Inmueble inmueble = new Inmueble{
                        Id = Convert.ToInt32(reader["Id"]),
                        Direccion = reader["Direccion"].ToString(),
                        Tipo = reader["Tipo"].ToString(),
                        Ambientes = Convert.ToInt32(reader["Ambientes"]),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        Estado = Convert.ToBoolean(reader["Estado"]),
                        PropietarioId = Convert.ToInt32(reader["PropietarioId"]),
                        propietario = new Propietario{
                            Id = Convert.ToInt32(reader["PropietarioId"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Dni = reader["Dni"].ToString()
                        }
                    };
                    inmuebles.Add(inmueble);
                }
                connection.Close();
            }
        }
        return inmuebles;
    }
}