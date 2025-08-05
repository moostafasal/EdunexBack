using Microsoft.Extensions.Options;

namespace EduNexAPI.Helpers
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString();
    }

    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly DatabaseSettings _settings;
        private readonly IConfiguration _config;

        public ConnectionStringProvider(IOptions<DatabaseSettings> options, IConfiguration config)
        {
            _settings = options.Value;
            _config = config;
        }

        public string GetConnectionString()
        {
            var host = _settings.Host ?? _config["DB_HOST"];
            var name = _settings.Name ?? _config["DB_NAME"];
            var user = _settings.User ?? _config["DB_USER"];
            var password = _settings.Password ?? _config["DB_PASSWORD"];

            return $"Server={host};Database={name};User Id={user};Password={password};" +
                   "Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
        }
    }

}
