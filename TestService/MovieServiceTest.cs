using Lab1.Models;
using Lab1.Services;
using Lab1.ViewModels;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestService
{
    class MovieServiceTest
    {
        [Test]
        public void GetAllShouldReturnCorrectNumberOfPagesForFilme()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPagesForFilme))
              .Options;

            using (var context = new DataDbContext(options))
            {
                var filmService = new MovieService(context);
                var addedFilm = filmService.Create(new MoviePostModel
                {
                    Title = "film de test 1",
                    Director = "dir1",
                    DateAdded = DateTime.Parse("2019-06-11T00:00:00"),
                    DurationInMinutes = 100,
                    Description = "asdvadfbdbsb",
                    MovieGenre = "Comedy",
                    ReleseYear = 2000,
                    Rating = 3,
                    WasWatched = "NO",
                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "asd"
                        }
                    },

                }, null);

                DateTime from = DateTime.Parse("2019-06-10T00:00:00");
                DateTime to = DateTime.Parse("2019-06-12T00:00:00");

                var allFilms = filmService.GetAllMovies(1, from, to);
                Assert.AreEqual(1, allFilms.Entries.Count);
            }
        }


        [Test]
        public void GetByIdShouldReturnFilmWithCorrectIdNumber()
        {
            var options1 = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnFilmWithCorrectIdNumber))
              .Options;

            using (var context = new DataDbContext(options1))
            {
                var filmService = new MovieService(context);
                var addedFilm = filmService.Create(new MoviePostModel
                {
                    Title = "Testare",
                    Director = "dir1",
                    DateAdded = new DateTime(),
                    DurationInMinutes = 100,
                    Description = "asdvadfbdbsb",
                    MovieGenre = "Comedy",
                    ReleseYear = 2000,
                    Rating = 3,
                    WasWatched = "NO"
                }, null);

                var theFilm = filmService.GetById(addedFilm.Id);
                Assert.AreEqual("Testare", theFilm.Title);
            }
        }


        [Test]
        public void CreateShouldAddAndReturnTheFilmCreated()
        {
            var options2 = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateShouldAddAndReturnTheFilmCreated))
              .Options;

            using (var context = new DataDbContext(options2))
            {
                var filmService = new MovieService(context);
                var addedFilm = filmService.Create(new MoviePostModel
                {
                    Title = "Create",
                    Director = "dir1",
                    DateAdded = new DateTime(),
                    DurationInMinutes = 100,
                    Description = "asdvadfbdbsb",
                    MovieGenre = "Comedy",
                    ReleseYear = 2000,
                    Rating = 3,
                    WasWatched = "NO"
                }, null);

                Assert.IsNotNull(addedFilm);
                Assert.AreEqual("Create", addedFilm.Title);
            }
        }


        [Test]
        public void UpsertShouldChangeTheFildValues()
        {
            var options3 = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(UpsertShouldChangeTheFildValues))
              .EnableSensitiveDataLogging()
              .Options;

            using (var context = new DataDbContext(options3))
            {
                var filmService = new MovieService(context);
                var original = filmService.Create(new MoviePostModel
                {
                    Title = "Original",
                    Director = "dir1",
                    DateAdded = new DateTime(),
                    DurationInMinutes = 100,
                    Description = "asdvadfbdbsb",
                    MovieGenre = "Comedy",
                    ReleseYear = 2000,
                    Rating = 3,
                    WasWatched = "NO"
                }, null);


                var film = new MoviePostModel
                {
                    Title = "upsert"
                };

                context.Entry(original).State = EntityState.Detached;

                var result = filmService.Upsert(1, film);

                Assert.IsNotNull(original);
                Assert.AreEqual("upsert", result.Title);
            }
        }

        [Test]
        public void DeleteShouldRemoveAndReturnFilm()
        {
            var options4 = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(DeleteShouldRemoveAndReturnFilm))
              .EnableSensitiveDataLogging()
              .Options;

            using (var context = new DataDbContext(options4))
            {
                var filmService = new MovieService(context);
                var toAdd = filmService.Create(new MoviePostModel
                {
                    Title = "DeSters",
                    Director = "dir1",
                    DateAdded = new DateTime(),
                    DurationInMinutes = 100,
                    Description = "asdvadfbdbsb",
                    MovieGenre = "Comedy",
                    ReleseYear = 2000,
                    Rating = 3,
                    WasWatched = "NO"
                }, null);

                Assert.IsNotNull(toAdd);
                Assert.AreEqual(1, filmService.GetAllMovies(1, null, null).Entries.Count);

                var deletedFilm = filmService.Delete(1);

                Assert.IsNotNull(deletedFilm);
                Assert.AreEqual(0, filmService.GetAllMovies(1, null, null).Entries.Count);
            }
        }
    }
}
