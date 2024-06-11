using System.Text.Json.Serialization;

namespace InstagramDemo.DTOs;

public class PostDto
{
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("post_title")]
    public string PostTitle { get; set; } = null!;

    [JsonPropertyName("post_content")]
    public string PostComment { get; set; } = null!;

    [JsonPropertyName("likes")]
    public string? Likes { get; set; }

    [JsonPropertyName("Comment_info")]
    public string? CommentInfo { get; set; }

    [JsonPropertyName("comment_id")]
    public long CommentId { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
}

public class PostCreateDto
{
    // [JsonPropertyName("user_id")]
    // public long UserId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;

    [JsonPropertyName("likes")]
    public string? Likes { get; set; }

    // [JsonPropertyName("created_at")]
    // public DateTimeOffset CreatedAt { get; set; }
}

public class PostUpdateDto
{

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;

    [JsonPropertyName("likes")]
    public string? Likes { get; set; }

    // [JsonPropertyName("updated_at")]
    // public DateTimeOffset? UpdatedAt { get; set; }
}