
using System;
using System.Security.Cryptography;

namespace UniCampus.BLL
{
    public class SeguridadPassword
    {
        private const int Iteraciones = 210000;
        private const int TamanoSalt = 16;
        private const int TamanoHash = 32;

        public static string GenerarHash(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(
                    "La contraseña no puede estar vacía.");
            }

            byte[] salt = new byte[TamanoSalt];

            using (RandomNumberGenerator generador =
                   RandomNumberGenerator.Create())
            {
                generador.GetBytes(salt);
            }

            byte[] hash;

            using (Rfc2898DeriveBytes pbkdf2 =
                   new Rfc2898DeriveBytes(
                       password,
                       salt,
                       Iteraciones,
                       HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(TamanoHash);
            }

            return "PBKDF2-SHA256$" +
                   Iteraciones + "$" +
                   Convert.ToBase64String(salt) + "$" +
                   Convert.ToBase64String(hash);
        }

        public static bool VerificarPassword(
            string password,
            string hashGuardado)
        {
            if (string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(hashGuardado))
            {
                return false;
            }

            try
            {
                string[] partes = hashGuardado.Split('$');

                if (partes.Length != 4 ||
                    partes[0] != "PBKDF2-SHA256")
                {
                    return false;
                }

                int iteraciones;

                if (!int.TryParse(partes[1], out iteraciones) ||
                    iteraciones < 100000 ||
                    iteraciones > 1000000)
                {
                    return false;
                }

                byte[] salt =
                    Convert.FromBase64String(partes[2]);

                byte[] hashOriginal =
                    Convert.FromBase64String(partes[3]);

                if (salt.Length != TamanoSalt ||
                    hashOriginal.Length != TamanoHash)
                {
                    return false;
                }

                byte[] hashCalculado;

                using (Rfc2898DeriveBytes pbkdf2 =
                       new Rfc2898DeriveBytes(
                           password,
                           salt,
                           iteraciones,
                           HashAlgorithmName.SHA256))
                {
                    hashCalculado =
                        pbkdf2.GetBytes(hashOriginal.Length);
                }

                int diferencia = 0;

                for (int i = 0; i < hashOriginal.Length; i++)
                {
                    diferencia |=
                        hashOriginal[i] ^ hashCalculado[i];
                }

                return diferencia == 0;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (CryptographicException)
            {
                return false;
            }
        }
    }
}
