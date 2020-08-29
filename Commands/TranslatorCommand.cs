using System.Linq;
using Discord;
using Discord.WebSocket;
using FounnBot.Interfaces;

namespace FounnBot.Commands{
    public class TranslatorCommand : ICommand
    {
        public bool RunCommand(SocketMessage msg)
        {
            var user = msg.Author;
            var guild = ((SocketGuildChannel) msg.Channel).Guild;

            foreach(var role in guild.Roles){
                if(role.Name != "Translator")
                    continue;

                var guildUser = (SocketGuildUser) user;

                if(guildUser.Roles.Contains(role))
                    guildUser.RemoveRoleAsync(role);
                else
                    guildUser.AddRoleAsync(role);
            }

            msg.Channel.SendMessageAsync("Roles of user **" + user.Username + "** adjusted.");

            return true;
        }
    }
}