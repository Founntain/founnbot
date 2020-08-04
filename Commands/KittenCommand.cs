using System;
using System.IO;
using System.Net;
using Discord.WebSocket;
using FounnBot.Interfaces;

namespace FounnBot.Commands{
    public class KittenCommand : ICommand{
        public bool RunCommand(SocketMessage msg){            
            using(var wc = new WebClient()){
                var image = wc.DownloadData("http://www.randomkittengenerator.com/cats/rotator.php");

                msg.Channel.SendFileAsync((Stream) new MemoryStream(image), "kitten.png");
            }
            
            return true;
        }
    }
}