using Lab1.Models;
using Lab1.Services;
using Lab1.ViewModels;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestService
{
    public class UserRoleServiceTest
    {
        private UserRole roleAdmin;
        [SetUp]
        public void Setup()
        {
            roleAdmin = new UserRole
            {
                Name = "Aadmin",
                Description = "Admin"
            };
        }

        [Test]
        public void GetAllShouldReturnUserRoles()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnUserRoles))
              .Options;

            using (var context = new DataDbContext(options))
            {
                var userRoleService = new UserRoleService(context);
                var addUserRole = userRoleService.Create(new UserRolePostModel
                {
                    Name = "Rol testare",
                    Description = "Creat pentru testare"
                });

                var allUsers = userRoleService.GetAll();
                Assert.IsNotNull(allUsers);
            }
        }


        [Test]
        public void GetByIdShouldReturnUserRole()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnUserRole))
              .Options;

            using (var context = new DataDbContext(options))
            {
                var userRoleService = new UserRoleService(context);
                var addUserRole = userRoleService.Create(new UserRolePostModel
                {
                    Name = "Rol testare",
                    Description = "Creat pentru testare"
                });

                var userRole = userRoleService.GetById(addUserRole.Id);
                Assert.AreEqual("Rol testare", userRole.Name);
            }
        }


        [Test]
        public void CreateShouldAddAndReturnTheRole()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateShouldAddAndReturnTheRole))
              .Options;

            using (var context = new DataDbContext(options))
            {
                var userRoleService = new UserRoleService(context);
                var addUserRole = userRoleService.Create(new UserRolePostModel
                {
                    Name = "Rol testare",
                    Description = "Creat pentru testare"
                });

                var userRole = context.UserRoles.Find(addUserRole.Id);
                Assert.AreEqual(addUserRole.Name, userRole.Name);
            }
        }


        //[Test]
        //public void UpsertShouldAddNew()
        //{
        //    var options = new DbContextOptionsBuilder<DataDbContext>()
        //      .UseInMemoryDatabase(databaseName: nameof(UpsertShouldAddNew))
        //      .EnableSensitiveDataLogging()
        //      .Options;

        //    using (var context = new DataDbContext(options))
        //    {
        //        var userRoleService = new UserRoleService(context);
        //        var addUserRole = userRoleService.Upsert(1, new UserRole
        //        {
        //            Name = "Rol testare",
        //            Description = "Creat pentru testare"
        //        });

        //        var userRole = context.UserRoles.Find(addUserRole.Id);
        //        Assert.AreEqual(addUserRole.Name, userRole.Name);
        //    }

        //}

        [Test]
        public void UpsertShouldChangeTheFildValuesForRole()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateShouldAddAndReturnTheRole))
              .Options;

            using (var context = new DataDbContext(options))
            {


                var userRoleService = new UserRoleService(context);
                userRoleService.Upsert(1 ,roleAdmin);

                var addedRole = context.UserRoles.Last();

                int id = addedRole.Id;

                //context.SaveChanges();


                //var upsertUserRole = userRoleService.Upsert(addUserRole.Id, new UserRole
                //{
                //    Name = "Admin",
                //    Description = "Modificat pentru testare"
                //});

                UserRole newRole = new UserRole
                {
                    Name = "AdminModificat",
                    Description = "Admiiin"
                };

                context.Entry(addedRole).State = EntityState.Detached;

                var updateRole = userRoleService.Upsert(id, newRole);


                var lastRole = context.UserRoles.Last();

                Assert.AreEqual(lastRole.Name, newRole.Name);
            }
            

        }

        [Test]
        public void DeleteShouldRemoveAndReturnUserRole()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(DeleteShouldRemoveAndReturnUserRole))
              .EnableSensitiveDataLogging()
              .Options;

            using (var context = new DataDbContext(options))
            {
                var userRoleService = new UserRoleService(context);
                var addUserRole = userRoleService.Create(new UserRolePostModel
                {
                    Name = "Rol testare",
                    Description = "Creat pentru testare"
                });

                Assert.IsNotNull(addUserRole);
                Assert.AreEqual("Rol testare", context.UserRoles.Find(1).Name);

                var deletedUserRole = userRoleService.Delete(1);

                Assert.IsNotNull(deletedUserRole);
                Assert.AreEqual(addUserRole.Name, deletedUserRole.Name);
            }
        }

        [Test]
        public void DeleteNothing()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(DeleteNothing))
              .EnableSensitiveDataLogging()
              .Options;

            using (var context = new DataDbContext(options))
            {
                var userRoleService = new UserRoleService(context);

                var deletedUserRole = userRoleService.Delete(1);
                Assert.AreEqual(0, context.UserRoles.Count());
            }
        }
    }
}
