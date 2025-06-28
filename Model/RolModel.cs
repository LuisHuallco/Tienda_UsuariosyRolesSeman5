using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda_UsuariosS5.Model
{
    public class RolModel
    {
        public int RolId { get; set; }
        public string NombreRol { get; set; }
        public string Detalle { get; set; } // ✅ propiedad faltante

        public override string ToString()
        {
            return NombreRol;
        }
    }
}
