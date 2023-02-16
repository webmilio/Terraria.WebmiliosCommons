using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace WebmilioCommons.UI;

public class UILayer<T> : UILayer where T : UIState
{
    public UILayer(T state, string name, InterfaceScaleType scaleType) : base(name, scaleType)
    {
        State = state;

        Interface = new();
        Interface.SetState(State);
    }

    public T State { get; }
}

public class UILayer : GameInterfaceLayer
{
    public UILayer(string name, InterfaceScaleType scaleType) : base(name, scaleType)
    {
    }

    public virtual void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index = IndexProvider(layers);
        
        if (index >= 0)
            layers.Insert(index, this);
        else if (index == layers.Count)
            layers.Add(this);
    }

    public virtual void Update(GameTime gameTime)
    {
        if (ShouldUpdate())
            Interface.Update(gameTime);
    }

    protected override bool DrawSelf()
    {
        if (IsVisible)
            Interface.Draw(Main.spriteBatch, Main.gameTimeCache);

        return true;
    }

    public bool IsVisible
    {
        get => Interface.IsVisible;
        set => Interface.IsVisible = value;
    }
    
    public UserInterface Interface { get; protected init; }

    public virtual Func<List<GameInterfaceLayer>, int> IndexProvider { get; init; } = _ => VanillaLayers.Cursor_Id;
    public virtual Func<bool> ShouldUpdate => () => IsVisible;

    public static class VanillaLayers
    {
        public const string
            VanillaPrefix = "Vanilla: ",

            AchievementCompletePopups_Name = VanillaPrefix + "Achievement Complete Popups",

            BuilderAccessoriesBar_Name = VanillaPrefix + "Builder Accessories Bar",

            CaptureManagerCheck_Name = VanillaPrefix + "Capture Manager Check",
            Cursor_Name = VanillaPrefix + "Cursor",

            DeathText_Name = VanillaPrefix + "Death Text",
            DebugStuff_Name = VanillaPrefix + "Debug Stuff",
            DiagnoseNet_Name = VanillaPrefix + "Diagnose Net",
            DiagnoseVideo_Name = VanillaPrefix + "Diagnose Video",
            DresserWindow_Name = VanillaPrefix + "Dresser Window",

            EmoteBubbles_Name = VanillaPrefix + "Emote Bubbles",
            EntityHealthBars_Name = VanillaPrefix + "Entity Health Bars",
            EntityMarkers_Name = VanillaPrefix + "Entity Markers",

            FancyUI_Name = VanillaPrefix + "Fancy UI",

            GamepadLockOn_Name = VanillaPrefix + "Gamepad Lock On",

            HairWindow_Name = VanillaPrefix + "Hair Window",
            HideUIToggle_Name = VanillaPrefix + "Hide UI Toggle",
            Hotbar_Name = VanillaPrefix + "Hotbar",

            InfoAccessoriesBar_Name = VanillaPrefix + "Info Accessories Bar",
            IngameOptions_Name = VanillaPrefix + "Ingame Options",
            InteractItemIcon_Name = VanillaPrefix + "Interact Item Icon",
            InterfaceLogic1_Name = VanillaPrefix + "Interface Logic 1",
            InterfaceLogic2_Name = VanillaPrefix + "Interface Logic 2",
            InterfaceLogic3_Name = VanillaPrefix + "Interface Logic 3",
            InterfaceLogic4_Name = VanillaPrefix + "Interface Logic 4",
            InvasionProgressBars_Name = VanillaPrefix + "Invasion Progress Bars",
            Inventory_Name = VanillaPrefix + "Inventory",

            LaserRules_Name = VanillaPrefix + "Laser Ruler",

            MapMinimap_Name = VanillaPrefix + "Map / Minimap",
            MouseItem_NPCHead_Name = VanillaPrefix + "Mouse Item / NPC Head",
            MouseOver_Name = VanillaPrefix + "Mouse Over",
            MouseText_Name = VanillaPrefix + "Mouse Text",
            MPPlayerNames_Name = VanillaPrefix + "MP Player Names",

            NPCSignDialog_Name = VanillaPrefix + "NPC / Sign Dialog",

            PlayerChat_Name = VanillaPrefix + "Player Chat",

            SettingsButton_Name = VanillaPrefix + "Settings Button",
            SignTileBubble_Name = VanillaPrefix + "Sign Tile Bubble",
            SmartCursorTargets_Name = VanillaPrefix + "Smart Cursor Targets",

            RadialHotbars_Name = VanillaPrefix + "Radial Hotbars",
            ResourceBars_Name = VanillaPrefix + "Resource Bars",
            Ruler_Name = VanillaPrefix + "Ruler",

            TileGridOption_Name = VanillaPrefix + "Tile Grid Option",
            TownNPCHouseBanners_Name = VanillaPrefix + "Town NPC House Banners",

            WireSelection_Name = VanillaPrefix + "Wire Selection";

        public const int
            InterfaceLogic1_Id = 0,
            MPPlayerNames_Id = InterfaceLogic1_Id + 1,
            EmoteBubbles_Id = MPPlayerNames_Id + 1,
            EntityMarkers_Id = EmoteBubbles_Id + 1,
            SmartCursorTargets_Id = EntityMarkers_Id + 1,
            LaserRules_Id = SmartCursorTargets_Id + 1,
            Ruler_Id = LaserRules_Id + 1,
            GamepadLockOn_Id = Ruler_Id + 1,
            TileGridOption_Id = GamepadLockOn_Id + 1,
            TownNPCHouseBanners_Id = TileGridOption_Id + 1,
            HideUIToggle_Id = TownNPCHouseBanners_Id + 1,
            WireSelection_Id = HideUIToggle_Id + 1,
            CaptureManagerCheck_Id = WireSelection_Id + 1,
            IngameOptions_Id = CaptureManagerCheck_Id + 1,
            FancyUI_Id = IngameOptions_Id + 1,
            AchievementCompletePopups_Id = FancyUI_Id + 1,
            EntityHealthBars_Id = AchievementCompletePopups_Id + 1,
            InvasionProgressBars_Id = EntityHealthBars_Id + 1,
            MapMinimap_Id = InvasionProgressBars_Id + 1,
            DiagnoseNet_Id = MapMinimap_Id + 1,
            DiagnoseVideo_Id = DiagnoseNet_Id + 1,
            SignTileBubble_Id = DiagnoseVideo_Id + 1,
            HairWindow_Id = SignTileBubble_Id + 1,
            DresserWindow_Id = HairWindow_Id + 1,
            NPCSignDialog_Id = DresserWindow_Id + 1,
            InterfaceLogic2_Id = NPCSignDialog_Id + 1,
            ResourceBars_Id = InterfaceLogic2_Id + 1,
            InterfaceLogic3_Id = ResourceBars_Id + 1,
            Inventory_Id = InterfaceLogic3_Id + 1,
            InfoAccessoriesBar_Id = Inventory_Id + 1,
            SettingsButton_Id = InfoAccessoriesBar_Id + 1,
            Hotbar_Id = SettingsButton_Id + 1,
            BuilderAccessoriesBar_Id = Hotbar_Id + 1,
            RadialHotbars_Id = BuilderAccessoriesBar_Id + 1,
            MouseText_Id = RadialHotbars_Id + 1,
            PlayerChat_Id = MouseText_Id + 1,
            DeathText_Id = PlayerChat_Id + 1,
            Cursor_Id = DeathText_Id + 1,
            DebugStuff_Id = Cursor_Id + 1,
            MouseItem_NPCHead_Id = DebugStuff_Id + 1,
            MouseOver_Id = MouseItem_NPCHead_Id + 1,
            InteractItemIcon_Id = MouseOver_Id + 1,
            InterfaceLogic4_Id = InteractItemIcon_Id + 1;
    }
}