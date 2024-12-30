using BepInEx;
using BepInEx.Configuration;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace NotEnoughMadness
{
    public static class NEMConfig
    {
        // These three will mostly stay unchanged ever forever eternally until the end of times maybe

        public const string PluginGUID = "xdefault.notenoughmadness";
        public const string PluginName = "NotEnoughMadness";
        public const string PluginVersion = "1.0.0";

        ///////////////

        // Edit this one for changing the number
        private const string _NEMVersion = "2.0";

        // This is the string that will be displayed in the corner
        // I use the wording "w/" rather than "for" because with bepinex you can run an older version of nem on a new mpn version that wasn't tested or updated for
        public static readonly string Version = "NotEnoughMadness v" + _NEMVersion + " w/ M:PN " + Game_Manager.ReturnGameVersion();

        // Create config file
        private static ConfigFile config = new ConfigFile(Path.Combine(Paths.ConfigPath, "NotEnoughMadness.cfg"), true);

        const string core = "CORE";
       
        public static ConfigEntry<bool> ModMenuEnabled = config.Bind(core, "ModMenuEnabled", true, "Do you want the mod menu to show up in game? You probably want this ON. If false, menu doesn't show up if you hold the keybind.");
        public static ConfigEntry<bool> TeleportEnabled = config.Bind(core, "TeleportEnabled", true, "Do you want the teleport keybind to teleport you or do nothing? If true, teleports you to cursor, if false, it doesn't.");
        public static ConfigEntry<bool> TeleportEffectEnabled = config.Bind(core, "TeleportEffectEnabled", true, "If true, doesn't hide the particle effects when teleporting. It uses the character's statcard's teleport effect, and if that doesn't exist, it uses the default red 404 error teleport effect.");
        public static ConfigEntry<bool> DebugModeOn = config.Bind(core, "DebugModeOn", false, "If true, enables the game's debug mode. You probably want this off unless you know what you're doing. It enables hidden objects, doors, additional non-configurable dev keybinds, you can skip stages outside of the beta branch, etc. It can also aid you in creating and testing your custom maps, as you can make your own debug objects that show up only when this is on.");
        public static ConfigEntry<bool> CodeExecutionModsOn = config.Bind(core, "CodeExecutionModsOn", false, "If true, NEM will load dll mods from your workshop and local M:PN mod folders. Off by default, due to the unsafe nature of this feature. Without this some mods may not work correctly, eg. a map with custom functionality. Turn on at your own risk, use only mods you can trust, you don't know what mod authors hide in the code, but also this is a cool feature to mess around with.");

        const string cam = "Camera Settings";

        public static ConfigEntry<bool> CameraToggleEnabled = config.Bind(cam, "CameraToggleEnabled", true, "If false, camera hotkeys won't work and you won't be able to change the camera type.");
        public static ConfigEntry<KeyCode> CameraToggle = config.Bind(cam, "CameraToggle", KeyCode.V, "Key code that toggles between available camera modes, circling back to vanilla.");
        public static ConfigEntry<float> CameraSensivity = config.Bind(cam, "CameraSensivity", 1f, "How sensitive are the custom camera types (not vanilla)");
        public static ConfigEntry<float> CameraSpeed = config.Bind(cam, "CameraSpeed", 20f, "How fast the custom camera types move (not vanilla). If you hold left shift or right shift the speed gets doubled.");
        public static ConfigEntry<float> CameraFOV = config.Bind(cam, "CameraFOV", 90f, "Field of view for custom camera types.");
        
        public static ConfigEntry<KeyCode> CameraMoveForward = config.Bind(cam, "CameraMoveForward", KeyCode.UpArrow, "Moves free cam forward.");
        public static ConfigEntry<KeyCode> CameraMoveBackward = config.Bind(cam, "CameraMoveBackward", KeyCode.DownArrow, "Moves free cam backward.");
        public static ConfigEntry<KeyCode> CameraMoveLeft = config.Bind(cam, "CameraMoveLeft", KeyCode.LeftArrow, "Moves free cam to the left.");
        public static ConfigEntry<KeyCode> CameraMoveRight = config.Bind(cam, "CameraMoveRight", KeyCode.RightArrow, "Moves free cam to the right.");
        public static ConfigEntry<KeyCode> CameraMoveUp = config.Bind(cam, "CameraMoveUp", KeyCode.Keypad7, "Moves free cam up (global/world axis).");
        public static ConfigEntry<KeyCode> CameraMoveDown = config.Bind(cam, "CameraMoveDown", KeyCode.Keypad1, "Moves free cam down (global/world axis).");

        // KeyCodes config
        const string keybinds = "Keybinds";
        
        public static ConfigEntry<KeyCode> ToggleMenu = config.Bind(keybinds, "ToggleMenu", KeyCode.F, "Key code that displays the mod menu.");
        public static ConfigEntry<KeyCode> Teleport = config.Bind(keybinds, "Teleport", KeyCode.Z, "Key code that teleports you to your crosshair. If you have any hirelings selected with CTRL it will teleport them instead.");
        

        // Givers
        const string givers = "Givers";
        
        public static ConfigEntry<int> GiveBoonTokens = config.Bind(givers, "GiveBoonTokens", 5, "How many boon tokens to give.");
        public static ConfigEntry<int> GiveMoney = config.Bind(givers, "GiveMoney", 5000, "How much money to give.");
        public static ConfigEntry<int> GiveImprints = config.Bind(givers, "GiveImprints", 1, "How much imprints to add to your save file.");
        public static ConfigEntry<int> GiveExp = config.Bind(givers, "GiveExp", 5000, "How much experience to give to all four skill trees.");

        // Optional patches
        private const string patchesQol = "Other Patches";

        public static ConfigEntry<bool> MadnessUnlocked = config.Bind(patchesQol, "MadnessUnlocked", true, "Unlock madness difficulty from the start.");
        public static ConfigEntry<bool> OriginsUnlocked = config.Bind(patchesQol, "OriginsUnlocked", true, "Unlock all origins from the start. Imprints will still affect your stats regardless.");

        public static ConfigEntry<bool> UnlockInteractiveRestrictions = config.Bind(patchesQol, "UnlockInteractiveRestrictions", true, "In vanilla M:PN, interactive mode hides certain characters and weapons from the spawn menus. If this is true, it unhides those things.");

        public static ConfigEntry<bool> CreateGhostOfferingPatch = config.Bind(patchesQol, "CreateGhostOfferingPatch", false, "Boosts the offering ghost creation chance to 100%.");
    }


}