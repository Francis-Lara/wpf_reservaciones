using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Agrefar los namespace
using System.Data.SqlClient;
using System.Configuration;

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
                    return "disponible";

                case EstadosHabitacion.Mantenimiento:
                    return "mantenimiento";
 
                case EstadosHabitacion.FueraDeServicio:
                    return "fuera de servicio";
            
                default:
                    return "disponible";
               
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
    }
    
}
