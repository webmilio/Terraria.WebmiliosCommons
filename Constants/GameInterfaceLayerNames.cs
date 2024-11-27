using static System.Net.WebRequestMethods;

namespace WebCom.Constants;

/// <summary>Taken from <a href="https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values">tModLoader's wiki</a>.</summary>
public class GameInterfaceLayerNames
{
    public class Vanilla
    {
        // Why aren't these constants? If tModLoader changes something, we don't have to rebuild both the library and the mod, only the library.
        // This means mods that never get updated will still be compatible with whatever new version of the layers comes out.

        ///<summary>Handles logic related to mouse input while using items held on the cursor.</summary>
        public static readonly string InterfaceLogic1 = "Vanilla: Interface Logic 1";
        public static readonly string n00_InterfaceLogic1 = InterfaceLogic1;

        ///<summary>Draws other players' names, distances, health text, and head icons.</summary>
        public static readonly string MPPlayerNames = "Vanilla: MP Player Names";
        public static readonly string n01_MPPlayerNames = MPPlayerNames;

        ///<summary>Draws emote bubbles for NPCs and players as well as the NPC chat bubble when hovering over an NPC.</summary>
        public static readonly string EmoteBubbles = "Vanilla: Emote Bubbles";
        public static readonly string n02_EmoteBubbles = EmoteBubbles;

        ///<summary>Draws the minion targeting marker for manually targeted NPCs.</summary>
        public static readonly string EntityMarkers = "Vanilla: Entity Markers";
        public static readonly string n03_EntityMarkers = EntityMarkers;

        ///<summary>Draws the targeted tile highlight when placing or breaking with smart cursor.</summary>
        public static readonly string SmartCursorTargets = "Vanilla: Smart Cursor Targets";
        public static readonly string n04_SmartCursorTargets = SmartCursorTargets;

        ///<summary>Draws the mechanical ruler tile grid.</summary>
        public static readonly string LaserRuler = "Vanilla: Laser Ruler";
        public static readonly string n05_LaserRuler = LaserRuler;

        ///<summary>Draws the ruler lines and sets the ruler text value but doesn't draw the text itself.</summary>
        public static readonly string Ruler = "Vanilla: Ruler";
        public static readonly string n06_Ruler = Ruler;

        ///<summary>Draws indicators for NPC lock on.</summary>
        public static readonly string GamepadLockOn = "Vanilla: Gamepad Lock On";
        public static readonly string n07_GamepadLockOn = GamepadLockOn;

        ///<summary>Draws the radial tile grid overlay for the tile grid option.</summary>
        public static readonly string TileGridOption = "Vanilla: Tile Grid Option";
        public static readonly string n08_TileGridOption = TileGridOption;

        ///<summary>Draws NPC home banners and handles logic for manually kicking out NPCs from their home.</summary>
        public static readonly string TownNPCHouseBanners = "Vanilla: Town NPC House Banners";
        public static readonly string n09_TownNPCHouseBanners = TownNPCHouseBanners;

        ///<summary>Handles logic for hiding UI based on the hide UI toggle.</summary>
        public static readonly string HideUIToggle = "Vanilla: Hide UI Toggle";
        public static readonly string n10_HideUIToggle = HideUIToggle;

        ///<summary>Draws and handles logic for the wire selection radial menu and wire mode cursor preview. Also draws the golf swing power gague and golf ball indicator arrow.</summary>
        public static readonly string WireSelection = "Vanilla: Wire Selection";
        public static readonly string n11_WireSelection = WireSelection;

        ///<summary>Draws and handles logic for camera mode.</summary>
        public static readonly string CaptureManagerCheck = "Vanilla: Capture Manager Check";
        public static readonly string n12_CaptureManagerCheck = CaptureManagerCheck;

        ///<summary>Draws and handles logic for the ingame options menu.</summary>
        public static readonly string IngameOptions = "Vanilla: Ingame Options";
        public static readonly string n13_IngameOptions = IngameOptions;

        ///<summary>Draws Fancy UIs such as the Achievements UI, Virutal Keyboard UI, and Mod Config UI.</summary>
        public static readonly string FancyUI = "Vanilla: Fancy UI";
        public static readonly string n14_FancyUI = FancyUI;

        ///<summary>Draws ingame notifications for Achievements and Join Requests.</summary>
        public static readonly string AchievementCompletePopups = "Vanilla: Achievement Complete Popups";
        public static readonly string n15_AchievementCompletePopups = AchievementCompletePopups;

        ///<summary>Draws health bars for NPCs and other players.</summary>
        public static readonly string EntityHealthBars = "Vanilla: Entity Health Bars";
        public static readonly string n16_EntityHealthBars = EntityHealthBars;

        ///<summary>Draws invasion progress bars and boss health bars.</summary>
        public static readonly string InvasionProgressBars = "Vanilla: Invasion Progress Bars";
        public static readonly string n17_InvasionProgressBars = InvasionProgressBars;

        ///<summary>Draws the ingame minimap. (Doesn't draw the fullscreen map or overlay map)</summary>
        public static readonly string MapMinimap = "Vanilla: Map / Minimap";
        public static readonly string n18_MapMinimap = MapMinimap;

