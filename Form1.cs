using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using iText.Forms;
using iText.Forms.Fields;
using iText.Forms.Form.Element;
using iText.Kernel.Pdf;

namespace LearnUnity
{
    public partial class Form1 : Form
    {
        private ToolTip toolTip;

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        //Plantilla
        string plantilla;

        //--Datos del cliente--
        string nombreCompleto,
            nif,
            domicilio,
            provincia,
            correo,
            codigoPostal,
            localidad,
            telefono;

        //--Datos de la instalación--
        string emplazamiento,
            uso,
            cups,
            localidadInstalacion,
            instalacion,
            numeroPortal,
            codigoPostalInstalacion;

        string tipoDeInstalacion,
            provinciaInstalacion;
        private SizeF formOriginalSize;

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            this.TopMost = true; // lo pone arriba al inicio
            this.Activate(); // le da el foco
            this.TopMost = false;

            toolTip = new ToolTip();
            toolTip.AutoPopDelay = 3000; // Mostrar por 3 segundos
            toolTip.InitialDelay = 100; // Aparece rápido
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true; // Mostrar incluso si el control está deshabilitado
            toolTip.BackColor = Color.FromArgb(127, 133, 24, 147); // Fondo del tooltip
            toolTip.ForeColor = Color.White;
            this.FormClosing += Form1_FormClosing;
            panel1.MouseDown += panel1_Paint;
        }

        /// <summary>
        /// Datos Clientes COMIENZA
        /// </summary>
        ///
        private void Notify(TextBox obj, string textShow, int permitted)
        {
            string caracteresPermitidos = "";
            switch (permitted)
            {
                case 0:
                    caracteresPermitidos = "[^a-zA-ZáéíóúÁÉÍÓÚñÑ ]";
                    break;
                case 1:
                    caracteresPermitidos = "[^0-9]";
                    break;
                case 2:
                    caracteresPermitidos = "[^a-zA-Z0-9 ]";
                    break;
            }

            //caso 0 solo letras
            //caso 1 solo numeros
            //caso 2 letras y numeros

            toolTip.Show(textShow, obj, obj.Width - 20, obj.Height, 3000);

            obj.Text = Regex.Replace(obj.Text, caracteresPermitidos, "");
            obj.SelectionStart = obj.Text.Length;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox1.Text, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]*$"))
                Notify(textBox1, "Solo se permiten letras.", 0);

