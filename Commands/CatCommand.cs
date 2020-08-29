using System.IO;
using System.Net;
using Discord;
using Discord.WebSocket;
using FounnBot.Classes;
using FounnBot.Interfaces;
using Newtonsoft.Json;

namespace FounnBot.Commands{
    public class CatCommand : ICommand
    {
        public bool RunCommand(SocketMessage msg)
        {
            using(var wc = new WebClient()){
                wc.Headers.Add("X-API-KEY", "5f455455-16f8-461a-8c80-c569d0d7eb04");
                wc.QueryString.Add("mime_types", "jpg,png");
                wc.QueryString.Add("has_breeds", "true");
                wc.QueryString.Add("size", "small");
                wc.QueryString.Add("sub_id", msg.Author.Username);
                wc.QueryString.Add("limit", "1");
            
                var cat = JsonConvert.DeserializeObject<Animal[]>(wc.DownloadString("https://api.thecatapi.com/v1/images/search?"))[0];

                var data = wc.DownloadData(cat.Url);

                var x = msg.Channel.SendFileAsync((Stream) new MemoryStream(data), "cat.png", $"Powered by **TheCatAPI** *({cat.Id})*").Result;

                x.AddReactionAsync(new Emoji("❤"));
                x.AddReactionAsync(new Emoji("💔"));
            }

            return true;
        }
    }
}