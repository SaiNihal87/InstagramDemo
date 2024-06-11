using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InstagramDemo.DTOs;

public class CommentDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("post_id")]
    public long PostId { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
}

public class CommentCreateDto
{
    // [JsonPropertyName("user_id")]
    // [Required]
    // public long UserId { get; set; }

    [JsonPropertyName("post_id")]
    [Required]
    public long PostId { get; set; }

    [JsonPropertyName("description")]
    [Required]
    public string Description { get; set; } = null!;
}

public class CommentUpdateDto
{
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;
}