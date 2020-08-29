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

            foreach(var userEntity in userFromGuild){
                var user = this.Guild.GetUser(userEntity.UserId);

                if(user == null) continue;

                output += $"{place}) {user.Username} | Level {userEntity.Level} | TotalXP {userEntity.TotalXp}\n";
                place++;
            }

            userFromGuild.ForEach(x => {
                
            });

            output += "```";

            msg.Channel.SendMessageAsync(output);

            return true;
        }
    }
}