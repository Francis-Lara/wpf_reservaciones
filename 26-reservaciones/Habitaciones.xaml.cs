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
            habitacion.Id = Convert.ToInt32(lbHabitaciones.SelectedValue);
            
        }

        private void OcultarBotonesOperaciones(Visibility ocultar)
        {
            btnAgregar.Visibility = ocultar;
            btnModificar.Visibility = ocultar;
            btnEliminar.Visibility = ocultar;
            btnRegresar.Visibility = ocultar;
        }
        private bool VerificarValores()
        {
            if (txtDescripcion.Text == string.Empty || txtNumeroHabitacion.Text == string.Empty)
            {
                MessageBox.Show("pOR FAVOR INGRESA TODOS LOS VALORES EN LAS CAJAS DE TEXTO");
                return false;
            }
            else if (cmbEstado.SelectedValue == null)
            {
                MessageBox.Show("Selecciona un estado para la habitacion");
                return false; 
            }
            return true;
  
        }
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            // verificar que se ingresaron los valores requeridos
            
            if(VerificarValores())
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

        private void ValoresFormUlarioDesdeObjeto()
        {
            txtDescripcion.Text = habitacion.Descripcion;
            txtNumeroHabitacion.Text = habitacion.Numero.ToString();
            cmbEstado.SelectedValue = habitacion.Estado;


        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (lbHabitaciones.SelectedValue == null)
                MessageBox.Show("Por favor selecciona una habitacion desde el listado");
            else
            {
                try
                {
                    //obtener informacion de la habitacion
                    habitacion = habitacion.BuscarHabitacion(Convert.ToInt32(lbHabitaciones.SelectedValue));

                    //llenar los valores  del formulario
                    ValoresFormUlarioDesdeObjeto();

                    //OCULTAR LOS BOTONES DE OPERACIONES crud
                    OcultarBotonesOperaciones(Visibility.Hidden);

                }
                catch(Exception ex)
                {
                    MessageBox.Show("ha ocurrido un error al modificar ");
                    Console.WriteLine(ex.Message);
                }
            }

        
        }

        private void btnCancelar_click(object sender, RoutedEventArgs e)
        {
            //mostrar los botones de operaciones CRUD
            OcultarBotonesOperaciones(Visibility.Visible);
            LimpiarFormulario();

        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (VerificarValores())
            {
                try
                {
                    //obtener los valores para la habitacion desde el formulario
                    ObtenerValoresFormulario();

                    //actualizar los valores en la base de datos
                    habitacion.ModificarHabitacion(habitacion);

                    //actualizar el listbox de habitaciones
                    ObtenerHabitaciones();

                    //mensaje de actualizacion
                    MessageBox.Show("habitacion modificada correctamente");

                    //mostrar botones otra vez
                    OcultarBotonesOperaciones(Visibility.Visible);

                    //limpiar
                    LimpiarFormulario();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar la habitacion");
                    Console.WriteLine(ex.Message);
                    
                }
                finally
                {
                    ObtenerHabitaciones();
                }
            }
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            //cerrar el formulario 
            this.Close();
        }
    }
}
