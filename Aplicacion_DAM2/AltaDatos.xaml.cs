using Microsoft.Win32;
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
using System.IO;

namespace Aplicacion_DAM2
{
    /// <summary>
    /// Lógica de interacción para AltaDatos.xaml
    /// </summary>
    public partial class AltaDatos : Window
    {

        //GUARDAMOS LOS DATOS AQUÍ
        private string archivoDatos = "datosEmpleados.txt";
        public AltaDatos()
        {
            InitializeComponent();
            GenerarCodigoEmpleado();
            
        }

       
      

        private void GuardarDatos_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigoEmpleado.Text) ||
        string.IsNullOrWhiteSpace(txtNombreEmpleado.Text) || // OBLIGATORIO NOMBRE DE EMPLEADO Y SALARIO, EL CODIGO TAMBIEN PERO SE PONE SÓLO, AÚN ASÍ LO VALIDAM,OS
        string.IsNullOrWhiteSpace(txtSalario.Text))
            {
                MessageBox.Show("¡Error, es obligatorio rellenar Código, Nombre y Salario!");
                return;
            }

            string datosEmpleado = $"Código Empleado: {txtCodigoEmpleado.Text}\n" +
                           $"Nombre: {txtNombreEmpleado.Text}\n" +  // Guardar el nombre
                           $"Alta: {dpAlta.SelectedDate?.ToString("d") ?? "No especificado"}\n" +
                           $"Antigüedad: {txtAntiguedad.Text ?? "No especificado"}\n" +
                           $"Tiempo Completo: {(RadioButtonYes.IsChecked == true ? "Sí" : "No")}\n" +
                           $"Especialidad: {txtEspecialidad.Text ?? "No especificado"}\n" +
                           $"Titulación: {txtTitulacion.Text ?? "No especificado"}\n" +
                           $"Categoría L: {txtCategoriaL.Text ?? "No especificado"}\n" +
                           $"Salario: {txtSalario.Text ?? "No especificado"}\n" +
                           $"Porcentaje: {txtPorcentaje.Text ?? "No especificado"}\n" +
                           $"Departamento: {cbDepartamento.Text ?? "No especificado"}\n" +
                           $"Grado: {cbGrado.Text ?? "No especificado"}\n" +
                           $"Comentarios: {txtComentarios.Text ?? "Sin comentarios"}\n" +
                           $"Premios: {txtPremios.Text ?? "Ninguno"}\n" +
                           $"-------------------------";

            try
            {
                File.AppendAllText(archivoDatos, datosEmpleado + Environment.NewLine);
                MessageBox.Show($"Se ha guardado correctamente al empleado {txtNombreEmpleado.Text}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar los datos: {ex.Message}");
            }

            LimpiarFormulario();
        }


        //DEBAJO EL MÉTODO QUE TRAERÁ AUTOMÁTICAMENTE EL ULTIMO CÓDIGO DE EMPLEADO DEL FICHERO +1
        private void GenerarCodigoEmpleado()
        {
            int maxCodigo = 0;

            // VERIFICO Q EL FICHERO EXISTE, EN CASO CONTRARIO CODIGO=1
            if (File.Exists(archivoDatos))
            {
                // LEO EL ARCHIVO ENTERO GUARDADO EN UN ARRAY QUE USAMOS TEMPORALMENTE
                string[] lines = File.ReadAllLines(archivoDatos);

                // Busca el número más alto de empleado en el archivo
                foreach (var line in lines)
                {
                    if (line.StartsWith("Código Empleado:"))
                    {
                        // RECORREMOS, CUANDO EL PUNTERO ESTÉ EN LA LINEA QUE EMPIEZA POR "Código Empleado:" RECOGERÁ EL NÚMERO CON SPLIT
                        string[] parts = line.Split(':');
                        if (parts.Length > 1 && int.TryParse(parts[1].Trim(), out int codigoEmpleado))
                        {
                            if (codigoEmpleado > maxCodigo)
                            {
                                maxCodigo = codigoEmpleado;
                            }
                        }
                    }
                }
            }
            else
            {
                //NO EXISTE EL FICHERO PUES EL PRIMER CODIGO SERA EL 0 QUE AÑADIRÉ +1 DEBAJO DEL ELSE
                maxCodigo = 0;
            }

  
            int siguienteCodigoEmpleado = maxCodigo + 1;

            // NO FUNCIONA ESTO POR ALGUNA RAZÓN DESCONOCIDA
            txtCodigoEmpleado.Text = siguienteCodigoEmpleado.ToString();
            //YA LO ENTIENDO, NO PUEDO DEJAR EN EL XAML ENABLED=FALSE PORQUE ENTONCES EL NUMERO NO SE METE, TENGO QUE HACERLO DESPUÉS DE PONER EL CODIGO, DEBAJO DE ESTA LINEA
            txtCodigoEmpleado.IsEnabled = false;
        }

        
        private void LimpiarFormulario()
        {
            txtNombreEmpleado.Text = "";
            txtCodigoEmpleado.Text = "";
            dpAlta.SelectedDate = null;
            txtAntiguedad.Text = "";
            RadioButtonYes.IsChecked = false;
            RadioButtonNo.IsChecked = false;
            txtEspecialidad.Text = "";
            txtTitulacion.Text = "";
            txtCategoriaL.Text = "";
            txtSalario.Text = "";
            txtPorcentaje.Text = "";
            cbDepartamento.SelectedIndex = -1;
            cbGrado.SelectedIndex = -1;
            txtComentarios.Text = "";
            txtPremios.Text = "";
            
        }

        private void TxtNombreEmpleado_GotFocus(object sender, RoutedEventArgs e)
        {
            // FUNCIONALIDAD SOBRE LA CAJA DEL NOMBRE QUE QUEDARÁ VACÍA SI HACEMOS CLICK SOBRE ELLA
            if (txtNombreEmpleado.Text == "Escribe el nombre...")
            {
                txtNombreEmpleado.Text = "";
                txtNombreEmpleado.Foreground = Brushes.Black; //TEXTO EN NEGRO SE VE MEJOR
            }
        }

        private void TxtNombreEmpleado_LostFocus(object sender, RoutedEventArgs e)
        {
            // SI QUITAMOS EL FOCO, DEJAMOS DE NUEVO LO QUE HABÍA Y PONEMOS EL TEXTO EN GRIS OSCURO (TIPO DISABLED)
            if (string.IsNullOrWhiteSpace(txtNombreEmpleado.Text))
            {
                txtNombreEmpleado.Text = "Escribe el nombre...";
                txtNombreEmpleado.Foreground = Brushes.LightGray;
            }
        }


    }
}
