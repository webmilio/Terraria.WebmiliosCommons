using static System.Net.WebRequestMethods;

namespace WebCom.Constants;

/// <summary>Taken from <a href="https://github.com/tModLoader/tModLoader/wiki/Vanilla-Interface-layers-values">tModLoader's wiki</a>.</summary>
internal class GameInterfaceLayerNames
{
    public class Vanilla
    {
        // Why aren't these constants? If tModLoader changes something, we don't have to rebuild both the library and the mod, only the library.
        // This means mods that never get updated will still be compatible with whatever new version of the layers comes out.

        ///<summary>Handles logic related to mouse input while using items held on the cursor.</summary>
        public static readonly string InterfaceLogic1 = "Vanilla: Interface Logic 1";

        ///<summary>Draws other players' names, distances, health text, and head icons.</summary>
        public static readonly string MPPlayerNames = "Vanilla: MP Player Names";

        ///<summary>Draws emote bubbles for NPCs and players as well as the NPC chat bubble when hovering over an NPC.</summary>
        public static readonly string EmoteBubbles = "Vanilla: Emote Bubbles";

        ///<summary>Draws the minion targeting marker for manually targeted NPCs.</summary>
        public static readonly string EntityMarkers = "Vanilla: Entity Markers";

        ///<summary>Draws the targeted tile highlight when placing or breaking with smart cursor.</summary>
        public static readonly string SmartCursorTargets = "Vanilla: Smart Cursor Targets";

        ///<summary>Draws the mechanical ruler tile grid.</summary>
        public static readonly string LaserRuler = "Vanilla: Laser Ruler";

        ///<summary>Draws the ruler lines and sets the ruler text value but doesn't draw the text itself.</summary>
        public static readonly string Ruler = "Vanilla: Ruler";

        ///<summary>Draws indicators for NPC lock on.</summary>
        public static readonly string GamepadLockOn = "Vanilla: Gamepad Lock On";

        ///<summary>Draws the radial tile grid overlay for the tile grid option.</summary>
        public static readonly string TileGridOption = "Vanilla: Tile Grid Option";

        ///<summary>Draws NPC home banners and handles logic for manually kicking out NPCs from their home.</summary>
        public static readonly string TownNPCHouseBanners = "Vanilla: Town NPC House Banners";

        ///<summary>Handles logic for hiding UI based on the hide UI toggle.</summary>
        public static readonly string HideUIToggle = "Vanilla: Hide UI Toggle";

        ///<summary>Draws and handles logic for the wire selection radial menu and wire mode cursor preview. Also draws the golf swing power gague and golf ball indicator arrow.</summary>
        public static readonly string WireSelection = "Vanilla: Wire Selection";

        ///<summary>Draws and handles logic for camera mode.</summary>
        public static readonly string CaptureManagerCheck = "Vanilla: Capture Manager Check";

        ///<summary>Draws and handles logic for the ingame options menu.</summary>
        public static readonly string IngameOptions = "Vanilla: Ingame Options";

        ///<summary>Draws Fancy UIs such as the Achievements UI, Virutal Keyboard UI, and Mod Config UI.</summary>
        public static readonly string FancyUI = "Vanilla: Fancy UI";

        ///<summary>Draws ingame notifications for Achievements and Join Requests.</summary>
        public static readonly string AchievementCompletePopups = "Vanilla: Achievement Complete Popups";

        ///<summary>Draws health bars for NPCs and other players.</summary>
        public static readonly string EntityHealthBars = "Vanilla: Entity Health Bars";

        ///<summary>Draws invasion progress bars and boss health bars.</summary>
        public static readonly string InvasionProgressBars = "Vanilla: Invasion Progress Bars";

        ///<summary>Draws the ingame minimap. (Doesn't draw the fullscreen map or overlay map)</summary>
        public static readonly string MapMinimap = "Vanilla: Map / Minimap";

        ///<summary>Draws network diagnostics.</summary>
        public static readonly string DiagnoseNet = "Vanilla: Diagnose Net";

        ///<summary>Draws rendering diagnostics.</summary>
        public static readonly string DiagnoseVideo = "Vanilla: Diagnose Video";

        ///<summary>Draws the sign chat bubble when hovering over a sign.</summary>
        public static readonly string SignTileBubble = "Vanilla: Sign Tile Bubble";

        ///<summary>Draws the stylist hairstyle menu.</summary>
        public static readonly string HairWindow = "Vanilla: Hair Window";

        ///<summary>Draws the dresser menu.</summary>
        public static readonly string DresserWindow = "Vanilla: Dresser Window";

        ///<summary>Draws dialogue menus for NPCs and signs.</summary>
        public static readonly string NPCSignDialog = "Vanilla: NPC / Sign Dialog";

        ///<summary>Handles logic related to inventory colors.</summary>
        public static readonly string InterfaceLogic2 = "Vanilla: Interface Logic 2";

        ///<summary>Draws health, mana, and breath bars, as well as buff icons.</summary>
        public static readonly string ResourceBars = "Vanilla: Resource Bars";

        ///<summary>Handles logic for when the player inventory is closed.</summary>
        public static readonly string InterfaceLogic3 = "Vanilla: Interface Logic 3";

        ///<summary>Draws and handles logic for everything inventory related.</summary>
        public static readonly string Inventory = "Vanilla: Inventory";

        ///<summary>Draws and handles logic for PDA informations.</summary>
        public static readonly string InfoAccessoriesBar = "Vanilla: Info Accessories Bar";

        ///<summary>Draws and handles logic for the Settings button.</summary>
        public static readonly string SettingsButton = "Vanilla: Settings Button";

        ///<summary>Draws and handles logic for the hotbar.</summary>
        public static readonly string Hotbar = "Vanilla: Hotbar";

        ///<summary>Draws and handles logic for builder accessory toggles.</summary>
        public static readonly string BuilderAccessoriesBar = "Vanilla: Builder Accessories Bar";

        ///<summary>Draws and handles logic for the radial hotbar and radial quickbar.</summary>
        public static readonly string RadialHotbars = "Vanilla: Radial Hotbars";

        ///<summary>Draws ruler measurement text and handles logic for setting mouse text.</summary>
        public static readonly string MouseText = "Vanilla: Mouse Text";

        ///<summary>Draws the chat.</summary>
        public static readonly string PlayerChat = "Vanilla: Player Chat";

        ///<summary>Draws the death text overlay.</summary>
        public static readonly string DeathText = "Vanilla: Death Text";

        ///<summary>Draws the cursor.</summary>
        public static readonly string Cursor = "Vanilla: Cursor";

        ///<summary>Does absolutely nothing.</summary>
        public static readonly string DebugStuff = "Vanilla: Debug Stuff";

        ///<summary>Draws items held on the cursor, also draws and handles logic for manually moving NPC homes.</summary>
        public static readonly string MouseItemNPCHead = "Vanilla: Mouse Item / NPC Head";

        ///<summary>Draws sign text and handles logic for hovering over resource bars, NPCs, players, and dropped items.</summary>
        public static readonly string MouseOver = "Vanilla: Mouse Over";

        ///<summary>Draws custom cursor icons and the currently selected item in the inventory. (Doesn't draw items held on the cursor)</summary>
        public static readonly string InteractItemIcon = "Vanilla: Interact Item Icon";

        ///<summary>Draws gamepad instructions, any pending mouse text, and handles logic related to interacting with NPCs using smart cursor.</summary>
        public static readonly string InterfaceLogic4 = "Vanilla: Interface Logic 4";
    }
}
