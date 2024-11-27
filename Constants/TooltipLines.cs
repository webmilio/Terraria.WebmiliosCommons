using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace WebCom.Constants;

// Created by RockmanZeta
public static class TooltipLines
{
    public const string
        LineItemName = "ItemName",
        LineFavorite = "Favorite",
        LineFavoriteDesc = "FavoriteDesc",
        LineSocial = "Social",
        LineSocialDesc = "SocialDesc",

        LineDamage = "Damage",
        LineCritChance = "CritChange",
        LineSpeed = "Speed",
        LineKnockback = "Knockback",

        NameFishingPower = "FishingPower",
        NameNeedsBait = "NeedsBait",
        NameBaitPower = "BaitPower",

        LineEquipable = "Equipable",
        LineWandConsumes = "WandConsumes",
        LineQuest = "Quest",

        LineVanity = "Vanity",
        LineDefense = "Defense",
        LinePickPower = "PickPower",
        LineAxePower = "AxePower",
        LineHammerPower = "HammerPower",
        LineTileBoost = "TileBoost",

        LineHealLife = "HealLife",
        LineHealMana = "HealMana",
        LineUseMana = "UseMana",

        LinePlaceable = "Placeable",
        LineAmmo = "Ammo",
        LineConsumable = "Consumable",
        LineMaterial = "Material",

        LineTooltipNb = "Tooltip#",
        LineEtherianManaWarning = "EtherianManaWarning",
        LineWellFedExpert = "WellFedExpert",

        LineBuffTime = "BuffTime",
        LineOneDropLogo = "OneDropLogo",

        LinePrefixDamage = "PrefixDamage",
        LinePrefixSpeed = "PrefixSpeed",
        LinePrefixCritChance = "PrefixCritChance",
        LinePrefixUseMana = "PrefixUseMana",
        LinePrefixSize = "PrefixSize",
        LinePrefixShootSpeed = "PrefixShootSpeed",
        LinePrefixKnockback = "PrefixKnockback",
        LinePrefixAccDefense = "PrefixAccDefense",
        LinePrefixAccMaxMana = "PrefixAccMaxMana",
        LinePrefixAccCritChance = "PrefixAccCritChance",
        LinePrefixAccDamage = "PrefixAccDamage",
        LinePrefixAccMoveSpeed = "PrefixAccMoveSpeed",
        LinePrefixAccMeleeSpeed = "PrefixAccMeleeSpeed",

        LineSetBonus = "SetBonus",
        LineExpert = "Expert",
        LineMaster = "Master",

        LineJourneyResearch = "JourneyResearch",
        LineBestiaryNotes = "BestiaryNotes",
        LineSpecialPrice = "SpecialPrice",
        LinePrice = "Price";

    internal static readonly string[] VanillaTooltipNames = new string[]
    {
        LineItemName, LineFavorite, LineFavoriteDesc, LineSocial, LineSocialDesc,
        LineDamage, LineCritChance, LineSpeed, LineKnockback,
        NameFishingPower, NameNeedsBait, NameBaitPower,
        LineEquipable, LineWandConsumes, LineQuest,
        LineVanity, LineDefense, LinePickPower, LineAxePower, LineHammerPower, LineTileBoost,
        LineHealLife, LineHealMana, LineUseMana,
        LinePlaceable, LineAmmo, LineConsumable, LineMaterial,
        LineTooltipNb,
        LineEtherianManaWarning,
        LineWellFedExpert,
        LineBuffTime, LineOneDropLogo,
        LinePrefixDamage, LinePrefixSpeed, LinePrefixCritChance, LinePrefixUseMana, LinePrefixSize, LinePrefixShootSpeed, LinePrefixKnockback,
        LinePrefixAccDefense, LinePrefixAccMaxMana, LinePrefixAccCritChance, LinePrefixAccDamage, LinePrefixAccMoveSpeed, LinePrefixAccMeleeSpeed,
        LineSetBonus, LineExpert, LineMaster,
        LineJourneyResearch, LineBestiaryNotes,
        LineSpecialPrice, LinePrice
    };

    public static void Insert(this List<TooltipLine> lines, TooltipLine line, params string[] names)
    {
        int i;
        for (i = 0; i < lines.Count && !lines[i].Name.Equals(names[i]); i++) ;

        if (i == lines.Count - 1)
        {
            lines.Add(line);
        }
        else
        {
            lines.Insert(line);
        }
    }

    public static int FindLineIndex(string name)
    {
        if (name.StartsWith("Tooltip"))
        {
            name = "Tooltip#";
        }

        for (int i = 0; i < VanillaTooltipNames.Length; i++)
        {
            if (name == VanillaTooltipNames[i])
            {
                return i;
            }
        }

        return -1;
    }

    public static void AddTooltip(this List<TooltipLine> tooltips, TooltipLine line)
    {
        tooltips.Insert(Math.Min(tooltips.GetIndex("Tooltip#"), tooltips.Count), line);
    }

    public static int GetIndex(this List<TooltipLine> tooltips, string lineName)
    {
        int lineIndex = FindLineIndex(lineName);

        for (int i = 0; i < tooltips.Count; i++)
        {
            if (tooltips[i].Mod == "Terraria" && FindLineIndex(tooltips[i].Name) >= lineIndex)
            {
                if (lineName == "Tooltip#")
                {
                    for (; i < tooltips.Count; i++)
                    {
                        if (!tooltips[i].Name.StartsWith("Tooltip"))
                        {
                            return i;
                        }
                    }
                }

                return i;
            }
        }

        return tooltips.Count - 1;
    }
}