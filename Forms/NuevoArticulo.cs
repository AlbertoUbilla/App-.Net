using Conexion;
using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms
{
    public partial class NuevoArticulo: Form
    {
        Articulo articulo = null;
        public NuevoArticulo()
        {
            InitializeComponent();
            Text = "Agregar";
        }
        public NuevoArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar";
        }
        private void txtImagen_Leave(object sender, EventArgs e)
        {
            ValidarImagen(txtImagen.Text);
        }        
        private void ValidarImagen(string imagen)
        {
            try
            {
                pbAgregarModificar.Load(imagen);
            }
            catch (Exception ex)
            {
                pbAgregarModificar.Load("https://www.medicaltourismcostarica.com/wp-content/uploads/2014/12/no-image.png");
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void NuevoArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            cboMarca.DataSource = marcaNegocio.listar();
            cboMarca.ValueMember = "Id";
            cboMarca.DisplayMember = "Descripcion";

            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            cboCategoria.DataSource = categoriaNegocio.listar();
            cboCategoria.ValueMember = "Id";
            cboCategoria.DisplayMember = "Descripcion";

            if(articulo != null)
            {
                txtCodigo.Text = articulo.Codigo;
                txtNombre.Text = articulo.Nombre;
                txtDescripcion.Text = articulo.Descripcion;
                txtImagen.Text = articulo.ImagenUrl;
                ValidarImagen(articulo.ImagenUrl);
                txtPrecio.Text = articulo.Precio.ToString();
                cboMarca.SelectedValue = articulo.Marca.Id;
                cboCategoria.SelectedValue = articulo.Categoria.Id;
            }
        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if(articulo == null)
                    articulo = new Articulo();

                if (!ValidarCampos())
                    return;

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.ImagenUrl = txtImagen.Text;
                
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;

                if(articulo.Id != 0)
                {
                    negocio.Modificar(articulo);
                    MessageBox.Show("Modificado Exitosamente","Modificar",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else
                {
                    negocio.Agregar(articulo);
                    MessageBox.Show("Agregado Exitosamente","Agregar",MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //Validacion del txtPrecio con el evento KeyPress usando el codigo ASCII para limitar solo entradas numericas 
        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8)
            {
                e.Handled = true; // Cancela la tecla
            }
        }
        private bool ValidarCampos()
        {
            bool esValido = true;
            errorProvider1.Clear(); // Limpiamos errores anteriores

            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                errorProvider1.SetError(txtCodigo, "Campo requerido");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                errorProvider1.SetError(txtNombre, "Campo requerido");
                esValido = false;
            }                       
            
            if (string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                errorProvider1.SetError(txtPrecio, "Campo requerido");
                esValido = false;
            }
                        
            return esValido;
        }
    }
}
