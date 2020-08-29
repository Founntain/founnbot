using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using FounnBot.Classes;
using FounnBot.Commands;
using FounnBot.Contexts;
using Newtonsoft.Json;

namespace FounnBot
{
    class Program
    {
        public Config Config;

        public DiscordSocketClient Client;

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public static void LogMessageToConsole(string message){
            Console.WriteLine($"{DateTime.Now.ToString("T")} => {message}");
        }

        public async Task MainAsync(){
            this.LoadConfig();
            this.ConnectToDatabase();

            this.Client = new DiscordSocketClient();

            this.Client.Log += this.Log;

            await this.Client.LoginAsync(TokenType.Bot, this.Config.BotToken);
            await this.Client.StartAsync();
            
            this.Client.MessageReceived += this.MessageReceived;

            this.Client.Ready += () => {
                LogMessageToConsole("FounnBot is connected to Discord");

                this.Client.Guilds.ToList().ForEach(x => Database.AddGuildToDbIfNotExisting(x));

                if(System.Diagnostics.Debugger.IsAttached)
                    this.Client.SetGameAsync("DEBUG MODE", null, ActivityType.Watching);

                return Task.CompletedTask;
            };

            await Task.Delay(-1);
        }

        private void ConnectToDatabase()
        {
            Database.Context = new FounnBotContext(this.Config.ConnectionString);
            LogMessageToConsole("Connected to founnbot@vserver.founntain.de database");
        }

        private void LoadConfig(){
            this.Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));

            Database.Config = this.Config;
        }

        private Task Log(LogMessage msg){
            LogMessageToConsole(msg.ToString());
            return Task.CompletedTask;
        }

        private Task MessageReceived(SocketMessage msg)
        {
            var user = msg.Author;
            var guild = ((SocketGuildChannel) msg.Channel).Guild;

            var guildsFromDb = Database.GetGuildsFromDatabase().ToList();

            if(!user.IsBot && !user.IsWebhook)
                Database.RefreshXpOfAuthor(msg, guild, user);

            var prefix = guildsFromDb.Count > 0 
                ? guildsFromDb.FirstOrDefault(x => x.Guild == guild.Id)?.GuildPrefix ?? this.Config.DefaultPrefix
                : this.Config.DefaultPrefix;

            if(!msg.Content.StartsWith(prefix))
                return Task.CompletedTask;

            if(Debugger.IsAttached && user.Id != this.Config.BotOwner){
                msg.Channel.SendMessageAsync("Bot is currently running in **DEBUG-MODE**. Only the Bot-Owner is allowed to use commands in that time. However you **still get XP as usual!**");
                return Task.CompletedTask;
            }

            if(msg.Content.StartsWith(prefix) && msg.Content.Split(prefix)[1].ToLower().StartsWith("help")){
                new BaseCommand<HelpCommand>().Run(msg, this.Config);

                return Task.CompletedTask;
            }
                
            if(msg.Content.StartsWith(prefix) && msg.Content.Split(prefix)[1].ToLower().StartsWith("kitten")){
                new BaseCommand<KittenCommand>().Run(msg);

                return Task.CompletedTask;
            }

            if(msg.Content.StartsWith(prefix) && msg.Content.Split(prefix)[1].ToLower().StartsWith("cat")){
                new BaseCommand<CatCommand>().Run(msg);

                return Task.CompletedTask;
            }

            if(msg.Content.StartsWith(prefix) && msg.Content.Split(prefix)[1].ToLower().StartsWith("dog")){
                new BaseCommand<DogCommand>().Run(msg);

                return Task.CompletedTask;
            }

            if(msg.Content.StartsWith(prefix) && msg.Content.Split(prefix)[1].ToLower().StartsWith("fox")){
                new BaseCommand<FoxCommand>().Run(msg);

                return Task.CompletedTask;
            }

            if(msg.Content.StartsWith(prefix) && msg.Content.Split(prefix)[1].ToLower().StartsWith("setprefix")){
                new BaseCommand<SetPrefixCommand>().Run(msg, this.Config);

                return Task.CompletedTask;
            }

            if(msg.Content.StartsWith(prefix) && msg.Content.Split(prefix)[1].ToLower().StartsWith("trust")){
                new BaseCommand<TrustCommand>().Run(msg, this.Config);

                return Task.CompletedTask;
            }

            if(msg.Content.StartsWith(prefix) && msg.Content.Split(prefix)[1].ToLower().StartsWith("ranking")){
                new BaseCommand<RankingCommand>().Run(msg, guild);

                return Task.CompletedTask;
            }

            if(msg.Content.StartsWith(prefix) && msg.Content.Split(prefix)[1].ToLower().StartsWith("createrole")){
                new BaseCommand<CreateRoleCommand>().Run(msg, guild);

                return Task.CompletedTask;
            }

            if(this.OsuPlayerDiscordCommands(msg, guild, prefix)) return Task.CompletedTask;

            msg.Channel.SendMessageAsync("Could not find command!");

            return Task.CompletedTask;
        }

        private bool OsuPlayerDiscordCommands(SocketMessage msg, SocketGuild guild, string prefix){

            if(guild.Id != Config.OsuPlayerDiscordId)
                return false;

            if(msg.Content.StartsWith(prefix) && msg.Content.Split(prefix)[1].ToLower().StartsWith("translator")){
                new BaseCommand<TranslatorCommand>().Run(msg);

                return true;
            }

            return false;
        }
    }
}
