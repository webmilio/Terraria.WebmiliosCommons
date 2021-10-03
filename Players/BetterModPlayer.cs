using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WebmilioCommons.Extensions;
using WebmilioCommons.Items.Starting;
using WebmilioCommons.Reflection;
using WebmilioCommons.Saving;

namespace WebmilioCommons.Players
{
    public abstract class BetterModPlayer : ModPlayer
    {
        public override void SaveData(TagCompound tag)
        {
            if (!PreSave(tag))
                return;

            ModContent.GetInstance<AutoSaveHandler>().Save(this, tag);
            ModSave(tag);
        }

        protected virtual bool PreSave(TagCompound tag) => true;
        protected virtual void ModSave(TagCompound tag) { }

        public override void LoadData(TagCompound tag)
        {
            if (!PreLoad(tag))
                return;

            ModContent.GetInstance<AutoSaveHandler>().Load(this, tag);
            ModLoad(tag);
        }

        protected virtual bool PreLoad(TagCompound tag) => true;
        protected virtual void ModLoad(TagCompound tag) { }

        public virtual bool CanInteractWithTownNPCs() => true;

        public virtual void OnLastInteractionNPCLoot(NPC npc)
        {
        }
    }
}