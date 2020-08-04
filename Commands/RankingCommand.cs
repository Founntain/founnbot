using System;
using System.Linq;
using Discord.WebSocket;
using FounnBot.Classes;
using FounnBot.Interfaces;

namespace FounnBot.Commands{
    public class RankingCommand : ICommand
    {
        private SocketGuild Guild {get; set;}

        public RankingCommand(SocketGuild guild)
        {
            this.Guild = guild;
        }

        public bool RunCommand(SocketMessage msg)
        {
            var userFromGuild = Database.GetUsersFromGuild(this.Guild).OrderByDescending(x => x.TotalXp).Take(10).ToList();

            var output = $"```\nRanking for {this.Guild.Name}\n\n";

            var place = 1;

            userFromGuild.ForEach(x => {
                output += $"{place}) {this.Guild.GetUser(x.UserId).Username} | Level {x.Level} | TotalXP {x.TotalXp}\n";
                place++;
            });

            output += "```";

            msg.Channel.SendMessageAsync(output);

            return true;
        }
    }
}