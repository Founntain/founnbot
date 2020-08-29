using System.Linq;
using Discord.WebSocket;
using FounnBot.Classes;
using FounnBot.Entities;
using FounnBot.Interfaces;

namespace FounnBot.Commands{
    public class HelpCommand : ICommand
    {
        private Config Config {get; set;}

        public HelpCommand(Config config){
            this.Config = config;
        }

        public bool RunCommand(SocketMessage msg)
        {
            var guild = ((SocketGuildChannel)msg.Channel).Guild;
            var guildFromDb = Database.GetGuildsFromDatabase().FirstOrDefault(x => x.Guild == guild.Id);

            var prefix = guildFromDb?.GuildPrefix ?? this.Config.DefaultPrefix;

            var output = "```";

            output += $"{guild.Name} prefix => {prefix}\n\n";
            output += $"{prefix}cat / {prefix}dog / {prefix}fox => Gives you a random cat / dog / fox image\n";
            output += $"{prefix}ranking => Shows XP ranking for this server\n";
            output += $"{prefix}setprefix <prefix (max 4 chars long)> => Sets the prefix of this server (Bot Owner, Server Owner and Trusted People Only)\n";
            output += $"{prefix}trust <mentionedUser> => Trust or untrust people form this Server (Server and Bot Owner only)\n";
            output += $"{prefix}createrole <color> <name> => Create a role with the given name and color and assign it to yourself. Color must be in HTML format e.g. FF22CC (without #)\n";
            
            output += $"```";

            msg.Channel.SendMessageAsync(output);

            return true;
        }
    }
}