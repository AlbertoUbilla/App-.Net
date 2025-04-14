using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms
{
    public partial class VerDetalle: Form
    {
        public VerDetalle()
        {
            InitializeComponent();
        }
        public VerDetalle(string cod,string nom,string img, decimal precio, string marca, string categoria, string desc)
        {
            InitializeComponent();

            lblCodResultado.Text = cod;
            lblNomResultado.Text = nom;
            lblPrecioResultado.Text = "$ " + precio.ToString();
            lblMarResultado.Text = marca;
            lblCatResultado.Text = categoria;
            lblDescResultado.Text = desc;

            ValidarImagen(img);
           
            Text = "Detalle";
        }
        private void ValidarImagen(string imagen)
        {
            try
            {
                pictureBox1.Load(imagen);
            }
            catch (Exception ex)
            {
                pictureBox1.Load("https://www.medicaltourismcostarica.com/wp-content/uploads/2014/12/no-image.png");
            }
        }
        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
