using System;
using System.Globalization;
using System.Linq;
using Discord;
using Discord.WebSocket;
using FounnBot.Interfaces;

namespace FounnBot.Commands{
    public class CreateRoleCommand : ICommand
    {
        SocketGuild Guild {get; set;}

        public CreateRoleCommand(SocketGuild guild)
        {
            this.Guild = guild;
        }

        public bool RunCommand(SocketMessage msg)
        {
            var args = msg.Content.Split(" ");

            if(args.Length < 3){

                msg.Channel.SendMessageAsync("Not enough arguments, enter name and color: **!createrole <name> <colorcode (html format (WITHOUT #))>**");
                return false;
            }

            if(args[1].Length != 6){
                msg.Channel.SendMessageAsync("Your colorcode must be a hexadecimal number with a length of 6!");
                return false;
            }

            var color = string.Empty;

            if(int.TryParse(args[1], NumberStyles.HexNumber, null, out int parsedColor)){
                color = Convert.ToString(parsedColor, 16);
            }else{
                 msg.Channel.SendMessageAsync("Your color is not a valid Hexnumber!");
                return false;
            }

            var name = string.Empty;

            for(var i = 2; i <= args.Length - 1; i++){
                name += args[i] + " ";
            }

            name = name.Substring(0, name.Length - 1);

            if(Guild.Roles.ToList().Any(x => x.Name == name)){
                msg.Channel.SendMessageAsync($"Sorry, on this server already exists a role with the name **{name}**");
                return false;
            }

            var role = Guild.CreateRoleAsync(name, null, new Color((uint) int.Parse(color, NumberStyles.HexNumber)), false, null).Result;

            ((IGuildUser) msg.Author).AddRoleAsync(role);

            msg.Channel.SendMessageAsync($"You created role **{name}** with color **#{color}** and assigned it to yourself!");

            return true;
        }
    }
}