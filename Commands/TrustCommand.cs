using System.Linq;
using Discord.WebSocket;
using FounnBot.Classes;
using FounnBot.Interfaces;

namespace FounnBot.Commands{
    public class TrustCommand : ICommand{
        
        private Config Config;

        public TrustCommand(Config config)
        {
            this.Config = config;   
        }

        public bool RunCommand(SocketMessage msg){ 
            if(msg.MentionedUsers.Count() != 1){
                msg.Channel.SendMessageAsync("Please mention one user you want to trust or untrust!");
                return false;
            }

            var user = msg.Author;
            var guild = ((SocketGuildChannel) msg.Channel).Guild;
            var mentionedUser = msg.MentionedUsers.ToArray()[0];

            if(user.Id != guild.OwnerId || user.Id != Config.BotOwner){
                msg.Channel.SendMessageAsync("You don't have permissions to do that. You either need to be server owner, or bot owner");
                return false;
            }

            var result = Database.TrustPerson(guild.Id, mentionedUser.Id);

            switch(result){
                case 0:
                    msg.Channel.SendMessageAsync($"You untrusted **{mentionedUser.Username}** on **{guild.Name}**");
                    break;
                case 1:
                    msg.Channel.SendMessageAsync($"You trusted **{mentionedUser.Username}** on **{guild.Name}**");
                    break;
                default:
                    msg.Channel.SendMessageAsync("**I SHOULD NOT SAY THAT. KICK FOUNNTAIN IN THE ASS WHEN YOU SEE THIS!**");
                    break;
            }

            return true;
        }
    }
}