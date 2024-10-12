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

    public partial class AltaDatos : Window
    {

        //GUARDAMOS LOS DATOS AQUÍ
        private string archivoDatos = "datosEmpleados.txt";
        public AltaDatos()
        {
            InitializeComponent();
            GenerarNumeroYCodigoEmpleado();

        }




        private void GuardarDatos_Click(object sender, RoutedEventArgs e)
        {
            //COMO CAMPOS ESENCIALES A RELLENAR DEJO EL NUMERO Y CODIGO Q SE GENERAN SOLOS, EVITANDO QUE SE REPITAN NUNCA Y ALEATORIAMENTE/SECUENCIALMENTE RESPECTIVAMENTE
            //EL NOMBRE POR SUPUESTO, EL SALARIO Y LA FECHA (LA FECHA PORQUE SINO DA UN ERROR AL LEERLA LUEGO SI NO ESTÁ BIEN ESCRITA)
            if (string.IsNullOrWhiteSpace(txtNumeroEmpleado.Text) ||
                string.IsNullOrWhiteSpace(txtCodigoEmpleado.Text) ||
                string.IsNullOrWhiteSpace(txtNombreEmpleado.Text) ||
                string.IsNullOrWhiteSpace(txtSalario.Text) ||
                !dpAlta.SelectedDate.HasValue)
            {
                MessageBox.Show("¡ERROR! Debes rellenar los campos: Número, Código, Nombre, Salario, Fecha de Alta.");
                return;
            }

            // RECOGEMOS TODOS LOS DATOS, AÑADIENDO LA ÚLTIMA MODIFICACIÓN (EL CÓDIGO DE EMPLEADO GENERADO ALEATORIAMENTE CON 3 LETRAS Y 3 NUYMEROS)
            string datosEmpleado = $"Número Empleado: {txtNumeroEmpleado.Text}\n" +
                                   $"Código Empleado: {txtCodigoEmpleado.Text}\n" +
                                   $"Nombre: {txtNombreEmpleado.Text}\n" +
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
                                   $"---------------------------------------------------------------------"; //BARRA SEPARADORA PARA CADA EMPLEADO EN EL TXTR

            //GUARDAMOS EN EL FICHERO EL STRING CON TODOS LOS DATOS SEPARADOS SECUENCIALMENTE
            try
            {
                File.AppendAllText(archivoDatos, datosEmpleado + Environment.NewLine);
                MessageBox.Show($"Se ha guardado correctamente al empleado {txtNombreEmpleado.Text}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar los datos: {ex.Message}");
            }

            //LIMPIAMOS EL FORMULARIO TRAS PULSAR EL BOTÓN GUARDAR EMPLEADO.
            LimpiarFormulario();
        }



        //DEBAJO EL MÉTODO QUE TRAERÁ AUTOMÁTICAMENTE EL ULTIMO CÓDIGO DE EMPLEADO DEL FICHERO +1
        private void GenerarNumeroYCodigoEmpleado()
        {
            int maxNumeroEmpleado = 0;

            //VERIFICAMOS FICHERO, SI NO EXISTE X Y SI EXISTE Y
            if (File.Exists(archivoDatos))
            {
                //SE LEE, EL MEJOR MÉTODO QUE HE ENCONTRADO ES GUARDAR EN UN ARRAY ESTÁTICO DE DATOS TODAS LAS LÍNEAS Y LUEGO UN FOREACH LAS RECORRE, ASÍ NO TENGO QUE
                //CONTROLAR EL PUNTERO LECTOR YO
                string[] lines = File.ReadAllLines(archivoDatos);

                //EL NÚMERO MÁS SALTO SERÁ +1 PARA AÑADIR EL NUEVO, QUE ESTARÁ DISABLED POR SUPUESTO
                foreach (var line in lines)
                {
                    if (line.StartsWith("Número Empleado:"))
                    {
                        string[] parts = line.Split(':');
                        if (parts.Length > 1 && int.TryParse(parts[1].Trim(), out int numeroEmpleado))
                        {
                            if (numeroEmpleado > maxNumeroEmpleado)
                            {
                                maxNumeroEmpleado = numeroEmpleado;
                            }
                        }
                    }
                }
            }

            //+1
            int siguienteNumeroEmpleado = maxNumeroEmpleado + 1;
            txtNumeroEmpleado.Text = siguienteNumeroEmpleado.ToString();

            //GENERAMOS CÓDIGO ALEATORIO 3 LETRAS3 NUMEROS Q NO SE REPETIRÁN
            string codigoAleatorio;
            do
            {
                codigoAleatorio = GenerarCodigoAleatorio(); //MODULARIZAMOS, EL CODIGO SE GENERARÁ MIENTRAS EL BOOLEANO RETORNE QUE EXISTE ESE CÓDIGO (AUNQUE VEO DIFÍCIL QUE SE REPITAN)
            } while (CodigoYaExiste(codigoAleatorio));

            txtCodigoEmpleado.Text = codigoAleatorio;
            txtCodigoEmpleado.IsEnabled = false;
        }

        private string GenerarCodigoAleatorio()
        {
            Random random = new Random();
            string letras = new string(Enumerable.Repeat("ABCDEFGHIJKLMNÑOPQRSTUVWXYZ", 3) //EL 3 ES PARA RECORRER 3 VECES ALEATORIAMENTE EL ARRAY Y SELECCIONAR 3 LETRAS
                              .Select(s => s[random.Next(s.Length)]).ToArray());

            string numeros = random.Next(100, 999).ToString(); //EN ESTE CASO NO CONTROLO QUE EL CÓDIGO EMPIECE POR 0-99 NUNCA, PERO SÍ ENTRE 100 Y 999
            return letras + numeros;
        }

        //COMPROBAMOS SI EL CÓDIGO YA EXISTE O NO EN EL FICHERO, EN CASO DE EXISTIR GENERAMOS UNO NUEVO
        private bool CodigoYaExiste(string codigo)
        {
            if (!File.Exists(archivoDatos)) return false;

            string[] lines = File.ReadAllLines(archivoDatos);
            return lines.Any(line => line.Contains($"Código Empleado: {codigo}"));
        }


        private void LimpiarFormulario()
        {
            txtNumeroEmpleado.Text = ""; //AÑADO EL NUMERO DE EMPLEADO QUE ANTES NO LO TENÍA
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

            //REGENERAMOS EL NUEVO NUMERO Y CODIGO DE EMPLEADO PARA LA SIGUIENTE ALTA, DE FORMA AUTOMÁTICA Y SECUENCIAL
            GenerarNumeroYCodigoEmpleado();
        }

        private void TxtNombreEmpleado_GotFocus(object sender, RoutedEventArgs e)
        {
            // GOTFOCUS ME PERMITIRÁ LIMPIAR LA CAJA EN EL MOMENTO QUE EL CLICK DEL USUARIO SE POSE SOBRE ESTA CAJITA
            if (txtNombreEmpleado.Text == "Escribe el nombre...")
            {
                txtNombreEmpleado.Text = "";
                txtNombreEmpleado.Foreground = Brushes.Black;
            }
        }

        private void TxtNombreEmpleado_LostFocus(object sender, RoutedEventArgs e)
        {
            // LOST FOCUS HACE QUE SI QUITAMOS EL FOCO DEL CLICK, VUELVA A APARECER -> "Escribe el nombre..."
            if (string.IsNullOrWhiteSpace(txtNombreEmpleado.Text))
            {
                txtNombreEmpleado.Text = "Escribe el nombre...";
                txtNombreEmpleado.Foreground = Brushes.LightGray;
            }
        }


    }
}
