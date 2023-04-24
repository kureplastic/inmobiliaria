using System.Data;
using Inmobiliaria_.Net_Core.Models;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Models;

public class RepositorioUsuario
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";

    public RepositorioUsuario(){
        
    }

    public List<Usuario> ObtenerUsuarios(){
        List<Usuario> usuarios = new List<Usuario>();
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"SELECT Id, Nombre, Apellido, Email, Clave, RutaAvatar, Rol FROM usuarios";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader()){
                    while (reader.Read()){
                        Usuario usuario = new Usuario{
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Email = reader.GetString("Email"),
                            Clave = reader.GetString("Clave"),
                            RutaAvatar = reader.GetString("RutaAvatar"),
                            Rol = reader.GetInt32("Rol")
                        };
                        usuarios.Add(usuario);
                    }
                }
                connection.Close();
            }
        }
        return usuarios;
    }

    public Usuario ObtenerUsuario(int id){
        Usuario? usuario = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"SELECT Id, Nombre, Apellido, Email, Clave, RutaAvatar, Rol 
                        FROM usuarios WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader()){
                    if (reader.Read()){
                        usuario = new Usuario
                        {
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Email = reader.GetString("Email"),
                            Clave = reader.GetString("Clave"),
                            RutaAvatar = reader.GetString("RutaAvatar"),
                            Rol = reader.GetInt32("Rol")
                        };
                    }
                }
                connection.Close();
            }
        }
        return usuario;
    }

    public Usuario ObtenerUsuarioPorEmail(string email){
        Usuario? usuario = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            
            var query = @"SELECT Id, Nombre, Apellido, Email, Clave, RutaAvatar, Rol
                        FROM usuarios WHERE Email = @email";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@email",  email);
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader()){
                    if (reader.Read()){
                        usuario = new Usuario
                        {
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("Nombre"),
                            Apellido = reader.GetString("Apellido"),
                            Email = reader.GetString("Email"),
                            Clave = reader.GetString("Clave"),
                            RutaAvatar = reader.GetString("RutaAvatar"),
                            Rol = reader.GetInt32("Rol")
                        };
                    }
                }
                connection.Close();
            }
        }
        return usuario;
    }

    public int RegistrarUsuario(Usuario usuario){
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"INSERT INTO usuarios (Nombre, Apellido, Email, Clave, RutaAvatar, Rol)
                        VALUES (@nombre, @apellido, @email, @clave, @rutaAvatar, @rol);
                        SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@apellido", usuario.Apellido);
                command.Parameters.AddWithValue("@email", usuario.Email);
                command.Parameters.AddWithValue("@clave", usuario.Clave);
                command.Parameters.AddWithValue("@rutaAvatar", "");
                command.Parameters.AddWithValue("@rol", usuario.Rol);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res;
    }

    public int ActualizarUsuario(Usuario usuario){
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"UPDATE usuarios SET Nombre = @nombre, Apellido = @apellido, Email = @email, Clave = @clave, RutaAvatar = @rutaAvatar, Rol = @rol
                        WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", usuario.Id);
                command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@apellido", usuario.Apellido);
                command.Parameters.AddWithValue("@email", usuario.Email);
                command.Parameters.AddWithValue("@clave", usuario.Clave);
                if(usuario.RutaAvatar == null){command.Parameters.AddWithValue("@rutaAvatar", "");}
                else {command.Parameters.AddWithValue("@rutaAvatar", usuario.RutaAvatar);}
                command.Parameters.AddWithValue("@rol", usuario.Rol);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public int EliminarUsuario(int id){
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"DELETE FROM usuarios WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }
}

