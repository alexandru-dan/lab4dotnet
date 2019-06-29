using Lab1.Models;
using Lab1.Services;
using Lab1.Validators;
using Lab1.ViewModels;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestService
{
    class UserToRoleServiceTest
    {
        [Test]
        public void GetByIdShouldReturnUserRole()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnUserRole))
              .Options;

            using (var context = new DataDbContext(options))
            {
                var userUserRolesService = new UserToRoleServices(context, null);

                User userToAdd = new User
                {
                    Email = "user@yahoo.com",
                    LastName = "Ion",
                    FirstName = "POpescu",
                    Password = "secret",
                    DataRegistered = DateTime.Now,
                    UserToRoles = new List<UserToRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRole = new UserRole
                {
                    Name = "Rol testare",
                    Description = "Creat pentru testare"
                };
                context.UserRoles.Add(addUserRole);
                context.SaveChanges();

                context.UserToRoles.Add(new UserToRole
                {
                    User = userToAdd,
                    UserRole = addUserRole,
                    StartTime = DateTime.Now,
                    EndTime = null
                });
                context.SaveChanges();

                var userUserRoleGetModels = userUserRolesService.GetHistoryRoleById(1);
                Assert.IsNotNull(userUserRoleGetModels.FirstOrDefaultAsync(uur => uur.EndTime == null));
            }
        }



        [Test]
        public void CreateShouldAddTheUserUserRole()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateShouldAddTheUserUserRole))
              .Options;

            using (var context = new DataDbContext(options))
            {
                var validator = new UserRoleValidator();
                var userUserRolesService = new UserToRoleServices(context, validator);

                User userToAdd = new User
                {
                    Email = "user@yahoo.com",
                    LastName = "Ion",
                    FirstName = "POpescu",
                    Password = "secret",
                    DataRegistered = DateTime.Now,
                    UserToRoles = new List<UserToRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRoleRegular = new UserRole
                {
                    Name = "Regular",
                    Description = "Creat pentru testare"
                };
                UserRole addUserRoleAdmin = new UserRole
                {
                    Name = "AdminDeTest",
                    Description = "Creat pentru testare"
                };
                context.UserRoles.Add(addUserRoleRegular);
                context.UserRoles.Add(addUserRoleAdmin);
                context.SaveChanges();

                context.UserToRoles.Add(new UserToRole
                {
                    User = userToAdd,
                    UserRole = addUserRoleRegular,
                    StartTime = DateTime.Parse("2019-06-13T00:00:00"),
                    EndTime = null
                });
                context.SaveChanges();

                //sectiunea de schimbare valori invalidata de catre UserRoleValidator
                var uurpm = new UserToRolePostModel
                {
                    UserId = userToAdd.Id,
                    UserRoleName = "Admin"
                };
                var result1 = userUserRolesService.Create(uurpm);
                Assert.IsNotNull(result1);   //User role nu exista in baza de date dupa validare, ==> exista erori la validare

                //sectiunea de schimbare valori validata de catre UserRoleValidator
                var uurpm1 = new UserToRolePostModel
                {
                    UserId = userToAdd.Id,
                    UserRoleName = "AdminDeTest"
                };
                var result2 = userUserRolesService.Create(uurpm1);
                Assert.IsNull(result2);   //User role exista si se face upsert
            }
        }


        [Test]
        public void GetUserRoleNameByIdShouldReturnUserRoleName()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetUserRoleNameByIdShouldReturnUserRoleName))
              .Options;

            using (var context = new DataDbContext(options))
            {
                var userToRolesService = new UserToRoleServices(context, null);

                User userToAdd = new User
                {
                    Email = "user@yahoo.com",
                    LastName = "Ion",
                    FirstName = "POpescu",
                    Password = "secret",
                    Username = "userrrrr",
                    DataRegistered = DateTime.Now,
                    UserToRoles = new List<UserToRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRole = new UserRole
                {
                    Name = "Regular",
                    Description = "Creat pentru testare"
                };
                context.UserRoles.Add(addUserRole);
                context.SaveChanges();

                context.UserToRoles.Add(new UserToRole
                {
                    User = userToAdd,
                    UserRole = addUserRole,
                    StartTime = DateTime.Parse("2019-06-13T00:00:00"),
                    EndTime = null
                });
                context.SaveChanges();
                var userToRoleId = context.UserToRoles.Last();

                string userRoleName = userToRolesService.GetUserRoleNameById(userToRoleId.Id);
                Assert.AreEqual("Regular", userRoleName);
            }
        }
    }
}
