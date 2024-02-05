using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ishop.Infrastructure.ServiceImplementations
{
    public class MessagingDbContext
    {
        private readonly string _connectionString;

        public MessagingDbContext(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("MySqlConnection");

        public IDbConnection Connection => new MySqlConnection(_connectionString);
    }
}
