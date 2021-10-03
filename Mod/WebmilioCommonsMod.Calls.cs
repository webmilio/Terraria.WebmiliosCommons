using Terraria.ModLoader;
using WebmilioCommons.Time;

namespace WebmilioCommons
{
    public sealed partial class WebmilioCommonsMod
    {
        public override object Call(params object[] args)
        {
            string callName = args[0].ToString().ToLower();

            switch (callName)
            {
                case "timemanagement_addimmunity":
                    switch (args[1].ToString().ToLower())
                    {
                        case "npc":
                            TimeManagement.AddNPCImmunity(int.Parse(args[2].ToString()));
                            break;

                        case "projectile":
                            TimeManagement.AddProjectileImmunity(int.Parse(args[2].ToString()));
                            break;
                    }

                    break;
            }

            return null;
        }
    }
}
