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

            return true;
        }
    }

    

    // This runs at the start of the game and every time you reapply mods (After the clearing above)

    // TODO:
    // ALL THIS was BROKEN with an update lol rip and i will check it out later at some point in teh future eventually for sure. perchance.
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
            foreach (ModItem modItem in Mod_Manager.ReturnAllMods(false))
            {
                // CHECK DIRECTORY FOR .NEM FILES
                foreach (FileInfo fileInfo in modItem.Directory.GetFiles("*", SearchOption.AllDirectories))
                {
                    // NEM ASSET BUNDLES
                    if (fileInfo.Extension == ".nem" || fileInfo.Extension == "")
                    {
                        AssetBundleLoader.ProcessModBundle(fileInfo);
                    }
                    
                    // HANDLED IN VANILLA NOW
                    // CODE EXECUTION (danger 😱😱😱)
                    /*if (fileInfo.Extension == ".dll")
                    {
                        // dont load if code execution is disabled
                        if (NEMConfig.CodeExecutionModsOn.Value == false)
                        {
                            continue;
                        }

                        // LOADING ASSEMBLY

                        AssemblyLoader.LoadPlugin(fileInfo.Name);
                    }*/

                    // ADDITIVE SCENE LOADING
                    // loading more stuff on top of the current scene
                    // MODIFYING LOADED SCENES
                    // changing what's already inside a scene, dynamically looking stuff up
                    // there's an example below to explain the concept

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


// Example scene data json
/*
{
	"arenahub_randomboxesandstuff": {
		"additiveScenes": [
			"loadinfinitesceneslol"
		],
		"modifications": {
			"thecube": {
				"components": {
					"InteractiveNoteComponentIdk": {
						"NoteText": "Etched into its surface are words: \"This cube... this cube has seen some things.. And some of them were cool. And some of them even cooler!",
						"FireEventSystemEventIdkIDidntUseItMuchYet": "activateSpookyBadGuysBehindYou"
					}
				},
				"tag": "Floor",
				"layer": 0
			}
		}
	},
	"hub_arenamode": {
		"additiveScenes": [
			"arenahub_moreworldchanges",
			"arenahub_randomboxesandstuff"
		],
		"modifications": {
			"MainDoor": {
				"components": {
					"UnityEngine.BoxCollider": {
						"enabled": false,
						"isTrigger": true,
                        "nonExistentProperty": 17,
                        "existentProperty": invalidValue
					},
					"Rigidbody": {
						"useGravity": false,
						"mass": 2.5
					},
					"ModNamespace.ModdedComponent": {
						"soundPath": "/music/smc5/club madblox"
					},
					"ModdedComponentWithNoNamespace": {
						"characterCardToSpawn": "Hank"
					},
					"InvalidNonExistingComponent": {
						"value": 123
					}
				}
			},
			"MainDoor/DoorKnob (aka path to the specific gameobject youre modifying)": {
				"position": { "x": 0.2, "y": 1.0, "z": 0.0 },
				"components": {
					"MeshRenderer": {
						"enabled": false
					}
				}
			}
		}
	}
}
 */