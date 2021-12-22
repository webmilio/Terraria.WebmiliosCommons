using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.Localization;
using Terraria.Social.Steam;
using Terraria.ModLoader;
using Terraria.Social;
using Terraria.Social.Base;
using WebmilioCommons.Extensions;

namespace WebmilioCommons;

internal class ModTagsSystem : ModSystem
{
    private const string AddModTagName = "AddModTag";
    private readonly MethodInfo _addModTag;

    private readonly List<string> _addedInternals = new();

    public ModTagsSystem()
    {
        try
        {
            _addModTag = typeof(SupportedWorkshopTags).GetMethod(AddModTagName, BindingFlags.NonPublic | BindingFlags.Instance);
        }
        catch (Exception e)
        {
            Mod.Logger.Warn("Failed to add supported mod workshop tags. Adding tags to mods via Webmilio's Commons is currently unsupported.", e);
        }
    }

    public override void Load()
    {
        if (_addModTag == null)
            return;

        AddModTag(
            ("QualityofLife", "quality of life"),
            ("Tweaks", "tweaks"),
            ("Utility", "utility"),
            ("NewContent", "new content"),
            ("TotalConversion", "total conversion"),
            ("Worldgen", "world gen"),
            ("Interface", "interface"),
            ("Localization", "localization"),
            ("ClientsideOnly", "clientside-only"),
            ("ServersideOnly", "serverside-only"),
            ("Library", "library"));
    }

    public override void Unload()
    {
        if (_addModTag == null)
            return;

        _addedInternals.Do(n => SupportedTags.ModTags.RemoveAll(t => t.InternalNameForAPIs.Equals(n)));
        _addedInternals.Clear();
    }

    public void AddModTag(params (string key, string name)[] tags) => AddModTag(SupportedTags, tags);

    public void AddModTag(AWorkshopTagsCollection workshopTags, params (string key, string name)[] tags)
    {
        if (_addModTag == null)
            return;

        const string localizationPrefix = $"Mods.{nameof(WebmilioCommons)}.ModTags";

        tags.Do(delegate ((string key, string name) tag)
        {
            _addModTag.Invoke(workshopTags, new object[] { $"{localizationPrefix}.{tag.key}", tag.name });
            _addedInternals.Add(tag.name);
        });
    }

    public AWorkshopTagsCollection SupportedTags
    {
        get => SocialAPI.Workshop.SupportedTags;
    }
}