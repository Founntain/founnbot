using System.IO;
using System.Net;
using Discord;
using Discord.WebSocket;
using FounnBot.Interfaces;

namespace FounnBot.Commands
{
    public class FoxCommand : ICommand
    {
        public bool RunCommand(SocketMessage msg)
        {
            using(var wc = new WebClient()){
                var fox = wc.DownloadData("https://foxrudor.de/");

                var sendedMessage = msg.Channel.SendFileAsync((Stream) new MemoryStream(fox), "fox.png", $"Powered by **foxrudor.de**").Result;

                sendedMessage.AddReactionAsync(new Emoji("❤"));
                sendedMessage.AddReactionAsync(new Emoji("💔"));
            }

            return true;
        }
    }
}