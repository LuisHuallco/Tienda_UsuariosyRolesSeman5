using System;
using System.Collections.Generic;
using Tienda_UsuariosS5.Model;

namespace Tienda_UsuariosS5.View.Usuarios
{
    public interface IUsuarioView
    {
        string Nombre { get; set; }
        string Correo { get; set; }
        string Clave { get; set; }

        // NUEVAS propiedades para rol
        string NombreRol { get; set; }
        string DetalleRol { get; set; }

        UsuarioModel UsuarioSeleccionado { get; }

        void MostrarMensaje(string mensaje);
        void LimpiarFormulario();
        void CargarUsuarios(List<UsuarioModel> usuarios);

        void MostrarUsuarioSeleccionado();

        event EventHandler GuardarClicked;
        event EventHandler EliminarClicked;
        event EventHandler EditarClicked;
        event EventHandler SalirClicked;
        event EventHandler SeleccionarUsuario;
        event EventHandler NuevoClicked; 

    }
}
