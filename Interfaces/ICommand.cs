using Discord.WebSocket;

namespace FounnBot.Interfaces{
    public interface ICommand{
        bool RunCommand(SocketMessage msg);
    }
}