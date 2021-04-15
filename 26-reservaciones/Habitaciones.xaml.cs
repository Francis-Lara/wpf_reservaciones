using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _26_reservaciones
{
    /// <summary>
    /// Lógica de interacción para Habitaciones.xaml
    /// </summary>
    public partial class Habitaciones : Window
    {
        //variables miembro
        private Habitacion habitacion = new Habitacion();
        private List<Habitacion> habitaciones;

        public Habitaciones()
        {
            InitializeComponent();

            //llenar el combobox de estado de habitacion
            cmbEstado.ItemsSource = Enum.GetValues(typeof(EstadosHabitacion));

            //llenar el listbox de habitaciones
            ObtenerHabitaciones();
        }

        private void LimpiarFormulario()
        {
            txtDescripcion.Text = string.Empty;
            txtNumeroHabitacion.Text = string.Empty;
            cmbEstado.SelectedValue = null;
        }
        private void ObtenerValoresFormulario()
        {
            habitacion.Descripcion = txtDescripcion.Text;
            habitacion.Numero = Convert.ToInt32(txtNumeroHabitacion.Text);
            habitacion.Estado = (EstadosHabitacion)cmbEstado.SelectedValue;
            
        }


        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            // verificar que se ingresaron los valores requeridos
            if (txtDescripcion.Text == string.Empty || txtNumeroHabitacion.Text == string.Empty)
                MessageBox.Show("pOR FAVOR INGRESA TODOS LOS VALORES EN LAS CAJAS DE TEXTO");
            else if (cmbEstado.SelectedValue == null)
                MessageBox.Show("Selecciona un estado para la habitacion");
            else
            {
                try
                {
                    //obtener los valores para la habitacion
                    ObtenerValoresFormulario();

                    //insertar los datos de la habitacion
                    habitacion.CrearHabitacion(habitacion);

                    //mensaje de insercion exitosa
                    MessageBox.Show("Datos Insertados");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ha ocurrido un error al insertar la habitacion");
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    LimpiarFormulario();
                    ObtenerHabitaciones();
                }
            }
        }
        private void ObtenerHabitaciones()
        {
            habitaciones = habitacion.MostrarHabitaciones();
            lbHabitaciones.DisplayMemberPath = "Descripcion";
            lbHabitaciones.SelectedValuePath = "Id";
            lbHabitaciones.ItemsSource = habitaciones;
          
        }
    }
}
