using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace _26_reservaciones
{
    class Usuario
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["_26_reservaciones.Properties.Settings.ReservacionesConexion"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);

        //Propiedades
        public int Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string NombreCompleto { get; set; }

        public bool Estado { get; set; }
        //Constructores
        public Usuario() { }

        public Usuario(string nombreCompleto, string username, string password, bool estado)
        {
            NombreCompleto = nombreCompleto;
            Username = username;
            Password = password;
            Estado = estado;
        }

        //Metodos

        /// <summary>
        /// Verifica si las credenciales de inicio de sesion son correctas.
        /// </summary>
        /// <param name="username">El nombre del usuario</param>
        /// <returns>Los datos del usuario</returns>
        public Usuario BuscarUsuario(string username)
        {
            //Crear ibjeto que almacena la información de los resultados
            Usuario usuario = new Usuario();

            try
            {
                //Query de seleccion
                string query = @"SELECT * FROM Usuarios.usuario
                                WHERE username = @username";

                //Establecer la coneccio
                sqlConnection.Open();

                //Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        //Obtener los valores de los usuarios si la consulta remota valores
                        usuario.Id = Convert.ToInt32(rdr["id"]);
                        usuario.NombreCompleto = rdr["nombreCompleto"].ToString();
                        usuario.Username = rdr["username"].ToString();
                        usuario.Password = rdr["password"].ToString();
                        usuario.Estado = Convert.ToBoolean(rdr["estado"]);
                    }
                }

                //Retonar el usuario con los valores
                return usuario;

            }

            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                sqlConnection.Close();
            }

         }  

    }
}
