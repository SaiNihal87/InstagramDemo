using System.Security.Claims;
using InstagramDemo.DTOs;
using InstagramDemo.Models;
using InstagramDemo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstagramDemo.Controllers;

[ApiController]
[Route("api/comment")]

public class CommentController : ControllerBase
{
    private readonly ICommentRepository _comment;
    public CommentController(ICommentRepository comment)
    {
        _comment = comment;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetComments()
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var comments = await _comment.GetCommentsByUserId(userId);
        return Ok(comments);
    }   

    [HttpGet("{id}")]

    public async Task<ActionResult> GetCommentById([FromRoute] long id)
    {
        var comment = await _comment.GetCommentById(id);
        if(comment == null)
            return NotFound("comment Not Found");
        return Ok(comment);
    }

    [HttpPost]
    [Authorize]

    public async Task<ActionResult> CreateComment([FromBody] CommentCreateDto comment)
    {
        var userId = UserUtils.GetUserId(HttpContext);
        
        var toCreate = new Comment
        {
            UserId = userId,
            PostId = comment.PostId,
            Description = comment.Description
        };

        var CreatedComment = await _comment.CreateComment(toCreate);

        if(CreatedComment == null)
            return BadRequest("Comment creation failed");
        
        return Ok(CreatedComment);
    }

    [HttpPut("{CommentId}")]
    [Authorize]

    public async Task<ActionResult> UpdateComment([FromBody] CommentUpdateDto comment, [FromRoute] long CommentId)
    {
        var userId = UserUtils.GetUserId(HttpContext);
        
        var existingComment = await _comment.GetCommentById(CommentId);
        if(existingComment == null)
            return NotFound("Comment not found");

        if (userId != existingComment.UserId)
            return Unauthorized();
        
        var toUpdate = existingComment with
        {
            Description = comment.Description ?? existingComment.Description
        };

        var updatedComment = await _comment.UpdateComment(toUpdate);

        return NoContent();
    }

    [HttpDelete("{CommentId}")]
    [Authorize]

    public async Task<ActionResult> DeleteComment([FromRoute] long CommentId)
    {

        var userId = UserUtils.GetUserId(HttpContext);

        var comment = await _comment.GetCommentById(CommentId);
        if(comment == null)
            return NotFound("Comment not found");

        if (userId != comment.UserId)
            return Unauthorized();

        var deleteComment = await _comment.DeleteComment(comment.Id);

        if(!deleteComment)
            return BadRequest("Comment not deleted");

        return NoContent();
    }
}