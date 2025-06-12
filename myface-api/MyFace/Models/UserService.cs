using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MyFace.Models.Database;
using System.Linq;
using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace MyFace
{
    public interface IUserService
    {
        Task<User> ValidateUserAsync(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly MyFaceDbContext _context;

        public UserService(MyFaceDbContext context)
        {
            _context = context;
        }

        public async Task<User> ValidateUserAsync(string username, string password)
        {
            var user = await _context.Users
                .SingleAsync(user => user.Username == username);
            if (user == null) return null;

            var storedUsername = user.Username;
            var storedHashedPassword = user.HashedPassword;
            var storedSalt = user.Salt;

            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: storedSalt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

            if (storedHashedPassword == hashedPassword && username == storedUsername)
            {
                return user;
            }
            else
            {
                return null;
            }

        }

    }
}