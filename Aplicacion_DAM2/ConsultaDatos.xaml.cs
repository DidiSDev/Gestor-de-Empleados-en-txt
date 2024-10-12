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

        public void deshabilitarCampos()
        {
            //MÉTODO PARA DESHABILITAR LOS CAMPOS QUE SEA LLAMADO TRAS SER RELLENADOS DEL FICHERO, SI NO NOS E RELLENAN
            
            txtNombreEmpleado.IsEnabled = false;
            txtCodigoEmpleado.IsEnabled = false;
            dpAlta.IsEnabled = false;
            txtAntiguedad.IsEnabled = false;
            RadioButtonNo.IsEnabled = false;
            RadioButtonYes.IsEnabled = false;
            txtEspecialidad.IsEnabled = false;
            txtTitulacion.IsEnabled = false;
            txtCategoriaL.IsEnabled = false;
            txtSalario.IsEnabled = false;
            txtPorcentaje.IsEnabled = false;
            cbDepartamento.IsEnabled = false;
            cbGrado.IsEnabled = false;
            txtComentarios.IsEnabled = false;
            txtPremios.IsEnabled = false;
            txtTotalNomina.IsEnabled = false; //AÚN NO SÉ QUÉ DEBE IR EN TOTAL...

            /*TENGO QUE DESHABILITAR TODOS ESTOS CAMPOS
             * 
             * txtNombreEmpleado.Text = "";
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
            txtPremios.Text = "";*/
        }

        // AL DAR AL BOTÓN BUSCAR SE EJECUTA LA LECTURA DEL FICHERO Y LA IMPLANTACIÓN DE LOS TEXTOS SEGÚN LOS DATOS RECOGIDOS PARA EL CODIGO DE EMPLEADO: X ELEGIDO
        //POR EL USUARIO

        private void BuscarDatos_Click(object sender, RoutedEventArgs e)
        {
            string numeroEmpleado = txtBuscarID.Text;

            if (string.IsNullOrWhiteSpace(numeroEmpleado))
            {
                MessageBox.Show("Por favor, introduce un número de empleado válido.");
                return;
            }

            try
            {
                // LEEMOS TODAS LAS LINEAS DEL ARCHIVO
                string[] lines = File.ReadAllLines(archivoDatos);

                // BUSCAMOS AL EMPLEADO POR NÚMERO DE EMPLEADO
                var empleado = BuscarEmpleadoPorNumero(lines, numeroEmpleado);

                if (empleado != null)
                {
                    // LLENAMOS EL FORMULARIO CON LOS DATOS DEL EMPLEADO
                    RellenarFormulario(empleado);
                    CalcularTotalNomina();
                }
                else
                {
                    MessageBox.Show("No se encontró ningún empleado con el número proporcionado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar el empleado: {ex.Message}");
            }
        }
        private string[] BuscarEmpleadoPorNumero(string[] lines, string numeroEmpleado)
        {
            var empleado = new StringBuilder();
            bool encontrado = false;

            foreach (var line in lines)
            {
                if (line.StartsWith("Número Empleado:") && line.Contains(numeroEmpleado))
                {
                    encontrado = true;
                }

                if (encontrado)
                {
                    empleado.AppendLine(line);
                }

                if (line.StartsWith("-------------------------") && encontrado)
                {
                    break; //CON EL INTERRUPTOR DEJAMOS DE RECORRER EL .TXT COMO BUENA PRÁCTICA SI HEMOS ENCONTRADO EL QUE SOLICITA EL USUARIO
                }
            }

            return encontrado ? empleado.ToString().Split('\n') : null;
        }

        private void CalcularTotalNomina()
        {
            if (decimal.TryParse(txtSalario.Text, out decimal salario) &&
                decimal.TryParse(txtPorcentaje.Text, out decimal porcentaje))
            {
                //CALCULAMOS EL CAMPO TOTAL DEL SALARIO, EL % TIENE QUE RESTARSE AL SALARIO ME DIJO ISA
                decimal total = salario - (salario * (porcentaje / 100));
                txtTotalNomina.Text = total.ToString("F2"); //TOTAL Y HASTA 2 DECIMALES (PARA ESO EL "F2)
            }
            else
            {
                txtTotalNomina.Text = "¡Error!"; //SI LOS VALORES NO SON CORRECTOS (METE LETRAS O COSAS ASÍ), ERROR
            }
        }

        // BUSCAMOS AL EMPLEADOPOR ID
        

        private void RellenarFormulario(string[] empleado)
        {
            foreach (var linea in empleado)
            {
                //AQUÍ RECORREMOS A PARTIR DE CODIGO DE EMPLEADO, DEBIDO A QUE EL NUMERO DE EMPLEADO YA HA BUSCADO EL DATO, Y SOLO SE HACE UNA VEZ, EL ARRAY SOLAMENTE TIENE
                //GUARDADO UN EMPLEADO
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

            deshabilitarCampos();
        }

        // MODULARIZAMOS EL SPLIT PORQUE SI NO LO TENGO Q HACER 80 VECES
        private string ObtenerValor(string linea)
        {
            return linea.Split(':')[1].Trim();
        }

        // MÉTODOS DEL FOCUS PARA BUSCAR ID, QUEDA INTERESANTE PARA QUITAR LAS LETRAS AL HACER CLICK Y RECORDAR SI PIERDE FOCUS Y NO HAY NÚMERO INSERTADO  
        private void TxtBuscarID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBuscarID.Text == "Num. de empleado o ID")
            {
                txtBuscarID.Text = ""; 
                txtBuscarID.Foreground = new SolidColorBrush(Colors.Black); 
            }
        }

        private void TxtBuscarID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscarID.Text))
            {
                txtBuscarID.Text = "Num. de empleado o ID";
                txtBuscarID.Foreground = new SolidColorBrush(Colors.LightGray); 
            }
        }

    }
}
