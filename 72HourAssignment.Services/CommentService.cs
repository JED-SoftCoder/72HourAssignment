using _72HourAssignment.Data;
using _72HourAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _72HourAssignment.Services
{
   public  class CommentService
    {
        private readonly Guid _userId;
        public CommentService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateComment(CommentCreate model)
        {
            var entity =
                new Comment()
                {
                    AuthorId = _userId,
                    Text = model.Text,
                    PostId = model.PostId


                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Comments.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }
        
        public IEnumerable<CommentListItem> GetComments()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Comments
                    .Where(e => e.AuthorId == _userId)
                    .Select(
                        e =>
                        new CommentListItem
                        {
                            CommentId = e.CommentId,
                            Text = e.Text,
                            PostId = e.PostId
                        }

                        );
                return query.ToArray();
                    
            }
        }

        public CommentDetail GetCommentById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Comments
                    .Single(e => e.PostId == id && e.AuthorId == _userId);
                return
                    new CommentDetail
                    {
                        CommentId = entity.CommentId,
                        Text = entity.Text,
                        Replies = entity.Replies,
                        PostId = entity.PostId
                    };
            } 
        }
    }
}
