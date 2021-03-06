﻿using System;
using System.Collections.Generic;
using CS321_W5D2_BlogAPI.Core.Models;

namespace CS321_W5D2_BlogAPI.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly IUserService _userService;

        public PostService(IPostRepository postRepository, IBlogRepository blogRepository, IUserService userService)
        {
            _postRepository = postRepository;
            _blogRepository = blogRepository;
            _userService = userService;
        }

        public Post Add(Post newPost)
        {
            //  Prevent users from adding to a blog that isn't theirs
          
            //     Use the _userService to get the current users id.
            var currentUserId = _userService.CurrentUserId;

            //     retrieve the blog in order to check user id
            var blog = _blogRepository.Get(newPost.BlogId);

            if (currentUserId == blog.UserId)
            {
                // assign the current date to DatePublished
                newPost.DatePublished = DateTime.Now;

                return _postRepository.Add(newPost);
            }
            throw new ApplicationException("You cannot add to a blog that isn't yours.");
          

        }

        public Post Get(int id)
        {
            return _postRepository.Get(id);
        }

        public IEnumerable<Post> GetAll()
        {
            return _postRepository.GetAll();
        }
        
        public IEnumerable<Post> GetBlogPosts(int blogId)
        {
            return _postRepository.GetBlogPosts(blogId);
        }

        public void Remove(int id)
        {
            var post = this.Get(id);
            //  prevent user from deleting from a blog that isn't theirs

            //     Use the _userService to get the current users id.
            var currentUserId = _userService.CurrentUserId;

            //     retrieve the blog in order to check user id
            var blog = _blogRepository.Get(post.BlogId);

            if (currentUserId == blog.UserId)
            {
                _postRepository.Remove(id);
            }

            throw new ApplicationException("You cannot delete a blog that isn't yours.");

        }

        public Post Update(Post updatedPost)
        {
            // prevent user from updating a blog that isn't theirs

            //      Use the _userService to get the current users id
            var currentUserId = _userService.CurrentUserId;

            //      retrieve the blog in order to check user id
            var blog = _blogRepository.Get(updatedPost.BlogId);

            if (currentUserId == blog.UserId)
            {
                return _postRepository.Update(updatedPost);
            }

            throw new ApplicationException("You cannot update a blog that isn't yours.");
        }

    }
}
