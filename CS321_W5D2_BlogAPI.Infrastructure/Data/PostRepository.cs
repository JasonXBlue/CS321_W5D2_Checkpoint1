using System;
using System.Collections.Generic;
using System.Linq;
using CS321_W5D2_BlogAPI.Core.Models;
using CS321_W5D2_BlogAPI.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CS321_W5D2_BlogAPI.Infrastructure.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _dbContext;

        public PostRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public Post Get(int id)
        {
            // Implement Get(id). Include related Blog and Blog.User
            return _dbContext.Posts
                .Include(a => a.Blog)
                .Include(a => a.Blog.User)
                .SingleOrDefault(b => b.Id == id);
        }

        public IEnumerable<Post> GetBlogPosts(int blogId)
        {
            // Implement GetBlogPosts, return all posts for given blog id
            // Include related Blog and AppUser
            return _dbContext.Posts
                .Include(a => a.Blog)
                .Include(a => a.Blog.User)
                .Where(b => b.BlogId == blogId);    
            
        }

        public Post Add(Post post)
        {
            // add Post
            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();
            return post;
        }

        public Post Update(Post updatedPost)
        {
            // update Post

            // get the post object in the current list 
            var currentPost = _dbContext.Posts.Find(updatedPost);

            // return null if blog to update isn't found
            if (currentPost == null) return null;

            // copy the property values from the changed blog into the
            // one in the db. NOTE that this is much simpler than individually
            // copying each property.
            _dbContext.Entry(currentPost)
                .CurrentValues
                .SetValues(updatedPost);

            // update the blog and save
            _dbContext.Posts.Update(currentPost);
            _dbContext.SaveChanges();
            return currentPost;


        }

        public IEnumerable<Post> GetAll()
        {
            // get all posts
            return _dbContext.Posts
                .Include(a => a.Blog);
                //.ToList();

        }

        public void Remove(int id)
        {
            // remove Post
            var currentPost = _dbContext.Posts.FirstOrDefault(a => a.Id == id);

            if (currentPost != null)
            {
                _dbContext.Posts.Remove(currentPost);
                _dbContext.SaveChanges();
            }
        }

    }
}
