using Lab1.Constants;
using Lab1.Models;
using Lab1.Validators;
using Lab1.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public interface IUsersService
    {
        LoginGetModel Authenticate(string username, string password);
        ErrorsCollection Register(RegisterPostModel registerInfo);
        User GetCurrentUser(HttpContext httpContext);
        IEnumerable<UserGetModel> GetAll();

        UserRole GetCurrentUserRole(User user);

        UserGetModel GetById(int id);
        ErrorsCollection Create(RegisterPostModel userModel);
        UserGetModel Upsert(int id, UserPostModel userPostModel);
        UserGetModel Delete(int id);
    }

    public class UsersService : IUsersService
    {
        private DataDbContext context;
        private readonly AppSettings appSettings;
        private IRegisterValidator registerValidator;
        private IUserToRoleServices userToRoleServices;

        public UsersService(DataDbContext context, IOptions<AppSettings> appSettings, IRegisterValidator registerValidator, IUserToRoleServices userToRoleServices)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.registerValidator = registerValidator;
            this.userToRoleServices = userToRoleServices;
        }

        public LoginGetModel Authenticate(string username, string password)
        {
            var user = context.Users
                .SingleOrDefault(x => x.Username == username &&
                                 x.Password == ComputeSha256Hash(password));

            // return null if user not found
            if (user == null)
                return null;

            string userRoleName = userToRoleServices.GetUserRoleNameById(user.Id);
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, userRoleName),
                    new Claim(ClaimTypes.UserData, user.DataRegistered.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var result = new LoginGetModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Token = tokenHandler.WriteToken(token)
            };
            // remove password before returning
            return result;
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            // TODO: also use salt
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public ErrorsCollection Register(RegisterPostModel registerInfo)
        {
            var errors = registerValidator.Validate(registerInfo, context);
            if (errors != null)
            {
                return errors;
            }

            User toAdd = new User
            {
                Email = registerInfo.Email,
                LastName = registerInfo.LastName,
                FirstName = registerInfo.FirstName,
                Password = ComputeSha256Hash(registerInfo.Password),
                Username = registerInfo.Username,
                DataRegistered = DateTime.Now,
                UserToRoles = new List<UserToRole>()
            };

            //se atribuie rolul de Regular ca default
            var regularRole = context
                .UserRoles
                .FirstOrDefault(u => u.Name == UserRoles.Regular);

            context.Users.Add(toAdd);
            context.UserToRoles.Add(new UserToRole
            {
                User = toAdd,
                UserRole = regularRole,
                StartTime = DateTime.Now,
                EndTime = null
            });

            context.SaveChanges();
            return null;
        }

        public User GetCurrentUser(HttpContext httpContext)
        {
            string username = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            //string accountType = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.AuthenticationMethod).Value;
            //return _context.Users.FirstOrDefault(u => u.Username == username && u.AccountType.ToString() == accountType);
            
            return context.Users.Include(u => u.UserToRoles).FirstOrDefault(u => u.Username == username);
        }

        public IEnumerable<UserGetModel> GetAll()

        {
            // return users without passwords
            return context.Users.Select(user => UserGetModel.FromUser(user));
        }
        public UserRole GetCurrentUserRole(User user)
        {
            return user
                .UserToRoles
                .FirstOrDefault(u => u.EndTime == null)
                .UserRole;
        }

        public UserGetModel GetById(int id)
        {
            User user = context.Users
                .FirstOrDefault(u => u.Id == id);

            return UserGetModel.FromUser(user);
        }

        public ErrorsCollection Create(RegisterPostModel userPostModel)
        {
            var errors = registerValidator.Validate(userPostModel, context);
            if (errors != null)
            {
                return errors;
            }

            User toAdd = RegisterPostModel.ToUser(userPostModel);

            //se atribuie rolul de Regular ca default
            var regularRole = context
                .UserRoles
                .FirstOrDefault(ur => ur.Name == UserRoles.Regular);

            context.Users.Add(toAdd);

            context.UserToRoles.Add(new UserToRole
            {
                User = toAdd,
                UserRole = regularRole,
                StartTime = DateTime.Now,
                EndTime = null
            });

            context.SaveChanges();
            return null;
        }



        public UserGetModel Upsert(int id, UserPostModel userPostModel)
        {
            var existing = context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
            if (existing == null)
            {
                User toAdd = UserPostModel.ToUser(userPostModel);
                context.Users.Add(toAdd);
                context.SaveChanges();
                return UserGetModel.FromUser(toAdd);
            }

            User toUpdate = UserPostModel.ToUser(userPostModel);
            toUpdate.Id = id;
            context.Users.Update(toUpdate);
            context.SaveChanges();
            return UserGetModel.FromUser(toUpdate);
        }


        public UserGetModel Delete(int id)
        {
            var existing = context.Users
                .FirstOrDefault(u => u.Id == id);
            if (existing == null)
            {
                return null;
            }

            context.Users.Remove(existing);
            context.SaveChanges();

            return UserGetModel.FromUser(existing);
        }

        }
}
