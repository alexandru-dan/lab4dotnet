using Lab1.Models;
using Lab1.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public interface IUserRoleService
    {
        IEnumerable<UserRoleGetModel> GetAll();
        UserRoleGetModel GetById(int id);
        UserRoleGetModel Create(UserRolePostModel userRolePostModel);
        UserRoleGetModel Upsert(int id, UserRolePostModel userRolePostModel);
        UserRoleGetModel Delete(int id);
    }
    public class UserRoleService : IUserRoleService
    {
        private DataDbContext context;
        public UserRoleService(DataDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<UserRoleGetModel> GetAll()
        {
            return context.UserRoles.Select(userRol => UserRoleGetModel.FromUserRole(userRol));
        }

        public UserRoleGetModel GetById(int id)
        {
            UserRole userRole = context.UserRoles
                                    .AsNoTracking()
                                    .FirstOrDefault(ur => ur.Id == id);

            return UserRoleGetModel.FromUserRole(userRole);
        }
        public UserRoleGetModel Create(UserRolePostModel userRolePostModel)
        {
            UserRole toAdd = UserRolePostModel.ToUserRole(userRolePostModel);

            context.UserRoles.Add(toAdd);
            context.SaveChanges();
            return UserRoleGetModel.FromUserRole(toAdd);
        }


        public UserRoleGetModel Upsert(int id, UserRolePostModel userRolePostModel)
        {
            var existing = context.UserRoles.AsNoTracking().FirstOrDefault(ur => ur.Id == id);
            if (existing == null)
            {
                UserRole toAdd = UserRolePostModel.ToUserRole(userRolePostModel);
                context.UserRoles.Add(toAdd);
                context.SaveChanges();
                return UserRoleGetModel.FromUserRole(toAdd);
            }

            UserRole toUpdate = UserRolePostModel.ToUserRole(userRolePostModel);
            toUpdate.Id = id;
            context.UserRoles.Update(toUpdate);
            context.SaveChanges();
            return UserRoleGetModel.FromUserRole(toUpdate);
        }


        public UserRoleGetModel Delete(int id)
        {
            var existing = context.UserRoles
                           .FirstOrDefault(ur => ur.Id == id);
            if (existing == null)
            {
                return null;
            }

            context.UserRoles.Remove(existing);
            context.SaveChanges();

            return UserRoleGetModel.FromUserRole(existing);
        }
    }
}
