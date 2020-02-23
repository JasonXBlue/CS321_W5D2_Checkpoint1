using System;
using System.Collections.Generic;
using System.Linq;
using CS321_W5D2_BlogAPI.Core.Models;
using CS321_W5D2_BlogAPI.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CS321_W5D2_BlogAPI.Infrastructure.Data
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _dbContext;

        public BlogRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Blog> GetAll()
        {
            // Retrieve all blgs. Include Blog.User.
            return _dbContext.Blogs
                .Include(a => a.User)
                .ToList();
        }

        public Blog Get(int id)
        {
            // Retrieve the blog by id. Include Blog.User.
            return _dbContext.Blogs
                .Include(a => a.User)
                .SingleOrDefault(b => b.Id == id);
        }

        public Blog Add(Blog blog)
        {
            // Add new blog
            _dbContext.Blogs.Add(blog);
            _dbContext.SaveChanges();
            return blog;
        }

        public Blog Update(Blog updatedItem)
        {
            // update blog

            // get the blog object in the current list 
            var currentItem = _dbContext.Blogs.Find(updatedItem);

            // return null if blog to update isn't found
            if (currentItem == null) return null;

            // copy the property values from the changed blog into the
            // one in the db. NOTE that this is much simpler than individually
            // copying each property.
            _dbContext.Entry(currentItem)
               .CurrentValues
               .SetValues(updatedItem);

            // update the blog and save
            _dbContext.Blogs.Update(currentItem);
            _dbContext.SaveChanges();
            return currentItem;
        }

        public void Remove(int id)
        {
            // remove blog
            var blog = _dbContext.Blogs.FirstOrDefault(a => a.Id == id);

            _dbContext.Blogs.Remove(blog);
            _dbContext.SaveChanges();
        }
    }
}
