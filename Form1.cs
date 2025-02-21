using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Examen1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos CSV (*.csv)|*.csv",
                Title = "Seleccionar archivo CSV"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                CargarCSV(openFileDialog.FileName);
            }

        }


        private void CargarCSV(string filePath)
        {
            List<Persona> personas = new List<Persona>();

            try
            {
                var lineas = File.ReadAllLines(filePath);
                foreach (var linea in lineas)
                {
                    var datos = linea.Split(',');

                    if (datos.Length >= 2)
                    {
                        string curp = datos[0].Trim();
                        if (decimal.TryParse(datos[1], out decimal promedio))
                        {
                            int edad = CalcularEdadDesdeCURP(curp);
                            string sexo = ObtenerSexoDesdeCURP(curp);

                            personas.Add(new Persona
                            {
                                CURP = curp,
                                Promedio = promedio,
                                Edad = edad,
                                Sexo = sexo
                            });
                        }
                    }
                }

                dataGridView1.DataSource = personas;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer el archivo: " + ex.Message);
            }
        }

        private int CalcularEdadDesdeCURP(string curp)
        {
            try
            {
                string fechaNacimiento = curp.Substring(4, 6);
                string siglo = (fechaNacimiento[0] == '0' || fechaNacimiento[0] == '1') ? "20" : "19";
                DateTime fecha = DateTime.ParseExact(siglo + fechaNacimiento, "yyyyMMdd", CultureInfo.InvariantCulture);
                int edad = DateTime.Now.Year - fecha.Year;
                if (DateTime.Now < fecha.AddYears(edad)) edad--; 
                return edad;
            }
            catch
            {
                return 0; 
            }
        }

        private string ObtenerSexoDesdeCURP(string curp)
        {
            if (curp.Length > 10)
            {
                char sexo = curp[10]; 
                return (sexo == 'H') ? "Hombre" : (sexo == 'M') ? "Mujer" : "Desconocido";
            }
            return "Desconocido";
        }
    }

    public class Persona
    {
        public string CURP { get; set; }
        public decimal Promedio { get; set; }
        public int Edad { get; set; }
        public string Sexo { get; set; }
    }
}
