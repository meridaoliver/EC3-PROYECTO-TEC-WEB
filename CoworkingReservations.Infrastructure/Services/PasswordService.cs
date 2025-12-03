using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using System.Text;

namespace CoworkingReservations.Infrastructure.Services
{
    public interface IPasswordService
    {
        string Hash(string password);
        bool Check(string hash, string password);
    }

    public class PasswordService : IPasswordService
    {
        public string Hash(string password)
        {
            // Usamos PBKDF2 (Estándar seguro)
            using var algorithm = new Rfc2898DeriveBytes(
                password,
                saltSize: 16,
                iterations: 10000,
                HashAlgorithmName.SHA256);

            var key = Convert.ToBase64String(algorithm.GetBytes(32));
            var salt = Convert.ToBase64String(algorithm.Salt);

            return $"{salt}.{key}";
        }

        public bool Check(string hash, string password)
        {
            var parts = hash.Split('.', 2);
            if (parts.Length != 2) return false;

            var salt = Convert.FromBase64String(parts[0]);
            var key = Convert.FromBase64String(parts[1]);

            using var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations: 10000,
                HashAlgorithmName.SHA256);

            var keyToCheck = algorithm.GetBytes(32);
            return keyToCheck.SequenceEqual(key);
        }
    }
}
