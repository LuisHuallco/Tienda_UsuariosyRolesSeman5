using System;

namespace Tienda_UsuariosS5.Model
{
    public class UsuarioModel
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public int RolId { get; set; }
        public RolModel Rol { get; set; }  // Esta propiedad debe existir

        public override string ToString()
        {
            return $"{Nombre} - {Rol?.NombreRol}";
        }

    }
}
