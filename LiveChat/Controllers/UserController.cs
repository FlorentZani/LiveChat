using LiveChat.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace LiveChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        public UserController (DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> Get()
        {
            var Users = await _context.Users.ToListAsync();
            var UsersData = Users.Select(Users => new 
            {
                Id = Users.Id,
                Name = Users.Name,
                LastName = Users.LastName,
                Email = Users.Email
                
            });
            return Ok(UsersData);
        }

        //Declaring a function for hashing the password
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }


        [HttpPost]
        public async Task<ActionResult> AddUser (UserDTO request)
        {
            byte[] passwordHash, passwordSalt; 
            CreatePasswordHash(request.Password,out passwordHash, out passwordSalt);

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt

            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("id")]
        public async Task<ActionResult> UpdateUser(Guid id,UserDTO updatedUser)
        {
            var user = await _context.Users.FindAsync(id);

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(updatedUser.Password, out passwordHash, out passwordSalt);

            user.Name = updatedUser.Name;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;


            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("id")]

        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return BadRequest("User not found!");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User delted successfully!");
        }





    }
}
