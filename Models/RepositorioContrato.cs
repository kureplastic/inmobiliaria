using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Models;

public class RepositorioContrato
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";

    public List<Contrato> ObtenerContratos()
    {
        List<Contrato> contratos = new List<Contrato>();
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"SELECT c.Id, FechaInicio, FechaFin, MontoMensual, c.InmuebleId, c.PropietarioId, c.InquilinoId, 
                        i.Direccion, i.Tipo, 
                        p.Dni as PropietarioDni, p.Nombre as PropietarioNombre, p.Apellido as PropietarioApellido, p.Telefono as PropietarioTelefono,
                        iq.Dni as InquilinoDni, iq.Nombre as InquilinoNombre, iq.Apellido as InquilinoApellido, iq.Telefono as InquilinoTelefono, iq.NombreGarante, iq.TelefonoGarante 
                        FROM contratos c
                        INNER JOIN inmuebles i ON c.InmuebleId = i.Id
                        INNER JOIN propietarios p ON c.PropietarioId = p.Id
                        INNER JOIN inquilinos iq ON c.InquilinoId = iq.Id";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()){
                Contrato contrato = new Contrato{
                    Id = Convert.ToInt32(reader["Id"]),
                    FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                    FechaFin = Convert.ToDateTime(reader["FechaFin"]),
                    MontoMensual = Convert.ToDecimal(reader["MontoMensual"]),
                    InmuebleId = Convert.ToInt32(reader["InmuebleId"]),
                    PropietarioId = Convert.ToInt32(reader["PropietarioId"]),
                    InquilinoId = Convert.ToInt32(reader["InquilinoId"]),
                    propietario = new Propietario{
                        Dni = Convert.ToString(reader["PropietarioDni"]),
                        Nombre = Convert.ToString(reader["PropietarioNombre"]),
                        Apellido = Convert.ToString(reader["PropietarioApellido"]),
                        Telefono = Convert.ToString(reader["PropietarioTelefono"]),
                    },
                    inquilino = new Inquilino{
                        Dni = Convert.ToString(reader["InquilinoDni"]),
                        Nombre = Convert.ToString(reader["InquilinoNombre"]),
                        Apellido = Convert.ToString(reader["InquilinoApellido"]),
                        Telefono = Convert.ToString(reader["InquilinoTelefono"]),
                        NombreGarante = Convert.ToString(reader["NombreGarante"]),
                        TelefonoGarante = Convert.ToString(reader["TelefonoGarante"]),
                    },
                    inmueble = new Inmueble{
                        Direccion = Convert.ToString(reader["Direccion"]),
                        Tipo = Convert.ToString(reader["Tipo"]),
                    }
                };
                contratos.Add(contrato);
            }
        }
        return contratos;
    }
    public Contrato ObtenerContrato(int id)
    {
        Contrato? contrato = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"SELECT c.Id, FechaInicio, FechaFin, MontoMensual, c.InmuebleId, c.PropietarioId, c.InquilinoId,
                        i.Direccion, i.Tipo,
                        p.Dni as PropietarioDni, p.Nombre as PropietarioNombre, p.Apellido as PropietarioApellido, p.Telefono as PropietarioTelefono,
                        iq.Dni as InquilinoDni, iq.Nombre as InquilinoNombre, iq.Apellido as InquilinoApellido, iq.Telefono as InquilinoTelefono, iq.NombreGarante, iq.TelefonoGarante
                        FROM contratos c
                        INNER JOIN inmuebles i ON c.InmuebleId = i.Id
                        INNER JOIN propietarios p ON c.PropietarioId = p.Id
                        INNER JOIN inquilinos iq ON c.InquilinoId = iq.Id
                        WHERE c.Id = @id";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()){
                contrato = new Contrato{
                    Id = Convert.ToInt32(reader["Id"]),
                    FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                    FechaFin = Convert.ToDateTime(reader["FechaFin"]),
                    MontoMensual = Convert.ToDecimal(reader["MontoMensual"]),
                    InmuebleId = Convert.ToInt32(reader["InmuebleId"]),
                    PropietarioId = Convert.ToInt32(reader["PropietarioId"]),
                    InquilinoId = Convert.ToInt32(reader["InquilinoId"]),
                    propietario = new Propietario{
                        Dni = Convert.ToString(reader["PropietarioDni"]),
                        Nombre = Convert.ToString(reader["PropietarioNombre"]),
                        Apellido = Convert.ToString(reader["PropietarioApellido"]),
                        Telefono = Convert.ToString(reader["PropietarioTelefono"]),
                    },
                    inquilino = new Inquilino{
                        Dni = Convert.ToString(reader["InquilinoDni"]),
                        Nombre = Convert.ToString(reader["InquilinoNombre"]),
                        Apellido = Convert.ToString(reader["InquilinoApellido"]),
                        Telefono = Convert.ToString(reader["InquilinoTelefono"]),
                        NombreGarante = Convert.ToString(reader["NombreGarante"]),
                        TelefonoGarante = Convert.ToString(reader["TelefonoGarante"]),
                    },
                    inmueble = new Inmueble{
                        Direccion = Convert.ToString(reader["Direccion"]),
                        Tipo = Convert.ToString(reader["Tipo"]),
                    }
                };
            }
            connection.Close();
        }
        return contrato;
    }
    public int RegistrarContrato(Contrato contrato)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"INSERT INTO contratos (FechaInicio, FechaFin, MontoMensual, InmuebleId, PropietarioId, InquilinoId)
                        VALUES (@FechaInicio, @FechaFin, @MontoMensual, @InmuebleId, @PropietarioId, @InquilinoId);
                        SELECT LAST_INSERT_ID();";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
            command.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
            command.Parameters.AddWithValue("@MontoMensual", contrato.MontoMensual);
            command.Parameters.AddWithValue("@InmuebleId", contrato.InmuebleId);
            command.Parameters.AddWithValue("@PropietarioId", contrato.PropietarioId);
            command.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
            connection.Open();
            res = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
        }
        return res;
    }

    public int EliminarContrato(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"DELETE FROM contratos WHERE Id = @Id";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            res = Convert.ToInt32(command.ExecuteNonQuery());
            connection.Close();
        }
        return res;
    }

    public int ActualizarContrato(Contrato contrato){
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"UPDATE contratos SET FechaInicio = @FechaInicio, FechaFin = @FechaFin, MontoMensual = @MontoMensual, InquilinoId = @InquilinoId
                        WHERE Id = @Id";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
            command.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
            command.Parameters.AddWithValue("@MontoMensual", contrato.MontoMensual);
            command.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
            command.Parameters.AddWithValue("@Id", contrato.Id);
            connection.Open();
            res = Convert.ToInt32(command.ExecuteNonQuery());
            connection.Close();
        }
        return res;
    }
    public List<Contrato> ObtenerContratosPorInmueble(int id){
        List<Contrato> contratos = new List<Contrato>();
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"SELECT c.Id, FechaInicio, FechaFin, MontoMensual, c.InmuebleId, c.PropietarioId, c.InquilinoId,
                        i.Direccion, i.Tipo,
                        p.Dni as PropietarioDni, p.Nombre as PropietarioNombre, p.Apellido as PropietarioApellido, p.Telefono as PropietarioTelefono,
                        iq.Dni as InquilinoDni, iq.Nombre as InquilinoNombre, iq.Apellido as InquilinoApellido, iq.Telefono as InquilinoTelefono, iq.NombreGarante, iq.TelefonoGarante
                        FROM contratos c
                        INNER JOIN inmuebles i ON c.InmuebleId = i.Id
                        INNER JOIN propietarios p ON c.PropietarioId = p.Id
                        INNER JOIN inquilinos iq ON c.InquilinoId = iq.Id
                        WHERE c.InmuebleId = @Id";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()){
                contratos.Add(new Contrato{
                    Id = Convert.ToInt32(reader["Id"]),
                    FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                    FechaFin = Convert.ToDateTime(reader["FechaFin"]),
                    MontoMensual = Convert.ToDecimal(reader["MontoMensual"]),
                    InmuebleId = Convert.ToInt32(reader["InmuebleId"]),
                    PropietarioId = Convert.ToInt32(reader["PropietarioId"]),
                    InquilinoId = Convert.ToInt32(reader["InquilinoId"]),
                    propietario = new Propietario{
                        Dni = Convert.ToString(reader["PropietarioDni"]),
                        Nombre = Convert.ToString(reader["PropietarioNombre"]),
                        Apellido = Convert.ToString(reader["PropietarioApellido"]),
                        Telefono = Convert.ToString(reader["PropietarioTelefono"]),
                    },
                    inquilino = new Inquilino{
                        Dni = Convert.ToString(reader["InquilinoDni"]),
                        Nombre = Convert.ToString(reader["InquilinoNombre"]),
                        Apellido = Convert.ToString(reader["InquilinoApellido"]),
                        Telefono = Convert.ToString(reader["InquilinoTelefono"]),
                        NombreGarante = Convert.ToString(reader["NombreGarante"]),
                        TelefonoGarante = Convert.ToString(reader["TelefonoGarante"]),
                    },
                    inmueble = new Inmueble{
                        Direccion = Convert.ToString(reader["Direccion"]),
                        Tipo = Convert.ToString(reader["Tipo"]),
                    }
                });
            }
            connection.Close();
        }
        return contratos;
    }
}