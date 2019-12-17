using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace WebmilioCommons.Achievements.Helper
{
    public class StoredAchievement
    {
        public Dictionary<string, JObject> Conditions;
    }
}