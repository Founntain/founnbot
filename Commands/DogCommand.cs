using System.IO;
using System.Net;
using Discord;
using Discord.WebSocket;
using FounnBot.Classes;
using FounnBot.Interfaces;
using Newtonsoft.Json;

namespace FounnBot.Commands{
    public class DogCommand : ICommand
    {
        public bool RunCommand(SocketMessage msg)
        {
            using(var wc = new WebClient()){
                wc.Headers.Add("X-API-KEY", "1a931ea0-f147-49cb-be25-9098e327527e");
                wc.QueryString.Add("mime_types", "jpg,png");
                wc.QueryString.Add("has_breeds", "true");
                wc.QueryString.Add("size", "small");
                wc.QueryString.Add("sub_id", msg.Author.Username);
                wc.QueryString.Add("limit", "1");
            
                var dog = JsonConvert.DeserializeObject<Animal[]>(wc.DownloadString("https://api.thedogapi.com/v1/images/search?"))[0];

                var data = wc.DownloadData(dog.Url);

                var x = msg.Channel.SendFileAsync((Stream) new MemoryStream(data), "cat.png", $"Powered by **TheDogAPI** *({dog.Id})*").Result;

                x.AddReactionAsync(new Emoji("❤"));
                x.AddReactionAsync(new Emoji("💔"));
            }

            return true;
        }
    }
}