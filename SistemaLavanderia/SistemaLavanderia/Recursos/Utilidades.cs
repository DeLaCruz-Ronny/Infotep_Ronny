using System.Security.Cryptography;
using System.Text;

namespace SistemaLavanderia.Recursos
{
    public class Utilidades
    {
        public static string EncriptarClave(string clave)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] ressult = hash.ComputeHash(enc.GetBytes(clave));
                foreach (byte b in ressult)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
