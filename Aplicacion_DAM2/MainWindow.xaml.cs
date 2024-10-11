using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aplicacion_DAM2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //NO HACE FALTA COMENTAR EL CODIGO, EL NOMBRE DE LOS MÉTODOS HABLA POR SÍ SÓLO
        private void AltaDatos_Click(object sender, RoutedEventArgs e)
        {
            AltaDatos altaWindow = new AltaDatos();
            altaWindow.ShowDialog();
        }

        private void ConsultaDatos_Click(object sender, RoutedEventArgs e)
        {
            ConsultaDatos consultaWindow = new ConsultaDatos();
            consultaWindow.ShowDialog();
        }

        private void ActualizarDatos_Click(object sender, RoutedEventArgs e)
        {
            ActualizarDatos actualizarWindow = new ActualizarDatos();
            actualizarWindow.ShowDialog();
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Estás seguro de que deseas salir?", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void VerAyuda_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("AQUÍ VA LA AYUDA DE LA ALPICACIÓN", "Ayuda", MessageBoxButton.OK, MessageBoxImage.Information);
            Ayuda Ayuda = new Ayuda();
            Ayuda.ShowDialog();
        }

        private void AcercaDe_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Aplicación de gestión de empleados. Versión 1.0 \nCreada por: Diego Díaz Senovilla", "Acerca de", MessageBoxButton.OK, MessageBoxImage.Information);

            acercaDe acercaDe = new acercaDe();
            acercaDe.ShowDialog();
        }
    }
}