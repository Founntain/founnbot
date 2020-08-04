using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using FounnBot.Contexts;
using FounnBot.Entities;

namespace FounnBot.Classes{
    public class Database{
        public static Config Config { get; set;}
        public static FounnBotContext Context {get; set;}

        public static ICollection<Prefix> GetGuildsFromDatabase(){
            return Context.Guilds.ToList().Select(x => new Prefix{
                Guild = x.GuildId,
                GuildPrefix = x.Prefix
            }).ToArray();
        }

        public static ICollection<TrustedPerson> GetTrustedPersonsFromDb(){
            return Context.TrustedPersons.ToArray();
        }

        public static IEnumerable<User> GetUsersFromGuild(SocketGuild guild){
            return Context.Users.ToArray().Where(x => x.Guild.GuildId == guild.Id);
        }

        public static int SetPrefix(ulong guild, string prefix){
            var guildOnDb = Context.Guilds.ToArray().FirstOrDefault(x => x.GuildId == guild);

            if(guildOnDb == null){
                Context.Guilds.Add(new Guild{
                    GuildId = guild,
                    Prefix = prefix
                });

                Context.SaveChanges();

                return 0;
            }

            guildOnDb.Prefix = prefix;

            Context.SaveChanges();

            return 1;
        }

        public static int TrustPerson(ulong guild, ulong user){
            var isUserTrusted = Context.TrustedPersons.Any(x => x.GuildId == guild && x.UserId == user);

            if(isUserTrusted){
                var trustedPerson = Context.TrustedPersons.FirstOrDefault(x => x.GuildId == guild && x.UserId == x.UserId);

                Context.TrustedPersons.Remove(trustedPerson);

                Context.SaveChanges();

                return 0;
            }
            
            Context.Add(new TrustedPerson{
                GuildId = guild,
                UserId = user
            });
            
            Context.SaveChanges();

            return 1;
        }

        internal static void AddGuildToDbIfNotExisting(SocketGuild guild)
        {
            if(Context.Guilds.Any(x => x.GuildId == guild.Id))
                return;

            Context.Guilds.Add(new Guild{
                GuildId = guild.Id
            });

            Context.SaveChanges();
        }

        private static bool CheckIfUserIsAlreadyInDb(ulong guild, ulong user){
            return Context.Users.Any(x => x.Guild.GuildId == guild && x.UserId == user);
        }

        private static Guild GetGuildById(ulong guild){
            return Context.Guilds.FirstOrDefault(x => x.GuildId == guild);
        }

        public static void RefreshXpOfAuthor(SocketMessage msg, SocketGuild guild, SocketUser author){
            if(!CheckIfUserIsAlreadyInDb(guild.Id, author.Id)){
                Context.Users.Add(new User{
                    UserId = author.Id,
                    Guild = GetGuildById(guild.Id),
                    TotalXp = 1,
                    Xp = 1,
                    Level = 1
                });

                Context.SaveChanges();

                return;
            }

            var user = Context.Users.FirstOrDefault(x => x.UserId == author.Id);

            user.Xp++;
            user.TotalXp++;

            var xpNeeded = XpNeededForNextLevel(user.Level);

            while(user.Xp >= xpNeeded){
                user.Level++;

                msg.Channel.SendMessageAsync($"**{author.Username}** you are now Level **{user.Level}** on **{guild.Name}**");

                user.Xp = user.Xp - xpNeeded == 0
                    ? 0
                    : user.Xp - xpNeeded;
            }

            Context.SaveChanges();
        }

        private static ulong XpNeededForNextLevel(short level){
            return (ulong)Math.Round(0.04 * (Math.Pow(level, 3)) + 0.8 * (Math.Pow(level, 2)) + 2 * level);
        }
    }
}