using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace NotEnoughMadness.Patches
{
    // This runs when you apply disabled/enabled mods but it doesn't go through all directories again so you have to 
    [HarmonyPatch(typeof(Mod_Manager), "CommitAllModsIfPending")]
    public class Mod_Manager_CommitAllModsIfPending
    {
        [HarmonyPrefix]
        static bool Prefix(Mod_Manager __instance)
        {

            // Clear custom scene paths and unload their bundles
            Debug.Log("NEM: Unloading all mods.");

            AssetBundleLoader.Cleanup();
            SceneModLoader.Cleanup();
            AssemblyLoader.Cleanup();

            return true;
        }
    }

    

    // This runs at the start of the game and every time you reapply mods (After the clearing above)
    [HarmonyPatch(typeof(Mod_Manager), "CommitMods_MadObjs")]
    public class Mod_Manager_CommitMods_MadObj
    {
        [HarmonyPrefix]
        static bool Prefix(Mod_Manager __instance)
        {
            // GET ALL ENABLED MODS
            // CHECK DIRECTORIES FOR .MOBJ FILES (ALTERNATIVELY .NEM FILES ???)
            // 

            // FOR ALL ENABLED MODS
            foreach (Mod_Manager.ModItem modItem in Mod_Manager.ReturnAllMods(false))
            {
                // CHECK DIRECTORY FOR .NEM FILES
                foreach (FileInfo fileInfo in modItem.Directory.GetFiles("*", SearchOption.AllDirectories))
                {
                    // NEM ASSET BUNDLES
                    if (fileInfo.Extension == ".nem" || fileInfo.Extension == "")
                    {
                        AssetBundleLoader.ProcessModBundle(fileInfo);
                    }
                    // CODE EXECUTION (danger 😱😱😱)
                    if (fileInfo.Extension == ".dll")
                    {
                        // dont load if code execution is disabled
                        if (NEMConfig.CodeExecutionModsOn.Value == false)
                        {
                            continue;
                        }

                        // LOADING ASSEMBLY

                        AssemblyLoader.LoadPlugin(fileInfo.Name);
                    }

                    // ADDITIVE SCENE LOADING
                    // loading more stuff on top of the current scene
                    // MODIFYING LOADED SCENES
                    // changing what's already inside a scene, dynamically looking stuff up

                    // nem bundles have to be already loaded (higher up)
                    if (fileInfo.Name == "NEMSceneData.json") 
                    {
                        SceneModLoader.LoadSceneData(fileInfo.FullName);
                    }
                }
            }


            return true;
        }
    }
}
