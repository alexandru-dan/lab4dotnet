using Lab1.Models;
using Lab1.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Lab1.Services
{
    public interface ICommentService
    {

        List<CommentGetModel> GetAllComments(string filter);

    }

    public class CommentService : ICommentService
    {
        private DataDbContext Context;

        public CommentService(DataDbContext Context)
        {
            this.Context = Context;
        }

        public List<CommentGetModel> GetAllComments(string text)
        {
            IQueryable<CommentGetModel> result = Context.Comments.Select(comment => new CommentGetModel() { 

                Id = comment.Id,
                Text = comment.Text,
                Important = comment.Important,
                MovieId = (from movies in Context.Movies
                           where movies.Comments.Contains(comment)
                           select movies.Id).FirstOrDefault()
            });

            if (text != null)
            {
                result = result.Where(comment => comment.Text.Contains(text));
            }

            return result.ToList();
        }
    }
}
