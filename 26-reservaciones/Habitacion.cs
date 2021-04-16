using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Agrefar los namespace
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;

namespace _26_reservaciones
{
    //Crear una variable que mantengaa los valoress para los estados de la habitacion
    public enum EstadosHabitacion
    {
        Ocupado = 'O',
        Disponible = 'D',
        Mantenimiento = 'M',
        FueraDeServicio = 'F'
    }
    class Habitacion
    {
        //Variable miembro
        private static string connecntionString = ConfigurationManager.ConnectionStrings["_26_reservaciones.Properties.Settings.ReservacionesConexion"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connecntionString);

        //Propiedades

        public int Id { get; set; }

        public string Descripcion { get; set; }

        public int Numero { get; set; }
        public EstadosHabitacion Estado { get; set; }

        //Constructores
        public Habitacion() { }

        public Habitacion(string descripcion, int numero, EstadosHabitacion estado)
        {
            Descripcion = descripcion;
            Numero = numero;
            Estado = estado;
        }
        /// <summary>
        /// retorna el estado de la habitacion desde el enum de estados
        /// </summary>
        /// <param name="estado"></el valor dentro del enum>
        /// <returns></estado valido dentro de la base de datos>

        private string ObtenerEstado(EstadosHabitacion estado)
        {
            switch (estado)
            {
                case EstadosHabitacion.Ocupado:
                    return "OCUPADA";

                case EstadosHabitacion.Disponible:
                    return "DISPONIBLE";

                case EstadosHabitacion.Mantenimiento:
                    return "MANTENIMIENTO";
 
                case EstadosHabitacion.FueraDeServicio:
                    return "FUERADESERVICIO";
            
                default:
                    return "DISPONIBLE";
               
            }

        }
        /// <summary>
        /// inserta una habitacion
        /// </summary>
        /// <param name="habitacion"></la informacion de la habitacion>
        public void CrearHabitacion(Habitacion habitacion)
        {

            try
            {
                string query = @"INSERT INTO Habitaciones.Habitacion (descripcion, numero, estado)
                             VALUES (@descripcion, @numero, @estado)";

                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@descripcion", habitacion.Descripcion);
                sqlCommand.Parameters.AddWithValue("@numero", habitacion.Numero);
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(habitacion.Estado));

                // ejecutar comando de insercion.
                sqlCommand.ExecuteNonQuery();

            }

            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //cerrar conexion
                sqlConnection.Close();
            }
        }
        /// <summary>
        /// muestra todas las habitaciones
        /// </summary>
        /// <returns> listado de todas las habitaciones</returns>
        public List<Habitacion> MostrarHabitaciones()
        {
            //inicializar una lista vacia de habitaciones
            List<Habitacion> habitaciones = new List<Habitacion>();
            try
            {
                //query de seleccion
                string query = @"SELECT id, descripcion, estado
                                 FROM habitaciones.habitacion ";

                //establecer la conexion
                sqlConnection.Open();
                //crear el comando sql
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //obtener los datos de las habitaciones
                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                        habitaciones.Add(new Habitacion { Id = Convert.ToInt32(rdr["id"]), Descripcion = rdr["descripcion"].ToString() });
                }
                return habitaciones;
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                //Cerrar la conexion
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// obtiene una habitacion por su id
        /// </summary>
        /// <param name="id">el id de la habitacion </param>
        /// <returns> los datos de la habitacion</returns>
        public Habitacion BuscarHabitacion(int id)
        {
            Habitacion laHabitacion = new Habitacion();
            try
            {
                //Query de busqueda
                string query = @"SELECT * FROM Habitaciones.Habitacion
                                WHERE  id = @id";

                //establecer conexion
                sqlConnection.Open();

                //crear comando sql
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //establecer el valor del parametro
                sqlCommand.Parameters.AddWithValue("@id", id);

                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        laHabitacion.Id = Convert.ToInt32(rdr["id"]);
                        laHabitacion.Descripcion = rdr["descripcion"].ToString();
                        laHabitacion.Numero = Convert.ToInt32(rdr["numero"]);
                        laHabitacion.Estado = (EstadosHabitacion)Convert.ToChar(rdr["estado"].ToString().Substring(0, 1));
                    }

                }
                return laHabitacion;
            }
            
            catch (Exception e)
            {
                throw e;
            }
             finally
            {
                sqlConnection.Close();
            }

        }
        /// <summary>
        /// modifica los datos de una habitacion
        /// </summary>
        /// <param name="habitacion">el id de la habitacion</param>
        public void ModificarHabitacion(Habitacion habitacion)
        {
            try
            {
                //query actualizacion
                string query = @"UPDATE Habitaciones.Habitacion
                                SET descripcion = @descripcion, numero = @numero, estado = @estado
                                WHERE id = @id";

                //establecer conexion
                sqlConnection.Open();

                //crear comando
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //establecer valores de los parametros
                sqlCommand.Parameters.AddWithValue("@id", habitacion.Id);
                sqlCommand.Parameters.AddWithValue("@descripcion", habitacion.Descripcion);
                sqlCommand.Parameters.AddWithValue("@numero",habitacion.Numero);
                sqlCommand.Parameters.AddWithValue("@estado",ObtenerEstado(habitacion.Estado));

                //ejecutar el comando de actualizacion
                sqlCommand.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            finally
            {
                //Cerrar conexion
                sqlConnection.Close();
            }
        }
        /// <summary>
        /// eliminar una habitacion
        /// </summary>
        /// <param name="id">id de la habitacion</param>
        public void EliminarHabitacion(int id)
        {
            try
            {
                //query de eliminacion
                string query = @"DELETE FROM Habitaciones.Habitacion
                                 WHERE id = @id";
                //establecer la conexion
                sqlConnection.Open();

                //crear el comando sql
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //establecer el valor dl parametro
                sqlCommand.Parameters.AddWithValue("@id", id);

                //ejecutar el comando de eliminacion 
                sqlCommand.ExecuteNonQuery();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                //cerrar la conexion
                sqlConnection.Close();
            }
        }
    }
    
}
