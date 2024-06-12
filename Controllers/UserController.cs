using System.Security.Claims;
using InstagramDemo.DTOs;
using InstagramDemo.Models;
using InstagramDemo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace InstagramDemo.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("api/user")]

public class UserController : ControllerBase
{
    private readonly IUserRepository _user;
    
    public UserController(IUserRepository user)
    {
        _user = user;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetUsers()
    {
        var users = await _user.GetUsers();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize]

    public async Task<ActionResult> GetUserById([FromRoute] long id)
    {
        var user = await _user.GetUserById(id);
        if(user == null)
            return NotFound("User Not Found");
        return Ok(user);
    }

    // [HttpPost]
    // [Authorize]

    // public async Task<ActionResult> Createuser([FromBody] UserCreateDto user)
    // {
    //     var toCreate = new User
    //     {
    //         Username = user.Username,
    //         FullName = user.FullName,
    //         Email = user.Email,
    //         Phone = user.Phone,
    //         Password = user.Password
    //     };

    //     var CreatedUser = await _user.CreateUser(toCreate);

    //     if(CreatedUser == null)
    //         return BadRequest("User creation failed");
        
    //     return Ok(CreatedUser);
    // }
    [HttpGet("current_user")]
    [Authorize]

    public async Task<ActionResult> GetCurrentUser()
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var currentUser = await _user.GetUserById(userId);
        return Ok(currentUser);


    }

    [HttpPut("{user_id}")]
    [Authorize]

    public async Task<ActionResult> UpdateUser([FromBody] UserUpdateDto user, [FromRoute] long user_id)
    {
        var userId = UserUtils.GetUserId(HttpContext);

        var existingUser = await _user.GetUserById(user_id);
        if(existingUser == null)
            return NotFound("User not found");

        if (userId != existingUser.Id)
            return Unauthorized();
        
        var toUpdate = existingUser with
        {
            Username = user.Username ?? existingUser.Username,
            FullName = user.FullName ?? existingUser.FullName,
            Email = user.Email ?? existingUser.Email,
            Phone = user.Phone ?? existingUser.Phone,
            Password = user.Password ?? existingUser.Password
        };

        var updatedUser = await _user.UpdateUser(toUpdate);

        return NoContent();
    }

    [HttpDelete("{user_id}")]
    [Authorize]

    public async Task<ActionResult> DeleteUser([FromRoute] long user_id)
    {
        var userId = UserUtils.GetUserId(HttpContext);

        var user = await _user.GetUserById(user_id);
        if(user == null)
            return NotFound("User not found");

        if (userId != user.Id)
            return Unauthorized();

        var deleteUser = await _user.DeleteUser(user.Id);

        if(!deleteUser)
            return BadRequest("User not deleted");

        return NoContent();


    }


}
