using Dapper;
using Ishop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ishop.Infrastructure.ServiceImplementations
{
    public class MessageRepository
    {
        private readonly MessagingDbContext _dbContext;

        public MessageRepository(MessagingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(string sender, string receiver)
        {
            using (var connection = _dbContext.Connection)
            {
                connection.Open();
                return await connection.QueryAsync<Message>(
                    "SELECT * FROM Message WHERE (Sender = @Sender AND Receiver = @Receiver) OR (Sender = @Receiver AND Receiver = @Sender)",
                    new { Sender = sender, Receiver = receiver });
            }
        }

        public async Task AddMessageAsync(Message message)
        {
            using (var connection = _dbContext.Connection)
            {
                connection.Open();
                await connection.ExecuteAsync(
                    "INSERT INTO Message (sender, receiver, content, timestamp) VALUES (@Sender, @Receiver, @Content, @Timestamp)",
                    message);
            }
        }




    }
}
