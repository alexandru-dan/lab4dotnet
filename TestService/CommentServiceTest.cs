using Lab1.Models;
using Lab1.Services;
using Lab1.ViewModels;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Tests
{
    class CommentServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Test for getting a VALID list of comments.
        /// </summary>
        [Test]
        public void GetAListOfCommentsShouldReturnAListOfComments()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAListOfCommentsShouldReturnAListOfComments))
              .Options;

            using (var context = new DataDbContext(options))
            {
                var commentsService = new CommentService(context);
                var movieService = new MovieService(context);

                var MovieWithComments = new MoviePostModel()
                {
                    Title = "Avatar",
                    Description = "SF",
                    MovieGenre = "Action",
                    DurationInMinutes = 100,
                    ReleseYear = 2004,
                    Director = "Alexandru",
                    DateAdded = new DateTime(08/06/2019),
                    Rating = 9,
                    WasWatched = "NO",
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            Text = "A test comment 1",
                            Important = false
                        },
                        new Comment()
                        {
                            Text = "A test comment 2",
                            Important = true
                        },
                        new Comment()
                        {
                            Text = "A test comment 3",
                            Important = false
                        }
                    }
                };
                movieService.Create(MovieWithComments);

                List<CommentGetModel> comments = commentsService.GetAllComments("");
                int numberOfComments = comments.Count;

                Assert.IsNotNull(comments);
                Assert.AreEqual(3, numberOfComments);

            }
        }


        /// <summary>
        /// Test for getting a list of comments filtered by a VALID value.
        /// </summary>
        [Test]
        public void GetCommentsFilteredByValueShouldReturnAListOfFilteredComments()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetCommentsFilteredByValueShouldReturnAListOfFilteredComments))
              .Options;

            using (var context = new DataDbContext(options))
            {
                var commentsService = new CommentService(context);
                var movieService = new MovieService(context);

                var MovieWithComments = new MoviePostModel()
                {
                    Title = "Nu Stiu",
                    Description = "SF",
                    MovieGenre = "Comedy",
                    DurationInMinutes = 120,
                    ReleseYear = 2018,
                    Director = "Alexandru",
                    DateAdded = new DateTime(03 / 06 / 2019),
                    Rating = 3,
                    WasWatched = "YES",
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            Text = "A test comment 1 filtered",
                            Important = false
                        },
                        new Comment()
                        {
                            Text = "A test comment 2 filtered",
                            Important = true
                        },
                        new Comment()
                        {
                            Text = "A test comment 3",
                            Important = false
                        }
                    }
                };
                movieService.Create(MovieWithComments);

                List<CommentGetModel> comments = commentsService.GetAllComments("filtered");
                int numberOfComments = comments.Count;

                Assert.IsNotNull(comments);
                Assert.AreEqual(2, numberOfComments);

            }
        }


        /// <summary>
        /// Test for getting a list of comments filtered by an INVALID value.
        /// </summary>
        [Test]
        public void GetCommentsFilteredByInvalidValueShouldReturnAListOfEmptyComments()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetCommentsFilteredByInvalidValueShouldReturnAListOfEmptyComments))
              .Options;

            using (var context = new DataDbContext(options))
            {
                var commentsService = new CommentService(context);
                var movieService = new MovieService(context);

                var MovieWithComments = new MoviePostModel()
                {
                    Title = "Nu Stiu",
                    Description = "SF",
                    MovieGenre = "Comedy",
                    DurationInMinutes = 120,
                    ReleseYear = 2018,
                    Director = "Alexandru",
                    DateAdded = new DateTime(03 / 06 / 2019),
                    Rating = 3,
                    WasWatched = "YES",
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            Text = "A test comment 1",
                            Important = false
                        },
                        new Comment()
                        {
                            Text = "A test comment 2",
                            Important = true
                        },
                        new Comment()
                        {
                            Text = "A test comment 3",
                            Important = false
                        }
                    }
                };
                movieService.Create(MovieWithComments);

                List<CommentGetModel> comments = commentsService.GetAllComments("xxxx");
                int numberOfComments = comments.Count;

                Assert.IsNotNull(comments);
                Assert.AreEqual(0, numberOfComments);
            }
        }
    }
}
