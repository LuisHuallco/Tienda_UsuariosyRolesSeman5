using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tienda_UsuariosS5.Model;

namespace Tienda_UsuariosS5.View.Usuarios
{
    public partial class FRMUsuarios : Form, IUsuarioView
    {
        public FRMUsuarios()
        {
            InitializeComponent();

            btnGuardar.Click += (s, e) => GuardarClicked?.Invoke(this, EventArgs.Empty);
            btnEliminar.Click += (s, e) => EliminarClicked?.Invoke(this, EventArgs.Empty);
            btnEditar.Click += (s, e) => EditarClicked?.Invoke(this, EventArgs.Empty);
            btnSalir.Click += (s, e) => SalirClicked?.Invoke(this, EventArgs.Empty);
            btnNuevo.Click += (s, e) => NuevoClicked?.Invoke(this, EventArgs.Empty); // ← NUEVO
            lstUsuarios.SelectedIndexChanged += (s, e) => SeleccionarUsuario?.Invoke(this, EventArgs.Empty);
        }

        public string Nombre { get => txtNombre.Text; set => txtNombre.Text = value; }
        public string Correo { get => txtCorreo.Text; set => txtCorreo.Text = value; }
        public string Clave { get => txtClave.Text; set => txtClave.Text = value; }
        public string NombreRol { get => txtRol.Text; set => txtRol.Text = value; }
        public string DetalleRol { get => txtDetalle.Text; set => txtDetalle.Text = value; }

        public UsuarioModel UsuarioSeleccionado => lstUsuarios.SelectedItem as UsuarioModel;

        public void MostrarMensaje(string mensaje) => MessageBox.Show(mensaje);

        public void LimpiarFormulario()
        {
            Nombre = "";
            Correo = "";
            Clave = "";
            NombreRol = "";
            DetalleRol = "";
            lstUsuarios.ClearSelected();
        }

        public void CargarUsuarios(List<UsuarioModel> usuarios)
        {
            lstUsuarios.DataSource = null;
            lstUsuarios.DataSource = usuarios;
            lstUsuarios.DisplayMember = null; // aseguramos que se actualice
            lstUsuarios.DisplayMember = "Display"; // Mostrar: Nombre - Rol
        }

        public void MostrarUsuarioSeleccionado()
        {
            if (UsuarioSeleccionado != null)
            {
                Nombre = UsuarioSeleccionado.Nombre;
                Correo = UsuarioSeleccionado.Correo;
                Clave = UsuarioSeleccionado.Password;
                NombreRol = UsuarioSeleccionado.Rol?.NombreRol ?? "";
                DetalleRol = UsuarioSeleccionado.Rol?.Detalle ?? "";
            }
        }

        public event EventHandler GuardarClicked;
        public event EventHandler EliminarClicked;
        public event EventHandler EditarClicked;
        public event EventHandler SalirClicked;
        public event EventHandler SeleccionarUsuario;
        public event EventHandler NuevoClicked; // ← NUEVO
    }
}
