namespace WebmilioCommons.States
{
    public abstract class State
    {
        protected State(object tracked)
        {
            Tracked = tracked;
        }


        public object Tracked { get; }
    }
}