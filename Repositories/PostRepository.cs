using Dapper;
using InstagramDemo.Models;

namespace InstagramDemo.Repositories;

public interface IPostRepository
{
    Task<List<Post>> GetPosts();

    Task<List<Post>> GetPostsByUserId(long userId);

    Task<Post?> GetPostById(long id);

    Task<Post> CreatePost(Post post);

    Task<bool> UpdatePost(Post post);

    Task<bool> DeletePost(long id);
}

public class PostRepository : BaseRepository, IPostRepository
{
    public PostRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<Post> CreatePost(Post post)
    {
        var query = @"INSERT INTO posts (user_id, title , content, likes)
        VALUES (@UserID, @Title, @Content, @likes) RETURNING *";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<Post>(query, post);

        return result!;
    }

    public async Task<bool> DeletePost(long id)
    {
        var query = @"DELETE FROM posts WHERE id = @Id";

        using var connection = DbConnection;
        var result = await connection.ExecuteAsync(query, new {Id = id});

        return result == 1;
    }

    public async Task<List<Post>> GetPostsByUserId(long userId)
    {
        var query = @"
                    SELECT p.*, COALESCE(c.comments, '') AS comments
                    FROM posts p
                    LEFT JOIN (SELECT post_id, STRING_AGG(description,', ') as comments
                    FROM comments
                    GROUP BY post_id
                    ) c 
                    ON p.id = c.post_id
                    WHERE p.user_id = @UserId
                    ";

        using var connection = DbConnection;
        var posts = await connection.QueryAsync<Post>(query, new { UserId = userId });

        return posts.AsList();
    }

    public async Task<Post?> GetPostById(long id)
    {
        var query = @"
                    SELECT p.id, p.user_id, p.title, p.content, p.likes, COALESCE(c.comments, '') AS comments
                    FROM posts p
                    LEFT JOIN (SELECT post_id, STRING_AGG(description, ', ') as comments
                    FROM comments 
                    GROUP  BY post_id
                    ) c ON p.id = c.post_id
                    WHERE p.id = @Id
                    ";

        using var connection = DbConnection;
        var res = await connection.QueryFirstOrDefaultAsync<Post>(query, new {Id = id});

        return res;
    }

    public async Task<List<Post>> GetPosts()
    {
        var query = @"
                    SELECT p.*, COALESCE(c.comments, '') AS comments
                    FROM posts p
                    LEFT JOIN (SELECT post_id, STRING_AGG(description,', ') as comments
                    FROM comments
                    GROUP BY post_id
                    ) c 
                    ON p.id = c.post_id
                    ";

        using var connection = DbConnection;
        var posts = await connection.QueryAsync<Post>(query);

        return posts.AsList();
    }

    public async Task<bool> UpdatePost(Post post)
    {
        var query = @"UPDATE posts p SET user_id = @UserId, title = @Title, content = @Content, 
        likes = @Likes, updated_at = NOW() WHERE p.id = @Id";

        using var connection = DbConnection;
        var res = await connection.ExecuteAsync(query, post);

        return res == 1;
    }
}

