using Ishop.Domain.Entities;
using Ishop.Gui.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ishop.Infrastructure.ServiceImplementations
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class MessagingController : ControllerBase
    {
        private readonly IHubContext<MessagingHub> _hubContext;
        private readonly MessageRepository _messageRepository;

        public MessagingController(IHubContext<MessagingHub> hubContext, MessageRepository messageRepository)
        {
            _hubContext = hubContext;
            _messageRepository = messageRepository;
        }

        [HttpGet("{sender}/{receiver}")]
        public async Task<IEnumerable<Message>> GetMessages(string sender, string receiver)
        {
            return await _messageRepository.GetMessagesAsync(sender, receiver);
        }

        [HttpPost("Send Message")]
        public async Task<IActionResult> SendMessage([FromBody] Message request)
        {
            // Validate the request here

            var message = new Message
            {
                Sender = request.Sender,
                Receiver = request.Receiver,
                Content = request.Content,
                Timestamp = DateTime.Now
            };

            await _messageRepository.AddMessageAsync(message);

            // Notify clients using SignalR
            await _hubContext.Clients.Users(request.Sender, request.Receiver).SendAsync("ReceiveMessage", message);

            return Ok();
        }
    }

    

}
