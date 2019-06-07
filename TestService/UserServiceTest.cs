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
                Secret = "LALALALA"
            });
        }

        [Test]
        public void ValidRegisterShouldCreateANewUser()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidRegisterShouldCreateANewUser))// "ValidRegisterShouldCreateANewUser")
              .Options;

            using (var context = new DataDbContext(options))
            {
                UsersService usersService = new UsersService(context, config);
                var added = new RegisterPostModel
                {
                    FirstName = "Alexandru",
                    LastName = "Dan",
                    Username = "NeK",
                    Email = "nekwow7@gmail.com",
                    Password = "unudoitrei"
                };
                var result = usersService.Register(added);

                Assert.IsNotNull(result);
                Assert.AreEqual(added.Username, result.Username);
            }
        }

        [Test]
        public void InvalidRegisterShouldNotCreateANewUser()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(InvalidRegisterShouldNotCreateANewUser))
              .Options;

            using (var context = new DataDbContext(options))
            {
                var usersService = new UsersService(context, config);
                var added = new RegisterPostModel
                {
                    FirstName = "Ana",
                    LastName = "Are",
                    Email = "mere",
                    Password = "123",
                    Username = "anacumerele"
                };

                var context2 = new ValidationContext(added, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(added, context2, results, true);

                Assert.IsTrue(results.Any(vr => vr.ErrorMessage == "The Email field is not a valid e-mail address."));

                added.Password = "123";
                Validator.TryValidateObject(added, context2, results, true);

                Assert.IsTrue(results.Any(vr => vr.ErrorMessage == "The field Password must be a string with a minimum length of 6 and a maximum length of 50."));

            }


        }

    }

}
