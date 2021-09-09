using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OBOToolsWebServer.Config.Objects.Chat;
using OBOToolsWebServer.Scripts.Logic.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OBOToolsWebServer.API.WorkSpace.Chat
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        [HttpPost]
        [Route("sendMessage")]
        public List<Message> SendMessage([FromBody] Message message)
        {
            return ChatLogic.getMessage(message);
        }
    }
}
