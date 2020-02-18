namespace WebmilioCommons.Commons
{
    public struct ManagedResource
    {
        /// <summary>Original ticks.</summary>
        public readonly int original;

        /// <summary>Remaining ticks before expiration.</summary>
        public int remaining;


        public ManagedResource(int original, int remaining)
        {
            this.original = original;
            this.remaining = remaining;
        }


        public void Tick() => remaining--;

        public bool Expired => remaining <= 0;
    }
}