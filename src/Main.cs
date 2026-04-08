using BepInEx;
using HarmonyLib;
using Logger = BepInEx.Logging.Logger;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Reflection;

namespace NotEnoughMadness
{
    //[BepInPlugin(NEMConfig.PluginGUID, NEMConfig.PluginName, NEMConfig.PluginVersion)]
    //[BepInProcess("Madness Project Nexus.exe")]
    public class Main : IModEntrypoint
    {
        static GameObject NEMGameObject;
        static Harmony harmony = new Harmony(NEMConfig.PluginGUID);
        public void Initialize()
        {
            // Plugin startup logic
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            //Logger.LogInfo($"{NEMConfig.Version} is loaded!");
            Debug.Log($"{NEMConfig.Version} is loaded!");
            Debug.Log(".NET VERSION: " + RuntimeInformation.FrameworkDescription);

            // Create gameobject for the nem menu and add the component to it

            NEMGameObject = new GameObject("NEMMenuObject");

            NEMGameObject.isStatic = true;
            NEMGameObject.AddComponent<NEMMenu>();
            GameObject.DontDestroyOnLoad(NEMGameObject);
        }

        public void Unload()
        {
            GameObject.Destroy(NEMGameObject);
            NEMGameObject = null;

            harmony.UnpatchSelf();
            Debug.Log("Not Enough Madness Unloaded!");
        }
    }


}