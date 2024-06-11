using Dapper;
using InstagramDemo.Models;

namespace InstagramDemo.Repositories;

public interface ICommentRepository
{
    Task<List<Comment>> GetComments();

    Task<List<Comment>> GetCommentsByUserId(long userId);

    Task<Comment?> GetCommentById(long id);

    Task<Comment> CreateComment(Comment comment);

    Task<bool> UpdateComment(Comment comment);

    Task<bool> DeleteComment(long id);
}

public class CommentRepository : BaseRepository, ICommentRepository
{
    public CommentRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<Comment> CreateComment(Comment comment)
    {
        var query = @"INSERT INTO comments(user_id, post_id, description) 
            VALUES (@USERID, @POSTID, @DESCRIPTION) RETURNING *";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<Comment>(query, comment);

        return result!;
    }

    public async Task<bool> DeleteComment(long id)
    {
        var query = @"DELETE FROM comments WHERE id = @Id";

        using var connection = DbConnection;
        var result = await connection.ExecuteAsync(query, new {Id = id});

        return result == 1;
    }

    public async Task<Comment?> GetCommentById(long id)
    {
        var query = @"
                    SELECT c.user_id, c.id, c.description, c.post_id , p.title
                    FROM comments c
                    LEFT JOIN posts p ON p.id = c.post_id
                    WHERE c.id = @Id
                    GROUP BY c.id, c.user_id, p.title;
                    ";

        using var connection = DbConnection;
        var res = await connection.QueryFirstOrDefaultAsync<Comment>(query, new {Id = id});

        return res;
    }

    public async Task<List<Comment>> GetComments()
    {
        var query = @"SELECT * FROM comments";

        using var connection = DbConnection;
        var comments = await connection.QueryAsync<Comment>(query);

        return comments.AsList();
    }

    public async Task<List<Comment>> GetCommentsByUserId(long userId)
    {
        var query = @"SELECT * FROM comments WHERE user_id = @UserId";
    
        using var connection = DbConnection;
        var comments = await connection.QueryAsync<Comment>(query, new { UserId = userId });

        return comments.AsList();
    }

    public async Task<bool> UpdateComment(Comment comment)
    {
        var query = @"UPDATE comments c SET description = @DESCRIPTION, updated_at = NOW() WHERE c.id = @Id";

        using var connection = DbConnection;
        var res = await connection.ExecuteAsync(query, comment);

        return res == 1;
    }
}