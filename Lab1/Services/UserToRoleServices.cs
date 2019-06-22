using Lab1.Models;
using Lab1.Validators;
using Lab1.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public interface IUserToRoleServices
    {
        IQueryable<UserToRoleGetModel> GetHistoryRoleById(int id);
        ErrorsCollection Create(UserToRolePostModel userToRolePostModel);

        string GetUserRoleNameById(int id);
    }
    public class UserToRoleServices : IUserToRoleServices
    {
        private DataDbContext context;
        private IUserRoleValidator userRoleValidator;

        public UserToRoleServices (DataDbContext context, IUserRoleValidator userRoleValidator)
        {
            this.context = context;
            this.userRoleValidator = userRoleValidator;
        }

        public IQueryable<UserToRoleGetModel> GetHistoryRoleById(int id)
        {
            IQueryable<UserToRole> userUserRole = context.UserToRoles
                                    .Include(u => u.UserRole)
                                    .AsNoTracking()
                                    .Where(uur => uur.UserId == id)
                                    .OrderBy(uur => uur.StartTime);

            return userUserRole.Select(uur => UserToRoleGetModel.FromUserToRole(uur));
        }

        public string GetUserRoleNameById(int id)
        {
            int userRoleId = context.UserToRoles
               .AsNoTracking()
                .FirstOrDefault(uur => uur.UserId == id && uur.EndTime == null)
                .UserRoleId;

            string numeRol = context.UserRoles
                  .AsNoTracking()
                  .FirstOrDefault(ur => ur.Id == userRoleId)
                  .Name;

            return numeRol;
        }



        public ErrorsCollection Create(UserToRolePostModel userUserRolePostModel)
        {
            var errors = userRoleValidator.Validate(userUserRolePostModel, context);
            if (errors != null)
            {
                return errors;
            }

            User user = context.Users
                .FirstOrDefault(u => u.Id == userUserRolePostModel.UserId);

            if (user != null)
            {
                UserRole userRole = context
                               .UserRoles
                               .Include(u => u.UserToRole)
                               .FirstOrDefault(u => u.Name == userUserRolePostModel.UserRoleName);

                UserToRole curentUserToRole = context.UserToRoles
                                .Include(ur => ur.UserRole)
                                .FirstOrDefault(ur => ur.UserId == user.Id && ur.EndTime == null);


                if (curentUserToRole == null)
                {
                    context.UserToRoles.Add(new UserToRole
                    {
                        User = user,
                        UserRole = userRole,
                        StartTime = DateTime.Now,
                        EndTime = null
                    });

                    context.SaveChanges();
                    return null;
                }

                if (!curentUserToRole.UserRole.Name.Contains(userUserRolePostModel.UserRoleName))
                {
                    curentUserToRole.EndTime = DateTime.Now;

                    context.UserToRoles.Add(new UserToRole
                    {
                        User = user,
                        UserRole = userRole,
                        StartTime = DateTime.Now,
                        EndTime = null
                    });

                    context.SaveChanges();
                    return null;
                }
                else
                {
                    return null;    
                }
            }
            return null;    

        }
    }
}
