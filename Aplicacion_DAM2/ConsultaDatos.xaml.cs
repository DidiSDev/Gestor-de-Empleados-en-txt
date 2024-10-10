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
    
    public partial class ConsultaDatos : Window
    {
        private string archivoDatos = "datosEmpleados.txt"; // FICHERO EMPLEADOS GUARDADOS

        public ConsultaDatos()
        {
            InitializeComponent();
        }

        // AL DAR AL BOTÓN BUSCAR SE EJECUTA LA LECTURA DEL FICHERO Y LA IMPLANTACIÓN DE LOS TEXTOS SEGÚN LOS DATOS RECOGIDOS PARA EL CODIGO DE EMPLEADO: X ELEGIDO
        //POR EL USUARIO
        private void BuscarDatos_Click(object sender, RoutedEventArgs e)
        {
            string idEmpleado = txtBuscarID.Text;

            if (string.IsNullOrWhiteSpace(idEmpleado))
            {
                MessageBox.Show("Por favor, introduce un ID válido.");
                return;
            }

            try
            {
                // LEEMOS TODAS LAS LINEAS
                string[] lines = File.ReadAllLines(archivoDatos);

                // BU8SCAMOS AL EMPLEADO
                var empleado = BuscarEmpleadoPorID(lines, idEmpleado);

                if (empleado != null)
                {
                    // LLAMAMOS AL METODO DE RELLENAR FORMULARIO CON EL EMPLEADO RECOGIDO
                    RellenarFormulario(empleado);
                }
                else
                {
                    MessageBox.Show("No se encontró ningún empleado con el ID proporcionado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar el empleado: {ex.Message}");
            }
        }

        // BUSCAMOS AL EMPLEADOPOR ID
        private string[] BuscarEmpleadoPorID(string[] lines, string idEmpleado)
        {
            var empleado = new StringBuilder();
            bool encontrado = false;

            foreach (var line in lines)
            {
                if (line.StartsWith("Código Empleado:") && line.Contains(idEmpleado))
                {
                    encontrado = true;
                }

                if (encontrado)
                {
                    empleado.AppendLine(line);
                }

                if (line.StartsWith("-------------------------") && encontrado)
                {
                    break; // Finalizamos la búsqueda cuando encontramos el final del bloque
                }
            }

            return encontrado ? empleado.ToString().Split('\n') : null;
        }

        private void RellenarFormulario(string[] empleado)
        {
            foreach (var linea in empleado)
            {
                if (linea.StartsWith("Código Empleado:"))
                {
                    txtCodigoEmpleado.Text = ObtenerValor(linea);
                }
                else if (linea.StartsWith("Nombre:"))
                {
                    txtNombreEmpleado.Text = ObtenerValor(linea);
                }
                else if (linea.StartsWith("Alta:"))
                {
                    dpAlta.SelectedDate = DateTime.Parse(ObtenerValor(linea));
                }
                else if (linea.StartsWith("Antigüedad:"))
                {
                    txtAntiguedad.Text = ObtenerValor(linea);
                }
                else if (linea.StartsWith("Tiempo Completo:"))
                {
                    if (ObtenerValor(linea) == "Sí")
                    {
                        RadioButtonYes.IsChecked = true;
                    }
                    else
                    {
                        RadioButtonNo.IsChecked = true;
                    }
                }
                else if (linea.StartsWith("Especialidad:"))
                {
                    txtEspecialidad.Text = ObtenerValor(linea);
                }
                else if (linea.StartsWith("Titulación:"))
                {
                    txtTitulacion.Text = ObtenerValor(linea);
                }
                else if (linea.StartsWith("Categoría L:"))
                {
                    txtCategoriaL.Text = ObtenerValor(linea);
                }
                else if (linea.StartsWith("Salario:"))
                {
                    txtSalario.Text = ObtenerValor(linea);
                }
                else if (linea.StartsWith("Porcentaje:"))
                {
                    txtPorcentaje.Text = ObtenerValor(linea);
                }
                else if (linea.StartsWith("Departamento:"))
                {
                    cbDepartamento.Text = ObtenerValor(linea);
                }
                else if (linea.StartsWith("Grado:"))
                {
                    cbGrado.Text = ObtenerValor(linea);
                }
                else if (linea.StartsWith("Comentarios:"))
                {
                    txtComentarios.Text = ObtenerValor(linea);
                }
                else if (linea.StartsWith("Premios:"))
                {
                    txtPremios.Text = ObtenerValor(linea);
                }
                else if (linea.StartsWith("Total:"))
                {
                    txtTotalNomina.Text = ObtenerValor(linea);
                }
            }
        }

        // MODULARIZAMOS EL SPLIT PORQUE SI NO LO TENGO QUE HACER 80 VECES
        private string ObtenerValor(string linea)
        {
            return linea.Split(':')[1].Trim();
        }

        // MÉTODOS DEL FOCUS PARA BUSCAR ID, QUEDA INTERESANTE
        private void TxtBuscarID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBuscarID.Text == "Escribe aquí el ID...")
            {
                txtBuscarID.Text = "";
                txtBuscarID.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TxtBuscarID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscarID.Text))
            {
                txtBuscarID.Text = "Escribe aquí el ID...";
                txtBuscarID.Foreground = new SolidColorBrush(Colors.LightGray);
            }
        }
    }
}
