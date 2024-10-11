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
    
   
    public partial class ActualizarDatos : Window
    {
        private string archivoDatos = "datosEmpleados.txt"; // FICHERO EMPLEADOS GUARDADOS
        public ActualizarDatos()
        {
            InitializeComponent();
        }
        //AL DAR AL BOTÓN BUSCAR SE EJECUTA LA LECTURA DEL FICHERO Y LA Y SE METEN LOS TEXTOS SEGÚN LOS DATOS RECOGIDOS PARA EL CÓDIGO DE EMPLEADO ELEIGIDO
        private void BuscarDatos_Click(object sender, RoutedEventArgs e)
        {
            string idEmpleado = txtBuscarID.Text;

            if (string.IsNullOrWhiteSpace(idEmpleado))
            {
                MessageBox.Show("¡ERROR! Debe introducir un ID válido");
                return;
            }

            //METO TRYCATCH DE CONTROLD E ERRORES
            try
            {
                //LEEMOS TODAS LAS LINEAS DEL ARCHIVO
                string[] lines = File.ReadAllLines(archivoDatos);

                //BUSCAMOS AL EMPLEADO POR EL ID
                var empleado = BuscarEmpleadoPorID(lines, idEmpleado);

                if (empleado != null)
                {
                    //LLAMAMOS AL MÉTODO PARA RELLENAR EL FORMULARIO
                    RellenarFormulario(empleado);
                    DeshabilitarCampoCodigo(); //EL CAMPO DE CÓDIGO LO DEJO DESHABILITADO, ES EL ÚNICO QUE NO SE PODRÁ MODIFICAR Y LO DESHABILITO NADA MÁS CARGAR LOS DATOS
                }
                else
                {
                    MessageBox.Show("No se encontró ningún empleado con ese ID.");
                }
            }
            catch (Exception ex)//MENSAJE DE TRYCATCH ERROR INESPERADO ¿?
            {
                MessageBox.Show($"Error al buscar el empleado: {ex.Message}");
            }
        }

        //BUSCAMOS EL EMPLEADO POR ID
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

        // MÉTODO PARA RELLENAR EL FORMULARIO CON LOS DATOS DEL EMPLEADO
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
            }
        }

        // MÉTODO PARA OBTENER EL VALOR DE CADA LÍNEA
        private string ObtenerValor(string linea)
        {
            return linea.Split(':')[1].Trim();
        }

        // MÉTODO PARA DESHABILITAR SOLO EL CAMPO DEL CÓDIGO DE EMPLEADO
        private void DeshabilitarCampoCodigo()
        {
            txtCodigoEmpleado.IsEnabled = false;
        }

        // MÉTODO PARA ACTUALIZAR LOS DATOS Y GUARDARLOS EN EL ARCHIVO
        //CONTROLO DE NUEVO POSIBLES ERRORES
        private void ActualizarDatos_Click(object sender, RoutedEventArgs e)
        {
            string idEmpleado = txtCodigoEmpleado.Text;

            if (string.IsNullOrWhiteSpace(idEmpleado))
            {
                MessageBox.Show("El ID del empleado no es válido.");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(archivoDatos);
                var empleadoActualizado = ActualizarEmpleado(lines, idEmpleado);

                if (empleadoActualizado != null)
                {
                    // REEMPLAZAMOS EL EMPLEADO EN EL ARCHIVO
                    File.WriteAllLines(archivoDatos, empleadoActualizado);
                    MessageBox.Show("Datos actualizados correctamente.");
                    //AQUI LIMPIAMOS
                    LimpiarFormulario();
                }
                else
                {
                    MessageBox.Show("Error al actualizar los datos.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el fichero .txt: {ex.Message}");
            }
        }

        private void LimpiarFormulario()
        {
            //LIMPIEZA DE TODO AUNQUE LA FECHA EN NULL
            txtCodigoEmpleado.Text = "";
            txtNombreEmpleado.Text = "";
            dpAlta.SelectedDate = null; 
            txtAntiguedad.Text = "";
            RadioButtonYes.IsChecked = false;
            RadioButtonNo.IsChecked = false;
            txtEspecialidad.Text = "";
            txtTitulacion.Text = "";
            txtCategoriaL.Text = "";
            txtSalario.Text = "";
            txtPorcentaje.Text = "";
            cbDepartamento.SelectedIndex = -1; //EL -1 ME QUITA LA SELECCIÓN DEL COMBOBOX RECUERDA DIEGO
            cbGrado.SelectedIndex = -1;
            txtComentarios.Text = "";
            txtPremios.Text = "";
            txtBuscarID.Text = "Escribe aquí el ID...";
            txtBuscarID.Foreground = new SolidColorBrush(Colors.LightGray);
        }

        // ACTUALIZA LOS DATOS DEL EMPLEADO EN EL ARCHIVO
        private string[] ActualizarEmpleado(string[] lines, string idEmpleado)
        {
            var empleadoActualizado = new List<string>();
            bool encontrado = false;

            foreach (var line in lines)
            {
                if (line.StartsWith("Código Empleado:") && line.Contains(idEmpleado))
                {
                    encontrado = true;
                }

                if (!encontrado)
                {
                    empleadoActualizado.Add(line);
                }
                else
                {
                    if (line.StartsWith("-------------------------"))
                    {
                        // GUARDAMOS LOS NUEVOS DATOS ACTUALIZADOS
                        empleadoActualizado.Add($"Código Empleado: {txtCodigoEmpleado.Text}");
                        empleadoActualizado.Add($"Nombre: {txtNombreEmpleado.Text}");
                        empleadoActualizado.Add($"Alta: {dpAlta.SelectedDate.Value.ToShortDateString()}");
                        empleadoActualizado.Add($"Antigüedad: {txtAntiguedad.Text}");
                        empleadoActualizado.Add($"Tiempo Completo: {(RadioButtonYes.IsChecked == true ? "Sí" : "No")}");
                        empleadoActualizado.Add($"Especialidad: {txtEspecialidad.Text}");
                        empleadoActualizado.Add($"Titulación: {txtTitulacion.Text}");
                        empleadoActualizado.Add($"Categoría L: {txtCategoriaL.Text}");
                        empleadoActualizado.Add($"Salario: {txtSalario.Text}");
                        empleadoActualizado.Add($"Porcentaje: {txtPorcentaje.Text}");
                        empleadoActualizado.Add($"Departamento: {cbDepartamento.Text}");
                        empleadoActualizado.Add($"Grado: {cbGrado.Text}");
                        empleadoActualizado.Add($"Comentarios: {txtComentarios.Text}");
                        empleadoActualizado.Add($"Premios: {txtPremios.Text}");
                        empleadoActualizado.Add("-------------------------");

                        encontrado = false;
                    }
                }
            }

            return empleadoActualizado.ToArray();
        }

        //IGUAL QUE EN CONSULTADATOS
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
