using System;
using System.Linq;
using Discord.WebSocket;
using FounnBot.Classes;
using FounnBot.Interfaces;

namespace FounnBot.Commands{
    public class SetPrefixCommand : ICommand{

        private Config Config;

        public SetPrefixCommand(Config config)
        {
            this.Config = config;
        }
        
        public bool RunCommand(SocketMessage msg){            
            var user = msg.Author;
            var guild = ((SocketGuildChannel) msg.Channel).Guild;
            var prefix = msg.Content.Split(" ")[1];

            var trustedPeople = Database.GetTrustedPersonsFromDb().ToList();

            if(guild.OwnerId == user.Id || trustedPeople.Where(x => x.GuildId == guild.Id).Select(x => x.UserId).Contains(user.Id) || user.Id == this.Config.BotOwner){
                if(prefix.Length > 4){
                    msg.Channel.SendMessageAsync("Your prefix can't be longer than 4 characters");
                    
                    return false;
                }

                var result = Database.SetPrefix(guild.Id, prefix);

                switch(result){
                    case 0:
                        msg.Channel.SendMessageAsync($"Prefix **{prefix}** was assigned to guild **{guild.Name}** and saved on database");

                        break;
                    case 1:
                        msg.Channel.SendMessageAsync($"Prefix for guild **{guild.Name}** updated on database");
                        break;
                    default:
                        msg.Channel.SendMessageAsync("**I SHOULD NOT SAY THAT. KICK FOUNNTAIN IN THE ASS WHEN YOU SEE THIS!**");
                        break;
                }

                return true;
            }
            
            msg.Channel.SendMessageAsync("You are not allowed to use this command. You either need to be the owner of the Discord, a Trusted Person or the Bot Owner");
            
            return false;
        }
    }
}