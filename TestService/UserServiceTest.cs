using Lab1.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Options;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Lab1.Models;
using Lab1.ViewModels;
using Microsoft.EntityFrameworkCore;
using Lab1.Validators;

namespace Test
{
    class UserServiceTest
    {
        private IOptions<AppSettings> config;

        [SetUp]
        public void Setup()
        {
            config = Options.Create(new AppSettings
            {
                Secret = "dsadhjcghduihdfhdifd8ih"
            });

        }

        /// <summary>
        /// TODO: AAA - Arrange, Act, Assert
        /// </summary>
        [Test]
        public void ValidRegisterShouldCreateNewUser()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
                         .UseInMemoryDatabase(databaseName: nameof(ValidRegisterShouldCreateNewUser))// "ValidRegisterShouldCreateANewUser")
                         .Options;

            using (var context = new DataDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, config, validator, null);
                var added = new RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "lastName1",
                    Username = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };

                var result = usersService.Register(added);

                Assert.IsNull(result);
                Assert.AreEqual(added.Username, context.Users.FirstOrDefault(u => u.Id == (context.Users.FirstOrDefault(ur => ur.Username == added.Username)).Id).Username);
                Assert.AreEqual(context.UserToRoles.First().Id , context.UserToRoles.FirstOrDefault(uur => uur.Id == (context.UserToRoles.FirstOrDefault(b => b.User.Username == added.Username)).Id).Id);
            }
        }

        /// <summary>
        /// TODO: AAA - Arrange, Act, Assert
        /// </summary>
        [Test]
        public void InvalidRegisterShouldReturnErrorsCollection()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
                         .UseInMemoryDatabase(databaseName: nameof(InvalidRegisterShouldReturnErrorsCollection))
                         .Options;

            using (var context = new DataDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, config, validator, null);
                var added = new RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "lastName1",
                    Username = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111"    //invalid password should invalidate register
                };

                var result = usersService.Register(added);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.ErrorMessages.Count());
            }
        }


        [Test]
        public void AuthenticateShouldLogTheRegisteredUser()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(AuthenticateShouldLogTheRegisteredUser))
              .Options;

            using (var context = new DataDbContext(options))
            {
                var validator = new RegisterValidator();
                var validatorUser = new UserRoleValidator();
                var userToRoleService = new UserToRoleServices(context, validatorUser);
                var usersService = new UsersService(context, config, validator, userToRoleService);

                UserRole addUserRoleRegular = new UserRole
                {
                    Name = "Regular",
                    Description = "Creat pentru testare"
                };
                context.UserRoles.Add(addUserRoleRegular);
                context.SaveChanges();

                var added = new RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "lastName1",
                    Username = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };
                var result = usersService.Register(added);

                var authenticated = new LoginPostModel
                {
                    Username = "test_userName1",
                    Password = "111111"
                };
                //valid authentification
                var authresult = usersService.Authenticate(added.Username, added.Password);

                Assert.IsNotNull(authresult);
                Assert.AreEqual(1, authresult.Id);
                Assert.AreEqual(authenticated.Username, authresult.Username);

                //invalid user authentification
                var authresult1 = usersService.Authenticate("unknown", "abcdefg");
                Assert.IsNull(authresult1);
            }
        }



        [Test]
        public void GetAllShouldReturnAllRegisteredUsers()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnAllRegisteredUsers))
              .Options;

            using (var context = new DataDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, config, validator, null);
                var added1 = new RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "firstName1",
                    Username = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };
                var added2 = new RegisterPostModel
                {
                    FirstName = "secondName2",
                    LastName = "secondName2",
                    Username = "test_userName2",
                    Email = "second@yahoo.com",
                    Password = "111111"
                };
                usersService.Register(added1);
                usersService.Register(added2);

                int numberOfElements = usersService.GetAll().Count();

                Assert.NotZero(numberOfElements);
                Assert.AreEqual(2, numberOfElements);

            }
        }

        [Test]
        public void GetByIdShouldReturnAnValidUser()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
         .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnAnValidUser))
         .Options;

            using (var context = new DataDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, config, validator, null);
                var added1 = new RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "firstName1",
                    Username = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };

                usersService.Register(added1);
                var userById = usersService.GetById(context.Users.FirstOrDefault(u => u.Id == (context.Users.FirstOrDefault(ur => ur.Username == added1.Username)).Id).Id);

                Assert.NotNull(userById);
                Assert.AreEqual("firstName1", userById.FirstName);

            }
        }

        [Test]
        public void GetCurentUserShouldReturnAccesToKlaims()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
        .UseInMemoryDatabase(databaseName: nameof(GetCurentUserShouldReturnAccesToKlaims))
        .Options;

            using (var context = new DataDbContext(options))
            {
                var validator = new RegisterValidator();
                var validatorUser = new UserRoleValidator();
                var userToRoleService = new UserToRoleServices(context, validatorUser);
                var usersService = new UsersService(context, config, validator, userToRoleService);

                UserRole addUserRoleRegular = new UserRole
                {
                    Name = "Regular",
                    Description = "Creat pentru testare"
                };
                context.UserRoles.Add(addUserRoleRegular);
                context.SaveChanges();

                var added = new RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "lastName1",
                    Username = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };
                var result = usersService.Register(added);

                var authenticated = new LoginPostModel
                {
                    Username = "test_userName1",
                    Password = "111111"
                };
                var authresult = usersService.Authenticate(added.Username, added.Password);

                //usersService.GetCurrentUser(httpContext);

                Assert.IsNotNull(authresult);
            }
        }


        [Test]
        public void CreateShouldReturnNullIfValidUserGetModel()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(CreateShouldReturnNullIfValidUserGetModel))
            .Options;

            using (var context = new DataDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, config, validator, null);

                UserRole addUserRoleRegular = new UserRole
                {
                    Name = "Regular",
                    Description = "Creat pentru testare"
                };
                context.UserRoles.Add(addUserRoleRegular);
                context.SaveChanges();

                var added1 = new RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "firstName1",
                    Username = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };

                var userCreated = usersService.Create(added1);

                Assert.IsNull(userCreated);
            }
        }


        [Test]
        public void DeleteShouldEmptyTheDb()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(DeleteShouldEmptyTheDb))
            .Options;

            using (var context = new DataDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, config, validator, null);
                var added1 = new RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "firstName1",
                    Username = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };

                var userCreated = usersService.Create(added1);

                Assert.IsNull(userCreated);
                Assert.AreEqual(1, usersService.GetAll().Count());

                var userDeleted = usersService.Delete(context.Users.FirstOrDefault(u => u.Id == (context.Users.FirstOrDefault(ur => ur.Username == added1.Username)).Id).Id);

                Assert.NotNull(userDeleted);
                Assert.AreEqual(0, usersService.GetAll().Count());

            }
        }


        [Test]
        public void UpsertShouldModifyFildsValues()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(UpsertShouldModifyFildsValues))
            .Options;

            using (var context = new DataDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, config, validator, null);
                var added22 = new RegisterPostModel
                {
                    FirstName = "Nume",
                    LastName = "Prenume",
                    Username = "userName",
                    Email = "user@yahoo.com",
                    Password = "333333"
                };

                usersService.Create(added22);

                var updated = new UserPostModel
                {
                    FirstName = "Alin",
                    LastName = "Popescu",
                    Username = "popAlin",
                    Email = "pop@yahoo.com",
                    Password = "333333"
                };

                var userUpdated = usersService.Upsert(1, updated);

                Assert.NotNull(userUpdated);
                Assert.AreEqual("Alin", userUpdated.FirstName);
                Assert.AreEqual("Popescu", userUpdated.LastName);

            }
        }


    }

}
