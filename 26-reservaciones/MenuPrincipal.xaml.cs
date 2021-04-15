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
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : Window
    {
        public MenuPrincipal(string usuario)
        {
            InitializeComponent();

            //monstrar nombre en el formulario
            lblUsuario.Content = string.Format("Hola {0} ¿Que deseas realizar?", usuario);
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            //Retonar el usuario al fomulario inicial
            IniciarSesion iniciarSesion = new IniciarSesion();
            iniciarSesion.Show();
            Close();
        }
    }
}
