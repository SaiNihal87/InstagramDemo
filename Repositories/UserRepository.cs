using System.ComponentModel.Design;
using Dapper;
using InstagramDemo.DTOs;
using InstagramDemo.Models;

namespace InstagramDemo.Repositories;

public interface IUserRepository
{
    Task<List<UserDto>> GetUsers();

    Task<User?> GetUserById(long id);

    Task<User> GetUserByUsername(string username);

    Task<User> CreateUser(User user);

    Task<bool> UpdateUser(User user);

    Task<bool> DeleteUser(long id);
}

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<User> CreateUser(User user)
    {
        var query = @"INSERT INTO users (username, full_name , email, phone, password)
        VALUES (@Username, @FullName, @Email, @Phone, @Password) RETURNING *";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<User>(query, user);

        return result!;
    }

    public async Task<bool> DeleteUser(long id)
    {
        var query = @"DELETE FROM users WHERE id = @Id";

        using var connection = DbConnection;
        var result = await connection.ExecuteAsync(query, new {Id = id});

        return result == 1;
    }

    public async Task<User?> GetUserById(long id)
    {
        var query = @"
                    SELECT u.id, u.username, u.full_name, u.email, u.phone, COALESCE(c.comments, '') AS comments, COALESCE(p.posts, ', ') AS posts
                    FROM users u
                    LEFT JOIN (SELECT user_id, STRING_AGG(description, ', ') AS comments
                    FROM comments   
                    GROUP BY user_id
                    ) c ON u.id = c.user_id
                    LEFT JOIN (SELECT user_id, STRING_AGG(title,', ') as posts
                    FROM posts
                    GROUP BY user_id
                    ) p ON u.id = p.user_id
                    WHERE u.id = @Id;
                    ";

        using var connection = DbConnection;
        var res = await connection.QueryFirstOrDefaultAsync<User>(query, new {Id = id});

        return res;
    }

    public async Task<User> GetUserByUsername(string username)
    {
        var query = @"SELECT * FROM users WHERE username = @Username";

        using var connection = DbConnection;
        var result = await connection.QueryFirstOrDefaultAsync<User>(query, new {Username = username});

        return result!;
    }

    public async Task<List<UserDto>> GetUsers()
    {
        var query = @"
        SELECT id, username, full_name, email, phone, created_at, updated_at , COALESCE(c.comments, '') AS comments, COALESCE(p.posts, '') AS posts
            FROM users u
            LEFT JOIN (
                SELECT user_id, STRING_AGG(description, ', ') AS comments
                FROM comments
                GROUP BY user_id
            ) c ON u.id = c.user_id
            LEFT JOIN (
                SELECT user_id, STRING_AGG(title, ', ') AS posts
                FROM posts
                GROUP BY user_id
            ) p ON u.id = p.user_id;
        ";

        using var connection = DbConnection;
        var users = await connection.QueryAsync<UserDto>(query);

        return users.AsList();
    }

    public async Task<bool> UpdateUser(User user)
    {
        var query = @"UPDATE users u SET username = @Username, full_name = @FullName, email = @Email, 
        phone = @Phone, password = @Password, updated_at = NOW() WHERE u.id = @Id";

        using var connection = DbConnection;
        var res = await connection.ExecuteAsync(query, user);

        return res == 1;
    }
}