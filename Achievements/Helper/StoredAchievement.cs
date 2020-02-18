using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace WebmilioCommons.Achievements.Helper
{
    internal class StoredAchievement
    {
        public Dictionary<string, JObject> Conditions;
    }
}