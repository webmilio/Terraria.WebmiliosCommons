namespace WebmilioCommons.Identity
{
    public class SteamIdentity : Identity
    {
        public SteamIdentity(long steamID64)
        {
            SteamID64 = steamID64;
        }
        

        internal bool Verify(long steamID64) => steamID64 == SteamID64;


        public long SteamID64 { get; }
    }
}