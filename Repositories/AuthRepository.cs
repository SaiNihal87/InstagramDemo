
using Dapper;

namespace InstagramDemo.Repositories;

public interface IAuthRepository
{
    Task<bool> CheckAuthentication(string Username, string Password);
}

public class AuthRepository : BaseRepository, IAuthRepository
{
    public AuthRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<bool> CheckAuthentication(string Username, string Password)
    {
        var query = "SELECT * FROM users WHERE username = @Username AND password = @Password";

        using var connection = DbConnection;
        return await connection.QueryFirstOrDefaultAsync(query, new{ Username, Password}) != null;

    }
}