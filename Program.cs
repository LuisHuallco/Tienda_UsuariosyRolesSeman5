using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tienda_UsuariosS5.Presenter;
using Tienda_UsuariosS5.View.Usuarios;

namespace Tienda_UsuariosS5
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var vista = new FRMUsuarios();
            var presenter = new UsuarioPresenter(vista);
            Application.Run(vista);

        }
    }
}
