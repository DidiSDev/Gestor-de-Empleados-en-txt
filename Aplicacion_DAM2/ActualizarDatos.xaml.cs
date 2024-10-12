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
            string numeroEmpleado = txtNumeroEmpleado.Text;  //BUSCAMOS EL NUMERO DE EMPLEADO

            if (string.IsNullOrWhiteSpace(numeroEmpleado) || numeroEmpleado == "Num. de empleado o ID")
            {
                MessageBox.Show("¡ERROR! Debe introducir un ID válido");
                return;
            }

            // LIMPIAMOS FORMULARIO ANTES DE BUSCAR LOS NUEVOS DATOS, POR SI ACASO
            LimpiarFormulario();

            try
            {
                // LEEMOS TODAS LAS LINEAS DEL ARCHIVO
                string[] lines = File.ReadAllLines(archivoDatos);

                // BUSCAMOS AL EMPLEADO POR EL NÚMERO DE EMPLEADO
                var empleado = BuscarEmpleadoPorID(lines, numeroEmpleado);

                if (empleado != null)
                {
                    // LLAMAMOS AL MÉTODO PARA RELLENAR EL FORMULARIO
                    RellenarFormulario(empleado);
                    DeshabilitarCamposCodigoYID();  //CODIGO Y NUMERO NO PUEDEN MODIFICARSE
                }
                else
                {
                    MessageBox.Show("No se encontró ningún empleado con ese ID."); //ERROR QUE SI CONTROLO, NO HAY ESE ID
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar el empleado: {ex.Message}"); //ERROR NO CONTROLADO
            }
        }

        //BUSCAMOS EL EMPLEADO POR ID
        private string[] BuscarEmpleadoPorID(string[] lines, string numeroEmpleado)
        {
            var empleado = new StringBuilder();
            bool encontrado = false;

            foreach (var line in lines)
            {
                if (line.StartsWith("Número Empleado:") && line.Contains(numeroEmpleado)) //NUEVA IMPLEMENTACIÓN, NUM EMPLEADO QUE EN MI VERSIÓN ANTERIOR NO ESTABA
                {
                    encontrado = true;
                }

                if (encontrado)
                {
                    empleado.AppendLine(line);
                }

                if (line.StartsWith("-------------------------") && encontrado)
                {
                    break; //EL INTERRUPTOR NOS SACA SI HEMOS ENCONTRADO AL SUSODICHO
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
                else if (linea.StartsWith("Número Empleado:"))  // NUMERO DE EMPLEADO SE RELLENA
                {
                    txtNumeroEmpleado.Text = ObtenerValor(linea);
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

            DeshabilitarCamposCodigoYID();  //DESHABILITAMOS CAMPOS AL CARGAR (ID Y CODIGO)
        }
        private void DeshabilitarCamposCodigoYID()
        {
            txtCodigoEmpleado.IsEnabled = false;
            txtNumeroEmpleado.IsEnabled = false; 
        }


        //EL SPLIT ES CANSADO HACERLO TODO EL TIEMPO, MODULARIZAMOS
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
            // LIMPIEZA DE TODO AUNQUE LA FECHA EN NULL
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
            cbDepartamento.SelectedIndex = -1;
            cbGrado.SelectedIndex = -1;
            txtComentarios.Text = "";
            txtPremios.Text = "";

            // A VECES EL GOTFOCUS Y LOSTFOCUS NO FUNCIONA UNA VEZ HMOS PULSADO ACTUALIZAR DATOS, PERO SIGUE SIENDO FUNCIONAL AUNQUE LAS LÍNEAS SE BORREN A MANO
            txtNumeroEmpleado.IsEnabled = true;
            txtNumeroEmpleado.Text = "Num. de empleado o ID"; 
            txtNumeroEmpleado.Foreground = new SolidColorBrush(Colors.LightGray);
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
                        // GUARDAMOS LOS NUEVOS DATOS ACTUALIZADOS SIN MODIFICAR CÓDIGO Y NÚMERO DE EMPLEADO
                        //PERO ESTA VEZ COMO YA TENEMOS EL NUMERO DE EMPLEADO (ID) YA NO ES NECESARIO INTRODUCIRLO DE NUEVO, PORQUE SI LA LINEA EMPIEZA CON CODIGO EMPLEADO..

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
            txtNumeroEmpleado.IsEnabled = true;
            txtNumeroEmpleado.Focus();
            return empleadoActualizado.ToArray();
            
        }

        //IGUAL QUE EN CONSULTADATOS
        private void TxtBuscarID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtNumeroEmpleado.Text == "Num. de empleado o ID")
            {
                txtNumeroEmpleado.Text = "";
                txtNumeroEmpleado.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TxtBuscarID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumeroEmpleado.Text))
            {
                txtNumeroEmpleado.Text = "Num. de empleado o ID";
                txtNumeroEmpleado.Foreground = new SolidColorBrush(Colors.LightGray);
            }
        }
    }
}
