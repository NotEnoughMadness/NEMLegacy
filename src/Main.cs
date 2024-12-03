using BepInEx;
using HarmonyLib;
using Logger = BepInEx.Logging.Logger;
using UnityEngine;

namespace NotEnoughMadness
{
    [BepInPlugin(NEMConfig.PluginGUID, NEMConfig.PluginName, NEMConfig.PluginVersion)]
    [BepInProcess("Madness Project Nexus.exe")]
    public class Main : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Harmony harmony = new Harmony(NEMConfig.PluginGUID);
            harmony.PatchAll();

            Logger.LogInfo($"{NEMConfig.Version} is loaded!");


            // Create gameobject for the nem menu and add the component to it

            GameObject nemGameobject = new GameObject("NEMMenuObject");
            
            nemGameobject.isStatic = true;
            nemGameobject.AddComponent<NEMMenu>();
            GameObject.DontDestroyOnLoad(nemGameobject);
        }
    }


}