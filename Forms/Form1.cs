using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Conexion;
using System.Runtime.InteropServices;

namespace Forms
{
    public partial class Form1 : Form
    {
        private List<Articulo> listaArticulo;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CargarDatos();
            cboCampo.Items.Add("Codigo");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Categoria");

        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltroAvanzado.Clear();
            CargarDatos();
        }
        private void dgvPrincipal_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPrincipal.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvPrincipal.CurrentRow.DataBoundItem;
                ValidarImagen(seleccionado.ImagenUrl);
            }
        }
        private void ValidarImagen(string imagen)
        {
            try
            {
                pbPrincipal.Load(imagen);
            }
            catch (Exception ex)
            {
                pbPrincipal.Load("https://www.medicaltourismcostarica.com/wp-content/uploads/2014/12/no-image.png");
            }
        }
        private void OcultarColumna()
        {
            dgvPrincipal.Columns["Id"].Visible = false;
            dgvPrincipal.Columns["ImagenUrl"].Visible = false;
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            NuevoArticulo nuevo = new NuevoArticulo();
            nuevo.ShowDialog();
            CargarDatos();
        }
        private void CargarDatos()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                
                listaArticulo = negocio.listar();
                dgvPrincipal.DataSource = listaArticulo;
                ValidarImagen(listaArticulo[0].ImagenUrl);
                OcultarColumna();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Articulo seleccionado;
                seleccionado = (Articulo)dgvPrincipal.CurrentRow.DataBoundItem;

                NuevoArticulo modificar = new NuevoArticulo(seleccionado);
                modificar.ShowDialog();
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            try
            {
                if(dgvPrincipal.CurrentRow != null)
                {
                    Articulo seleccionado = (Articulo)dgvPrincipal.CurrentRow.DataBoundItem;
                    DialogResult = MessageBox.Show("¿Desea Eliminar?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (DialogResult == DialogResult.Yes)
                    {
                        articuloNegocio.Eliminar(seleccionado.Id);
                        CargarDatos();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnDetalle_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPrincipal.CurrentRow != null)
                {
                    Articulo seleccionado = (Articulo)dgvPrincipal.CurrentRow.DataBoundItem;
                    VerDetalle detalle = new VerDetalle(seleccionado.Codigo, seleccionado.Nombre, seleccionado.ImagenUrl, seleccionado.Precio, seleccionado.Marca.ToString(), seleccionado.Categoria.ToString(), seleccionado.Descripcion);
                    detalle.ShowDialog();
                    CargarDatos();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void txtFiltroRapido_TextChanged(object sender, EventArgs e)
        {
            List<Articulo>listaFiltrada;
            string filtro = txtFiltroRapido.Text;
            try
            {
                if (filtro.Length > 2)
                {
                    listaFiltrada = listaArticulo.FindAll(x => x.Nombre.ToLower().Contains(filtro.ToLower()) || x.Codigo.ToLower().Contains(filtro.ToLower()) || x.Marca.ToString().ToLower().Contains(filtro.ToLower()));
                }
                else
                    listaFiltrada = listaArticulo;

                dgvPrincipal.DataSource = null;
                dgvPrincipal.DataSource = listaFiltrada;
                OcultarColumna();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnLimpiarRapido_Click(object sender, EventArgs e)
        {
            txtFiltroRapido.Clear();
        }
        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string opcion = cboCampo.SelectedItem.ToString();
                if (opcion != null)
                {
                    cboCriterio.Items.Clear();
                    cboCriterio.Items.Add("Empieza con");
                    cboCriterio.Items.Add("Termina con");
                    cboCriterio.Items.Add("Contiene");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool ValidadCombo()
        {
            if (cboCampo.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un campo.", "Campo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            if (cboCriterio.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un criterio.","Criterio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            if (txtFiltroAvanzado.Text.Length == 0)
            {
                MessageBox.Show("Ingrese lo que desea buscar.", "Error de busqueda", MessageBoxButtons.OK,MessageBoxIcon.Information);
                return true;
            }

            return false;
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();

                if (ValidadCombo())
                    return;

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;

                
                dgvPrincipal.DataSource = negocio.FiltroAvanzado(campo, criterio, filtro);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());                
            }
        }        
    }
}
