using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Models;

public class RepositorioPago
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";

    public RepositorioPago(){

    }

    public List<Pago> ObtenerPagos()
    {
        List<Pago> pagos = new List<Pago>();
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"SELECT p.Id, numPago, importe, fechaPago, p.ContratoId, 
                        c.InquilinoId, c.PropietarioId, c.InmuebleId, c.FechaInicio, c.FechaFin, c.MontoMensual,
                        inq.Dni as InquilinoDni, inq.Nombre as InquilinoNombre, inq.Apellido as InquilinoApellido, 
                        pa.Dni as PropietarioDni, pa.Nombre as PropietarioNombre, pa.Apellido as PropietarioApellido,
                        i.Direccion, i.Tipo
                        FROM pagos p
                        INNER JOIN contratos c ON p.ContratoId = c.Id
                        INNER JOIN inquilinos inq ON c.InquilinoId = inq.Id
                        INNER JOIN propietarios pa ON c.PropietarioId = pa.Id
                        INNER JOIN inmuebles i ON c.InmuebleId = i.Id";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()){
                Pago pago = new Pago{
                    Id = Convert.ToInt32(reader["Id"]),
                    numPago = Convert.ToInt32(reader["numPago"]),
                    importe = Convert.ToDecimal(reader["importe"]),
                    fechaPago = Convert.ToDateTime(reader["fechaPago"]),
                    ContratoId = Convert.ToInt32(reader["ContratoId"]),
                    contrato = new Contrato{
                        Id = Convert.ToInt32(reader["ContratoId"]),
                        InquilinoId = Convert.ToInt32(reader["InquilinoId"]),
                        PropietarioId = Convert.ToInt32(reader["PropietarioId"]),
                        InmuebleId = Convert.ToInt32(reader["InmuebleId"]),
                        FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                        FechaFin = Convert.ToDateTime(reader["FechaFin"]),
                        MontoMensual = Convert.ToDecimal(reader["MontoMensual"]),
                        inquilino = new Inquilino{
                            Dni = Convert.ToString(reader["InquilinoDni"]),
                            Nombre = Convert.ToString(reader["InquilinoNombre"]),
                            Apellido = Convert.ToString(reader["InquilinoApellido"])
                        },
                        propietario = new Propietario{
                            Dni = Convert.ToString(reader["PropietarioDni"]),
                            Nombre = Convert.ToString(reader["PropietarioNombre"]),
                            Apellido = Convert.ToString(reader["PropietarioApellido"])
                        },
                        inmueble = new Inmueble{
                            Direccion = Convert.ToString(reader["Direccion"]),
                            Tipo = Convert.ToString(reader["Tipo"])
                        }
                    }
                };
                pagos.Add(pago);
            }
            connection.Close();
        }
        return pagos;
    }

    public Pago ObtenerPago(int id){
        Pago? pago = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"SELECT Id, numPago, importe, fechaPago, ContratoId
                        FROM pagos
                        WHERE Id = @id";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read()){
                pago = new Pago{
                    Id = Convert.ToInt32(reader["Id"]),
                    numPago = Convert.ToInt32(reader["numPago"]),
                    importe = Convert.ToDecimal(reader["importe"]),
                    fechaPago = Convert.ToDateTime(reader["fechaPago"]),
                    ContratoId = Convert.ToInt32(reader["ContratoId"])
                };
            }
            connection.Close();
        }
        return pago;
    }

    public List<Pago> ObtenerPagosPorContrato(int idContrato)
    {
        List<Pago> pagos = new List<Pago>();
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"SELECT p.Id, numPago, importe, fechaPago, p.ContratoId, 
                        c.InquilinoId, c.PropietarioId, c.InmuebleId, c.FechaInicio, c.FechaFin, c.MontoMensual,
                        inq.Dni as InquilinoDni, inq.Nombre as InquilinoNombre, inq.Apellido as InquilinoApellido, 
                        pa.Dni as PropietarioDni, pa.Nombre as PropietarioNombre, pa.Apellido as PropietarioApellido,
                        i.Direccion, i.Tipo
                        FROM pagos p
                        INNER JOIN contratos c ON p.ContratoId = c.Id
                        INNER JOIN inquilinos inq ON c.InquilinoId = inq.Id
                        INNER JOIN propietarios pa ON c.PropietarioId = pa.Id
                        INNER JOIN inmuebles i ON c.InmuebleId = i.Id
                        WHERE p.ContratoId = @idContrato;";
            using (MySqlCommand command = new MySqlCommand(query, connection)){
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@idContrato", idContrato);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read()){
                Pago pago = new Pago{
                    Id = Convert.ToInt32(reader["Id"]),
                    numPago = Convert.ToInt32(reader["numPago"]),
                    importe = Convert.ToDecimal(reader["importe"]),
                    fechaPago = Convert.ToDateTime(reader["fechaPago"]),
                    ContratoId = Convert.ToInt32(reader["ContratoId"]),
                    contrato = new Contrato{
                        Id = Convert.ToInt32(reader["ContratoId"]),
                        InquilinoId = Convert.ToInt32(reader["InquilinoId"]),
                        PropietarioId = Convert.ToInt32(reader["PropietarioId"]),
                        InmuebleId = Convert.ToInt32(reader["InmuebleId"]),
                        FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                        FechaFin = Convert.ToDateTime(reader["FechaFin"]),
                        MontoMensual = Convert.ToDecimal(reader["MontoMensual"]),
                        inquilino = new Inquilino{
                            Id = Convert.ToInt32(reader["InquilinoId"]),
                            Dni = Convert.ToString(reader["InquilinoDni"]),
                            Nombre = Convert.ToString(reader["InquilinoNombre"]),
                            Apellido = Convert.ToString(reader["InquilinoApellido"])
                        },
                        propietario = new Propietario{
                            Id = Convert.ToInt32(reader["PropietarioId"]),
                            Dni = Convert.ToString(reader["PropietarioDni"]),
                            Nombre = Convert.ToString(reader["PropietarioNombre"]),
                            Apellido = Convert.ToString(reader["PropietarioApellido"])
                        },
                        inmueble = new Inmueble{
                            Id = Convert.ToInt32(reader["InmuebleId"]),
                            Direccion = Convert.ToString(reader["Direccion"]),
                            Tipo = Convert.ToString(reader["Tipo"])
                        }
                    }
                };
                pagos.Add(pago);
            }
            connection.Close();
            }
        }
        return pagos;
    }

    public int RegistrarPago(Pago pago){
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"INSERT INTO pagos (numPago, importe, fechaPago, ContratoId)
                        VALUES (@numPago, @importe, @fechaPago, @ContratoId);
                        SELECT LAST_INSERT_ID();";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@numPago", pago.numPago);
            command.Parameters.AddWithValue("@importe", pago.importe);
            command.Parameters.AddWithValue("@fechaPago", pago.fechaPago);
            command.Parameters.AddWithValue("@ContratoId", pago.ContratoId);
            connection.Open();
            res = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
        }
        return res;
    }

    public int EliminarPago(int idPago){
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"DELETE FROM pagos WHERE Id = @idPago";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@idPago", idPago);
            connection.Open();
            res = Convert.ToInt32(command.ExecuteNonQuery());
            connection.Close();
        }
        return res;
    }

    public int ActualizarPago(Pago pago){
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString)){
            var query = @"UPDATE pagos SET numPago = @numPago, importe = @importe, fechaPago = @fechaPago
                        WHERE Id = @idPago";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@numPago", pago.numPago);
            command.Parameters.AddWithValue("@importe", pago.importe);
            command.Parameters.AddWithValue("@fechaPago", pago.fechaPago);
            command.Parameters.AddWithValue("@idPago", pago.Id);
            connection.Open();
            res = Convert.ToInt32(command.ExecuteNonQuery());
            connection.Close();
        }
        return res;
    }
}