using System;
using Discord.WebSocket;
using FounnBot.Interfaces;

namespace FounnBot.Commands{
    public class BaseCommand<T> where T : ICommand{
        public bool Run(SocketMessage msg){
            Program.LogMessageToConsole($"{typeof(T).Name} called...");

            return ((T) Activator.CreateInstance(typeof(T))).RunCommand(msg);
        }

        public bool Run(SocketMessage msg, params object[] args){
            Program.LogMessageToConsole($"{typeof(T).Name} called...");

            return ((T) Activator.CreateInstance(typeof(T), args)).RunCommand(msg);
        }
    }
}