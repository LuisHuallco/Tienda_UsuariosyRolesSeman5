using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tienda_UsuariosS5.Model;
using Tienda_UsuariosS5.View.Usuarios;

namespace Tienda_UsuariosS5.Presenter
{
    public class UsuarioPresenter
    {
        private readonly IUsuarioView _vista;
        private readonly string _cadenaConexion = "server=localhost; uid=root; pwd=123; database=tienda_usuariosS5";

        public UsuarioPresenter(IUsuarioView vista)
        {
            _vista = vista;

            _vista.GuardarClicked += OnGuardar;
            _vista.EliminarClicked += OnEliminar;
            _vista.EditarClicked += OnEditar;
            _vista.SalirClicked += OnSalir;
            _vista.SeleccionarUsuario += OnSeleccionarUsuario;
            _vista.NuevoClicked += OnNuevo; // ✅ Evento del botón NUEVO

            CargarUsuarios();
        }

        private void OnGuardar(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_vista.Nombre) ||
                string.IsNullOrWhiteSpace(_vista.Correo) ||
                string.IsNullOrWhiteSpace(_vista.Clave) ||
                string.IsNullOrWhiteSpace(_vista.NombreRol) ||
                string.IsNullOrWhiteSpace(_vista.DetalleRol))
            {
                _vista.MostrarMensaje("Debe completar todos los campos.");
                return;
            }

            try
            {
                using (var cn = new MySqlConnection(_cadenaConexion))
                {
                    cn.Open();

                    int rolId = 0;
                    string checkRol = "SELECT RolId FROM roles WHERE NombreRol = @NombreRol AND Detalle = @Detalle";
                    using (var cmdCheck = new MySqlCommand(checkRol, cn))
                    {
                        cmdCheck.Parameters.AddWithValue("@NombreRol", _vista.NombreRol);
                        cmdCheck.Parameters.AddWithValue("@Detalle", _vista.DetalleRol);
                        var result = cmdCheck.ExecuteScalar();
                        if (result != null)
                        {
                            rolId = Convert.ToInt32(result);
                        }
                        else
                        {
                            string sqlRol = "INSERT INTO roles (NombreRol, Detalle) VALUES (@NombreRol, @Detalle)";
                            using (var cmdRol = new MySqlCommand(sqlRol, cn))
                            {
                                cmdRol.Parameters.AddWithValue("@NombreRol", _vista.NombreRol);
                                cmdRol.Parameters.AddWithValue("@Detalle", _vista.DetalleRol);
                                cmdRol.ExecuteNonQuery();
                                rolId = (int)cmdRol.LastInsertedId;
                            }
                        }
                    }

                    string sqlUser = "INSERT INTO usuarios (Nombre, Correo, Password, RolId) VALUES (@Nombre, @Correo, @Password, @RolId)";
                    using (var cmd = new MySqlCommand(sqlUser, cn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", _vista.Nombre);
                        cmd.Parameters.AddWithValue("@Correo", _vista.Correo);
                        cmd.Parameters.AddWithValue("@Password", _vista.Clave);
                        cmd.Parameters.AddWithValue("@RolId", rolId);
                        cmd.ExecuteNonQuery();
                    }
                }

                _vista.MostrarMensaje("Usuario guardado con éxito.");
                CargarUsuarios();
                _vista.LimpiarFormulario();
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje("Error al guardar: " + ex.Message);
            }
        }

        private void OnEliminar(object sender, EventArgs e)
        {
            if (_vista.UsuarioSeleccionado == null)
            {
                _vista.MostrarMensaje("Debe seleccionar un usuario.");
                return;
            }

            try
            {
                using (var cn = new MySqlConnection(_cadenaConexion))
                {
                    cn.Open();
                    string sql = "DELETE FROM usuarios WHERE UsuarioId = @id";
                    using (var cmd = new MySqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@id", _vista.UsuarioSeleccionado.UsuarioId);
                        cmd.ExecuteNonQuery();
                    }
                }

                _vista.MostrarMensaje("Usuario eliminado.");
                CargarUsuarios();
                _vista.LimpiarFormulario();
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje("Error al eliminar: " + ex.Message);
            }
        }

        private void OnEditar(object sender, EventArgs e)
        {
            if (_vista.UsuarioSeleccionado == null)
            {
                _vista.MostrarMensaje("Debe seleccionar un usuario.");
                return;
            }

            try
            {
                using (var cn = new MySqlConnection(_cadenaConexion))
                {
                    cn.Open();
                    string sql = "UPDATE usuarios SET Nombre=@Nombre, Correo=@Correo, Password=@Password WHERE UsuarioId=@UsuarioId";
                    using (var cmd = new MySqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", _vista.Nombre);
                        cmd.Parameters.AddWithValue("@Correo", _vista.Correo);
                        cmd.Parameters.AddWithValue("@Password", _vista.Clave);
                        cmd.Parameters.AddWithValue("@UsuarioId", _vista.UsuarioSeleccionado.UsuarioId);
                        cmd.ExecuteNonQuery();
                    }
                }

                _vista.MostrarMensaje("Usuario actualizado.");
                CargarUsuarios();
                _vista.LimpiarFormulario();
            }
            catch (Exception ex)
            {
                _vista.MostrarMensaje("Error al editar: " + ex.Message);
            }
        }

        private void OnSalir(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void OnSeleccionarUsuario(object sender, EventArgs e)
        {
            _vista.MostrarUsuarioSeleccionado();
        }

        private void OnNuevo(object sender, EventArgs e)
        {
            _vista.LimpiarFormulario();
        }

        private void CargarUsuarios()
        {
            var lista = new List<UsuarioModel>();
            using (var cn = new MySqlConnection(_cadenaConexion))
            {
                cn.Open();
                string sql = "SELECT u.UsuarioId, u.Nombre, u.Correo, u.Password, r.RolId, r.NombreRol, r.Detalle FROM usuarios u JOIN roles r ON u.RolId = r.RolId";
                using (var cmd = new MySqlCommand(sql, cn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new UsuarioModel
                        {
                            UsuarioId = reader.GetInt32("UsuarioId"),
                            Nombre = reader.GetString("Nombre"),
                            Correo = reader.GetString("Correo"),
                            Password = reader.GetString("Password"),
                            RolId = reader.GetInt32("RolId"),
                            Rol = new RolModel
                            {
                                RolId = reader.GetInt32("RolId"),
                                NombreRol = reader.GetString("NombreRol"),
                                Detalle = reader.GetString("Detalle")
                            }
                        });
                    }
                }
            }
            _vista.CargarUsuarios(lista);
        }
    }
}
