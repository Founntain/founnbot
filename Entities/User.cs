namespace FounnBot.Entities{
    public class User{
        public int ID {get; set;}
        public ulong UserId {get; set;}
        public Guild Guild {get; set;}
        public ulong TotalXp {get; set;}
        public ulong Xp {get; set;}
        public short Level {get; set;} 
    }
}