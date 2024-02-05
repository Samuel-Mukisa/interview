using Ishop.Domain.Entities;
using Ishop.Infrastructure.ServiceImplementations;
using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ishop.Gui.Controllers
{
    public class MessagingHub:Hub
    {
        private readonly MessageRepository _messageRepository;

        public MessagingHub(MessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task SendMessage(string sender, string receiver, string content)
        {
            var message = new Message
            {
                Sender = sender,
                Receiver = receiver,
                Content = content,
                Timestamp = DateTime.Now
            };

            await _messageRepository.AddMessageAsync(message);

            await Clients.Users(sender, receiver).SendAsync("ReceiveMessage", message);
        }
    }
}
