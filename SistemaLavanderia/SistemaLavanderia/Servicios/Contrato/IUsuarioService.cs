using SistemaLavanderia.Models;

namespace SistemaLavanderia.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuarios(string usuario, string clave);
        Task<Usuario> Saveusuario(Usuario usuario);
    }
}
