using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InstagramDemo.DTOs;

public class UserDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; } 

    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = null!;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [JsonPropertyName("phone")]
    public long Phone { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
}

public class UserCreateDto
{
    [JsonPropertyName("username")]
    [Required]
    public string Username { get; set; } = null!;

    [JsonPropertyName("full_name")]
    [Required]
    public string FullName { get; set; } = null!;
    
    [JsonPropertyName("email")]
    [Required]
    public string Email { get; set; } = null!;

    [JsonPropertyName("phone")]
    public long? Phone { get; set; }

    [JsonPropertyName("password")]
    [Required]
    public string Password { get; set; } = null!;
}

public class UserUpdateDto
{
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("full_name")]
    public string? FullName { get; set; }
    
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("phone")]
    public long? Phone { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }
}

public class UserLoginDto
{
    [JsonPropertyName("username")]
    [Required]
    public string Username { get; set; } = null!;

    [JsonPropertyName("password")]
    [Required]
    public string Password { get; set; } = null!;
}

