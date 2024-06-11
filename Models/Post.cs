using System.Text.Json.Serialization;

namespace InstagramDemo.Models;

public record Post
{
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;

    [JsonPropertyName("likes")]
    public string? Likes { get; set; }

    [JsonPropertyName("comments")]
    public string? Comments { get; set; }

    // [JsonPropertyName("comment_id")]
    // public long CommentId { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
}