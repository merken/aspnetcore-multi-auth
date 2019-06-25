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

    public class MyDb1AuthenticationService : MyDbAuthenticationService
    {
        public MyDb1AuthenticationService(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string GetConnectionString() => configuration.GetConnectionString("MyDb1");
    }

    public class MyDb2AuthenticationService : MyDbAuthenticationService
    {
        public MyDb2AuthenticationService(IConfiguration configuration) : base(configuration)
        {
        }

        protected override string GetConnectionString() => configuration.GetConnectionString("MyDb2");
    }

    public abstract class MyDbAuthenticationService : ICustomAuthenticationService
    {
        protected readonly IConfiguration configuration;

        public MyDbAuthenticationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected abstract string GetConnectionString();

        public async virtual Task<bool> AuthenticateUserAsync(string username, string password)
        {
            var connectionString = GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                var parameter = command.CreateParameter();
                parameter.ParameterName = "username";
                parameter.Value = username;
                command.Parameters.Add(parameter);

                command.CommandType = CommandType.Text;
                command.CommandText = $"SELECT TOP 1 Username, Password FROM Authentication WHERE Username = @username";

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