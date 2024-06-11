using System.Security.Claims;
using InstagramDemo.DTOs;
using InstagramDemo.Models;
using InstagramDemo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstagramDemo.Controllers;

[ApiController]
[Route("api/post")]

public class PostController : ControllerBase
{
    private readonly IPostRepository _post;
    public PostController(IPostRepository post)
    {
        _post = post;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetPosts()
    {
        var userId = UserUtils.GetUserId(HttpContext);

        var posts = await _post.GetPostsByUserId(userId);
        return Ok(posts);

    }

    [HttpGet("{id}")]
    [Authorize]

    public async Task<ActionResult> GetPostById([FromRoute] long id)
    {
        var post = await _post.GetPostById(id);
        if(post == null)
            return NotFound("post Not Found");
        return Ok(post);
    }

    [HttpPost]
    [Authorize]

    public async Task<ActionResult> CreatePost([FromBody] PostCreateDto post)
    {   
        var userId = UserUtils.GetUserId(HttpContext);

        var toCreate = new Post
        {
            UserId = userId,
            Title = post.Title,
            Content = post.Content,
            Likes = post.Likes
        };

        var CreatedPost = await _post.CreatePost(toCreate);

        if(CreatedPost == null)
            return BadRequest("post creation failed");
        
        return Ok(CreatedPost);
    }

    [HttpPut("{PostId}")]
    [Authorize]

    public async Task<ActionResult> UpdatePost([FromBody] PostUpdateDto post, [FromRoute] long PostId)
    {
        var userId = UserUtils.GetUserId(HttpContext);

        var existingPost = await _post.GetPostById(PostId);
        if(existingPost == null)
            return NotFound("Post not found");

        if (userId != existingPost.UserId)
            return Unauthorized();
        
        var toUpdate = existingPost with
        {
            Title = post.Title ?? existingPost.Title,
            Content = post.Content ?? existingPost.Content,
            Likes = post.Likes ?? existingPost.Likes
        };

        var updatedPost = await _post.UpdatePost(toUpdate);

        return NoContent();
    }

    [HttpDelete("{PostId}")]
    [Authorize]

    public async Task<ActionResult> DeletePost([FromRoute] long PostId)
    {
        var userId = UserUtils.GetUserId(HttpContext);

        var post = await _post.GetPostById(PostId);
        if(post == null)
            return NotFound("post not found");

        if (userId != post.UserId)
            return Unauthorized();

        var deletePost = await _post.DeletePost(post.Id);

        if(!deletePost)
            return BadRequest("Post not deleted");

        return NoContent();


    }
}