            nombreCompleto = textBox1.Text;
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox2.Text, @"^[a-zA-Z0-9]*$"))
                Notify(textBox2, "Solo se permiten letras y numeros.", 2);

            nif = textBox2.Text;
        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {
            domicilio = textBox3.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            correo = textBox5.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox4.Text, @"^\d{0,5}$"))
            {
                Notify(textBox4, "Solo se permiten numeros.", 1);
            }
            else
            {
                codigoPostal = textBox4.Text.PadLeft(5, '0');
            }
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox13.Text, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]*$"))
                Notify(textBox13, "Solo se permiten letras.", 0);
            localidad = textBox13.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox6.Text, @"^\d{0,5}$"))
            {
                Notify(textBox6, "Solo se permiten numeros.", 1);
            }
            else
            {
                telefono = textBox6.Text.PadLeft(5, '0');
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            provincia = comboBox1.SelectedItem.ToString();
        }

        /// <summary>
        /// Datos Clientes FIN
        /// </summary>



        /// <summary>
        /// Datos Instalacion COMIENZA
        /// </summary>

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            tipoDeInstalacion = comboBox2.SelectedItem.ToString();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                instalacion = "Nueva";
            else
                instalacion = "";
            Console.WriteLine("Instalación radio 1 : " + instalacion);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                instalacion = "Ampliación";
            else
                instalacion = "";
            Console.WriteLine("Instalación radio 2 : " + instalacion);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                instalacion = "Modificación";
            else
                instalacion = "";

            Console.WriteLine("Instalación radio 3 : " + instalacion);
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox10.Text, @"^\d{0,5}$"))
            {
                Notify(textBox10, "Solo se permiten numeros.", 1);
            }
            else
            {
                codigoPostalInstalacion = textBox10.Text;
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            numeroPortal = textBox9.Text;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            emplazamiento = textBox7.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox8.Text, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]*$"))
                Notify(textBox8, "Solo se permiten letras.", 0);
            localidadInstalacion = textBox8.Text;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            provinciaInstalacion = comboBox3.SelectedItem.ToString();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            uso = textBox11.Text;
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            cups = textBox12.Text;
        }

        /// <summary>
        /// Datos Instalacion FIN
        /// </summary>


        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            formOriginalSize = this.Size;

            float scale = CalcularFactorDeEscala();

            // Escalamos el formulario
            this.Size = new Size(
                (int)(formOriginalSize.Width * scale),
                (int)(formOriginalSize.Height * scale)
            );

            // Escalamos todos los controles
            EscalarControles(this, scale);

            this.TopMost = true; // lo pone arriba al inicio
            this.Activate(); // le da el foco
            this.TopMost = false;
        }

        private void EscalarControles(Control parent, float scale)
        {
            foreach (Control ctrl in parent.Controls)
            {
                // Escalamos tamaño
                ctrl.Width = (int)(ctrl.Width * scale);
                ctrl.Height = (int)(ctrl.Height * scale);

                // Escalamos posición
                ctrl.Left = (int)(ctrl.Left * scale);
                ctrl.Top = (int)(ctrl.Top * scale);

                // Escalamos fuente
                ctrl.Font = new Font(ctrl.Font.FontFamily, ctrl.Font.Size * scale, ctrl.Font.Style);

                // Escalamos hijos recursivamente
                if (ctrl.HasChildren)
                    EscalarControles(ctrl, scale);
            }
        }

        private float CalcularFactorDeEscala()
        {
            // Resolución de la pantalla donde está el formulario
            Screen screen = Screen.FromControl(this);
            int screenWidth = screen.WorkingArea.Width;
            int screenHeight = screen.WorkingArea.Height;

            // Resolución base en la que diseñaste tu formulario
            int baseWidth = 2550;
            int baseHeight = 1440;

            // Calculamos el factor de escala según ancho y alto
            float scaleX = (float)screenWidth / baseWidth;
            float scaleY = (float)screenHeight / baseHeight;

            // Elegimos el menor para que todo quepa en pantalla
            return Math.Min(scaleX, scaleY);
        }

        private void RestaurarColorProgresivamente(Control control, double segundos = 2)
        {
            Timer timer = new Timer();
            int pasos = 20; // más pasos = transición más suave
            timer.Interval = (int)(segundos * 1000 / pasos); // calcula el tiempo por paso
            int pasoActual = 0;

            timer.Tick += (s, e) =>
            {
                pasoActual++;
                int r =
                    control.BackColor.R + (255 - control.BackColor.R) / (pasos - pasoActual + 1);
                int g =
                    control.BackColor.G + (255 - control.BackColor.G) / (pasos - pasoActual + 1);
                int b =
                    control.BackColor.B + (255 - control.BackColor.B) / (pasos - pasoActual + 1);

                control.BackColor = Color.FromArgb(r, g, b);

                if (pasoActual >= pasos)
                {
                    control.BackColor = Color.White;
                    timer.Stop();
                    timer.Dispose();
                }
            };

            timer.Start();
        }

        private bool ValidarCamposObligatorios()
        {
            List<string> errores = new List<string>();
            List<Control> camposMarcados = new List<Control>(); // Para recordar cuáles marcar
            bool todoCorrecto = true;

            void MarcarCampo(Control control, bool valido)
            {
                if (!valido)
                {
                    control.BackColor = Color.LightCoral;
                    camposMarcados.Add(control);
                    todoCorrecto = false;
                }
            }

            // Validaciones de cliente
            MarcarCampo(textBox1, !string.IsNullOrWhiteSpace(nombreCompleto));
            if (string.IsNullOrWhiteSpace(nombreCompleto))
                errores.Add("Cliente → Nombre completo es obligatorio.");

            MarcarCampo(textBox2, !string.IsNullOrWhiteSpace(nif));
            if (string.IsNullOrWhiteSpace(nif))
                errores.Add("Cliente → NIF es obligatorio.");

            MarcarCampo(textBox3, !string.IsNullOrWhiteSpace(domicilio));
            if (string.IsNullOrWhiteSpace(domicilio))
                errores.Add("Cliente → Domicilio es obligatorio.");

            MarcarCampo(
                textBox4,
                !string.IsNullOrWhiteSpace(codigoPostal) && Regex.IsMatch(codigoPostal, @"^\d{5}$")
            );
            if (string.IsNullOrWhiteSpace(codigoPostal) || !Regex.IsMatch(codigoPostal, @"^\d{5}$"))
                errores.Add("Cliente → Código postal debe tener 5 dígitos.");

            MarcarCampo(textBox8, !string.IsNullOrWhiteSpace(localidad));
            if (string.IsNullOrWhiteSpace(localidad))
                errores.Add("Cliente → Localidad es obligatoria.");

            MarcarCampo(comboBox1, !string.IsNullOrWhiteSpace(provincia));
            if (string.IsNullOrWhiteSpace(provincia))
                errores.Add("Cliente → Provincia es obligatoria.");

            MarcarCampo(textBox5, !string.IsNullOrWhiteSpace(correo));
            if (string.IsNullOrWhiteSpace(correo))
                errores.Add("Cliente → Correo es obligatorio.");

            MarcarCampo(textBox6, !string.IsNullOrWhiteSpace(telefono));
            if (string.IsNullOrWhiteSpace(telefono))
                errores.Add("Cliente → Teléfono es obligatorio.");

            // Validaciones de instalación
            MarcarCampo(textBox7, !string.IsNullOrWhiteSpace(emplazamiento));
            if (string.IsNullOrWhiteSpace(emplazamiento))
                errores.Add("Instalación → Emplazamiento es obligatorio.");

            MarcarCampo(textBox9, !string.IsNullOrWhiteSpace(numeroPortal));
            if (string.IsNullOrWhiteSpace(numeroPortal))
                errores.Add("Instalación → Número de portal es obligatorio.");

            MarcarCampo(
                textBox10,
                !string.IsNullOrWhiteSpace(codigoPostalInstalacion)
                    && Regex.IsMatch(codigoPostalInstalacion, @"^\d{5}$")
            );
            if (
                string.IsNullOrWhiteSpace(codigoPostalInstalacion)
                || !Regex.IsMatch(codigoPostalInstalacion, @"^\d{5}$")
            )
                errores.Add("Instalación → Código postal debe tener 5 dígitos.");
            MarcarCampo(comboBox3, !string.IsNullOrWhiteSpace(provinciaInstalacion));
            if (string.IsNullOrWhiteSpace(provinciaInstalacion))
                errores.Add("Instalacion → Provincia es obligatoria.");
            MarcarCampo(comboBox2, !string.IsNullOrWhiteSpace(tipoDeInstalacion));
            if (string.IsNullOrWhiteSpace(tipoDeInstalacion))
                errores.Add("Instalación → Tipo de instalación es obligatorio.");
            MarcarCampo(textBox13, !string.IsNullOrWhiteSpace(localidadInstalacion));
            if (string.IsNullOrWhiteSpace(localidadInstalacion))
                errores.Add("Instalación → Localidad de Instalación es obligatorio.");
            MarcarCampo(textBox11, !string.IsNullOrWhiteSpace(uso));
            if (string.IsNullOrWhiteSpace(uso))
                errores.Add("Instalación → Uso es obligatorio.");

            if (string.IsNullOrWhiteSpace(instalacion))
                errores.Add("Instalación → Debe seleccionar una opción de instalación.");

            // Si hay errores, mostrar
            if (errores.Count > 0)
            {
                MessageBox.Show(
                    string.Join("\n", errores),
                    "Campos obligatorios faltantes",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                // Restaurar progresivamente colores después de cerrar el mensaje
                foreach (var campo in camposMarcados)
                {
                    RestaurarColorProgresivamente(campo);
                }

                return false;
            }

            return true;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposObligatorios())
                return;

            string outputPath;

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Archivos PDF|*.pdf";
                saveDialog.FileName = "CertificadoEléctrico.pdf";
                if (saveDialog.ShowDialog() != DialogResult.OK)
                    return;
                outputPath = saveDialog.FileName;
                MessageBox.Show("PDF guardado en: " + outputPath);
            }
            switch (instalacion)
            {
                case "Nueva":
                    plantilla = "Certificaciones.Plantilla_Nueva.pdf"; //LearnUnity. PARA DEBUGS
                    //Certificaciones. PARA COMPILADOS
                    break;
                case "Ampliación":
                    plantilla = "Certificaciones.Plantilla_Ampliacion.pdf";
                    break;
                case "Modificación":
                    plantilla = "Certificaciones.Plantilla_Modificacion.pdf";
                    break;
            }

            using (
                Stream plantillaStream = Assembly
                    .GetExecutingAssembly()
                    .GetManifestResourceStream(plantilla)
            )
            {
                if (plantillaStream == null)
                {
                    MessageBox.Show(
                        "Hubo un problema. "
                            + "(Codigo de error x0001)"
                            + " Contacte con el administrador"
                    );
                    return;
                }
                using (PdfReader reader = new PdfReader(plantillaStream))
                using (PdfWriter writer = new PdfWriter(outputPath))
                using (PdfDocument pdf = new PdfDocument(reader, writer))
                {
                    PdfAcroForm form = PdfFormCreator.GetAcroForm(pdf, true);
                    var fields = form.GetAllFormFields();

                    Console.WriteLine("Campos encontrados: " + fields.Count);
                    int index = 1;
                    foreach (var field in fields)
                    {
                        var formField = fields[field.Key];

                        switch (index)
                        { //Datos finales Clientes-
                            case 32:
                                formField.SetValue(nombreCompleto);
                                break;
                            case 30:
                                formField.SetValue(nif);
                                break;
                            case 31:
                                formField.SetValue(domicilio);
                                break;
                            case 33:
                                formField.SetValue(codigoPostal);
                                break;
                            case 5:
                                formField.SetValue(localidad);
                                break;
                            case 64:
                                formField.SetValue(provincia);
                                break;
                            case 34:
                                formField.SetValue(correo);
                                break;
                            case 35:
                                formField.SetValue(telefono);
                                break;
                            //Datos finales -Instalacion
                            case 6:
                                formField.SetValue(emplazamiento);
                                break;
                            case 38:
                                formField.SetValue(numeroPortal);
                                break;
                            case 12:
                                formField.SetValue(localidadInstalacion);
                                break;
                            case 63:
                                formField.SetValue(provinciaInstalacion);
                                break;
                            case 37:
                                formField.SetValue(codigoPostalInstalacion);
                                break;
                            case 65:
                                formField.SetValue(tipoDeInstalacion);
                                break;
                            case 13:
                                formField.SetValue(uso);
                                break;
                            case 15:
                                formField.SetValue(cups);
                                break;
                        }

                        formField.GetPdfObject().Put(PdfName.Q, new PdfNumber(0));
                        index++;
                    }

                    // form.FlattenFields();
                }
            }

            System.Diagnostics.Process.Start(outputPath);
            MessageBox.Show(
                "Certificado generado con éxito.",
                "Éxito",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Recorremos todos los TextBox del formulario
            bool hayDatos = this
                .Controls.OfType<TextBox>()
                .Any(tb => !string.IsNullOrWhiteSpace(tb.Text));

            if (hayDatos)
            {
                DialogResult result = MessageBox.Show(
                    "Está seguro que quiere salir? Se perderán los campos escritos.",
                    "Confirmar salida",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void iconPictureBox1_Click_1(object sender, EventArgs e)
        {
            toolTip.Show("Para cualquier duda, contacte con el administrador.", iconPictureBox1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e) { }

        private void textBox3_TextChanged(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void label1_Click_1(object sender, EventArgs e) { }

        private void label19_Click(object sender, EventArgs e) { }

        private void panel1_Paint(object sender, PaintEventArgs e) { }

        private void panel1_Paint(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void label21_Click(object sender, EventArgs e) { }

        private void label7_Click(object sender, EventArgs e) { }

        private void label22_Click(object sender, EventArgs e) { }

        private void label3_Click(object sender, EventArgs e) { }

        private void label4_Click(object sender, EventArgs e) { }

        private void label5_Click(object sender, EventArgs e) { }

        private void label6_Click(object sender, EventArgs e) { }

        private void label8_Click(object sender, EventArgs e) { }

        private void iconPictureBox1_Click(object sender, EventArgs e) { }

        private void label8_Click_1(object sender, EventArgs e) { }

        private void label8_Click_2(object sender, EventArgs e) { }

        private void label11_Click(object sender, EventArgs e) { }

        private void label16_Click(object sender, EventArgs e) { }

        private void label18_Click(object sender, EventArgs e) { }
    }
}
