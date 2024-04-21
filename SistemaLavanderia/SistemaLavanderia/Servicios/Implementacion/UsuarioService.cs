using Microsoft.EntityFrameworkCore;
using SistemaLavanderia.Models;
using SistemaLavanderia.Servicios.Contrato;

namespace SistemaLavanderia.Servicios.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly LavanderiaContext _context;

        public UsuarioService(LavanderiaContext context)
        {
            _context = context;
        }
        public async Task<Usuario> GetUsuarios(string usuario, string clave)
        {
            Usuario usuario_encontrado = await _context.Usuarios.Where(t => t.Usuario1 == usuario && t.Clave == clave).FirstOrDefaultAsync();
            return usuario_encontrado;
        }

        public async Task<Usuario> Saveusuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}
