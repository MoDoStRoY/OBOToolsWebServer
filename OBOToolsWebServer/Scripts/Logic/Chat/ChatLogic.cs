using OBOToolsWebServer.Config.Objects.Chat;
using OBOToolsWebServer.Config.Objects.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OBOToolsWebServer.Scripts.Logic.Chat
{
    public class ChatLogic
    {
        static Server server = Program.server;

        public static List<Message> getMessage(Message message)
        {
            server.listOfMessages.Add(message);

            return server.listOfMessages;
        }
    }
}
