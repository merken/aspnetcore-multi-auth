using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace auth.api.Services
{
    public interface ICustomAuthenticationService
    {
        Task<bool> AuthenticateUserAsync(string username, string password);
    }

    public class MyDbAuthenticationService : ICustomAuthenticationService
    {
        private readonly IConfiguration configuration;

        public MyDbAuthenticationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            var connectionString = configuration.GetConnectionString("MyDb");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = $"SELECT TOP 1 Username, Password FROM Authentication WHERE Username = '{username}'";

                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var userFromDb = reader.GetString(0);
                    var passwordFromDb = reader.GetString(1);

                    return username == userFromDb && password == passwordFromDb;
                }

                //User was not found, TOP 1 did not return a result
                return false;
            }
        }
    }
}