        ///<summary>Draws network diagnostics.</summary>
        public static readonly string DiagnoseNet = "Vanilla: Diagnose Net";
        public static readonly string n19_DiagnoseNet = DiagnoseNet;

        ///<summary>Draws rendering diagnostics.</summary>
        public static readonly string DiagnoseVideo = "Vanilla: Diagnose Video";
        public static readonly string n20_DiagnoseVideo = DiagnoseVideo;

        ///<summary>Draws the sign chat bubble when hovering over a sign.</summary>
        public static readonly string SignTileBubble = "Vanilla: Sign Tile Bubble";
        public static readonly string n21_SignTileBubble = SignTileBubble;

        ///<summary>Draws the stylist hairstyle menu.</summary>
        public static readonly string HairWindow = "Vanilla: Hair Window";
        public static readonly string n22_HairWindow = HairWindow;

        ///<summary>Draws the dresser menu.</summary>
        public static readonly string DresserWindow = "Vanilla: Dresser Window";
        public static readonly string n23_DresserWindow = DresserWindow;

        ///<summary>Draws dialogue menus for NPCs and signs.</summary>
        public static readonly string NPCSignDialog = "Vanilla: NPC / Sign Dialog";
        public static readonly string n24_NPCSignDialog = NPCSignDialog;

        ///<summary>Handles logic related to inventory colors.</summary>
        public static readonly string InterfaceLogic2 = "Vanilla: Interface Logic 2";
        public static readonly string n25_InterfaceLogic2 = InterfaceLogic2;

        ///<summary>Draws health, mana, and breath bars, as well as buff icons.</summary>
        public static readonly string ResourceBars = "Vanilla: Resource Bars";
        public static readonly string n26_ResourceBars = ResourceBars;

        ///<summary>Handles logic for when the player inventory is closed.</summary>
        public static readonly string InterfaceLogic3 = "Vanilla: Interface Logic 3";
        public static readonly string n27_InterfaceLogic3 = InterfaceLogic3;

        ///<summary>Draws and handles logic for everything inventory related.</summary>
        public static readonly string Inventory = "Vanilla: Inventory";
        public static readonly string n28_Inventory = Inventory;

        ///<summary>Draws and handles logic for PDA informations.</summary>
        public static readonly string InfoAccessoriesBar = "Vanilla: Info Accessories Bar";
        public static readonly string n29_InfoAccessoriesBar = InfoAccessoriesBar;

        ///<summary>Draws and handles logic for the Settings button.</summary>
        public static readonly string SettingsButton = "Vanilla: Settings Button";
        public static readonly string n30_SettingsButton = SettingsButton;

        ///<summary>Draws and handles logic for the hotbar.</summary>
        public static readonly string Hotbar = "Vanilla: Hotbar";
        public static readonly string n31_Hotbar = Hotbar;

        ///<summary>Draws and handles logic for builder accessory toggles.</summary>
        public static readonly string BuilderAccessoriesBar = "Vanilla: Builder Accessories Bar";
        public static readonly string n32_BuilderAccessoriesBar = BuilderAccessoriesBar;

        ///<summary>Draws and handles logic for the radial hotbar and radial quickbar.</summary>
        public static readonly string RadialHotbars = "Vanilla: Radial Hotbars";
        public static readonly string n33_RadialHotbars = RadialHotbars;

        ///<summary>Draws ruler measurement text and handles logic for setting mouse text.</summary>
        public static readonly string MouseText = "Vanilla: Mouse Text";
        public static readonly string n34_MouseText = MouseText;

        ///<summary>Draws the chat.</summary>
        public static readonly string PlayerChat = "Vanilla: Player Chat";
        public static readonly string n35_PlayerChat = PlayerChat;

        ///<summary>Draws the death text overlay.</summary>
        public static readonly string DeathText = "Vanilla: Death Text";
        public static readonly string n36_DeathText = DeathText;

        ///<summary>Draws the cursor.</summary>
        public static readonly string Cursor = "Vanilla: Cursor";
        public static readonly string n37_Cursor = Cursor;

        ///<summary>Does absolutely nothing.</summary>
        public static readonly string DebugStuff = "Vanilla: Debug Stuff";
        public static readonly string n38_DebugStuff = DebugStuff;

        ///<summary>Draws items held on the cursor, also draws and handles logic for manually moving NPC homes.</summary>
        public static readonly string MouseItemNPCHead = "Vanilla: Mouse Item / NPC Head";
        public static readonly string n39_MouseItemNPCHead = MouseItemNPCHead;

        ///<summary>Draws sign text and handles logic for hovering over resource bars, NPCs, players, and dropped items.</summary>
        public static readonly string MouseOver = "Vanilla: Mouse Over";
        public static readonly string n40_MouseOver = MouseOver;

        ///<summary>Draws custom cursor icons and the currently selected item in the inventory. (Doesn't draw items held on the cursor)</summary>
        public static readonly string InteractItemIcon = "Vanilla: Interact Item Icon";
        public static readonly string n41_InteractItemIcon = InteractItemIcon;

        ///<summary>Draws gamepad instructions, any pending mouse text, and handles logic related to interacting with NPCs using smart cursor.</summary>
        public static readonly string InterfaceLogic4 = "Vanilla: Interface Logic 4";
        public static readonly string n42_InterfaceLogic4 = InterfaceLogic4;
    }
}
