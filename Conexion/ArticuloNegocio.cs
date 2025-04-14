using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Conexion
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> listaArticulo = new List<Articulo>();
            ConexionDatos datos = new ConexionDatos();
            try
            {
                datos.Consulta("select A.Id, Codigo, Nombre, A.Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio, M.Descripcion Marca, C.Descripcion Categoria from Articulos A, Marcas M, Categorias C where M.Id = IdMarca and C.Id = A.IdCategoria");
                datos.Lectura();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = Math.Round((decimal)datos.Lector["Precio"], 1);

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    listaArticulo.Add(aux);
                }
                return listaArticulo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { datos.CerrarCenexion(); }
        }
        public void Agregar(Articulo nuevo)
        {
            ConexionDatos datos = new ConexionDatos();
            try
            {
                datos.Consulta("insert into Articulos (Codigo, Nombre,Descripcion,ImagenUrl,Precio,IdMarca,IdCategoria) values (@Codigo,@Nombre,@Descripcion,@ImagenUrl,@Precio,@IdMarca,@IdCategoria)");
                datos.Parametro("@Codigo", nuevo.Codigo);
                datos.Parametro("@Nombre", nuevo.Nombre);
                datos.Parametro("@Descripcion", nuevo.Descripcion);
                datos.Parametro("@ImagenUrl", nuevo.ImagenUrl);
                datos.Parametro("@Precio", nuevo.Precio);
                datos.Parametro("@IdMarca", nuevo.Marca.Id);
                datos.Parametro("@IdCategoria", nuevo.Categoria.Id);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Modificar(Articulo articulo)
        {
            ConexionDatos datos = new ConexionDatos();
            try
            {
                datos.Consulta("update Articulos set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, ImagenUrl = @ImagenUrl, Precio = @Precio, IdMarca = @IdMarca, IdCategoria = @IdCategoria Where Id = @Id");
                datos.Parametro("@Codigo", articulo.Codigo);
                datos.Parametro("@Nombre", articulo.Nombre);
                datos.Parametro("@Descripcion", articulo.Descripcion);
                datos.Parametro("@ImagenUrl", articulo.ImagenUrl);
                datos.Parametro("@Precio", articulo.Precio);
                datos.Parametro("@IdMarca", articulo.Marca.Id);
                datos.Parametro("@IdCategoria", articulo.Categoria.Id);
                datos.Parametro("@Id", articulo.Id);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Eliminar(int id)
        {
            ConexionDatos datos = new ConexionDatos();
            try
            {
                datos.Consulta("delete from Articulos Where Id = @Id");
                datos.Parametro("@Id", id);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Articulo> FiltroAvanzado(string campo, string criterio, string filtro)
        {
            List<Articulo> listaArticulo = new List<Articulo>();
            ConexionDatos datos = new ConexionDatos();
            try
            {
                string consulta = "select A.Id, Codigo, Nombre, A.Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio, M.Descripcion Marca , C.Descripcion Categoria from Articulos A, Marcas M, Categorias C where M.Id = IdMarca and C.Id = A.IdCategoria and ";
                
                if (campo == "Codigo")
                {
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "Codigo like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "Codigo like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Codigo like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "Nombre like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Marca")
                {
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "M.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "M.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "M.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Categoria")
                {
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "C.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "C.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "C.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                datos.Consulta(consulta);
                datos.Lectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = Math.Round((decimal)datos.Lector["Precio"], 1);

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    listaArticulo.Add(aux);
                }
                return listaArticulo;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}
