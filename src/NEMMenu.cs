using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UI_Inventory.InvTab;

namespace NotEnoughMadness
{
    public class NEMMenu : MonoBehaviour
    {
        // GENERAL SETUP
        public static NEMMenu currentNemMenu;

        void Start()
        {
            if (currentNemMenu != null)
            {
                return;
            }

            NEMMenu.currentNemMenu = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        public static Dictionary<string, bool> toggleBools = new Dictionary<string, bool>()
        {
            {"GodMode", false},
            {"InfiniteAmmo", false},
            {"BottomlessMag", false},
            {"CriticalHits", false}
        };

        // THE MEAT
        void OnGUI()
        {
            if (Game_Manager.currentManager == null || Game_Manager.currentManager.sceneIsMenu)
            {
                return;
            }

            if (NEMConfig.ModMenuEnabled.Value == false)
            {
                return;
            }

            // changed cutscene code on beginme (or beginstep idk) at startsound != null (lower added && myTransform to if check)

            // THE ACTUAL GUI
            if (Input.GetKey(NEMConfig.ToggleMenu.Value))
            {
                if (GUILayout.Toggle(toggleBools["GodMode"], "God Mode") != toggleBools["GodMode"])
                {
                    toggleBools["GodMode"] = !toggleBools["GodMode"];
                    if (toggleBools["GodMode"] == true)
                    {
                        Controller_Base.PlayerOne.MakeInvincible();
                    } else
                    {
                        Controller_Base.PlayerOne.EndInvincible();
                    }
                }

                if (GUILayout.Toggle(toggleBools["InfiniteAmmo"], "Infinite Ammo+Melee") != toggleBools["InfiniteAmmo"])
                {
                    toggleBools["InfiniteAmmo"] = !toggleBools["InfiniteAmmo"];
                }
                if (GUILayout.Toggle(toggleBools["BottomlessMag"], "Bottomless Mag") != toggleBools["BottomlessMag"])
                {
                    toggleBools["BottomlessMag"] = !toggleBools["BottomlessMag"];
                }
                if (GUILayout.Toggle(toggleBools["CriticalHits"], "Critical Hits") != toggleBools["CriticalHits"])
                {
                    toggleBools["CriticalHits"] = !toggleBools["CriticalHits"];
                }
                // Remote pickup toggle is changed to a normal button so it doesn't spam your traitlist with the same one trait and doesn't check everything every single frame
                // It could be like the toggles up there if not for it remaining toggled when you swap characters to someone who doesn't have the trait
                if (GUILayout.Button("Toggle Remote Pickup Trait"))
                {
                    if (!Controller_Base.PlayerOne.myData.myTraits.Contains(TraitList.RemotePickup))
                    {
                        Controller_Base.PlayerOne.myData.myTraits.Add(TraitList.RemotePickup);
                        UI_Game.NewNotice("Added the <color=yellow>RemotePickup</color> trait to " + Controller_Base.PlayerOne.myData.myName + "!");
                    }
                    else
                    {
                        Controller_Base.PlayerOne.myData.myTraits.Remove(TraitList.RemotePickup);
                        UI_Game.NewNotice("Removed the <color=yellow>RemotePickup</color> trait from " + Controller_Base.PlayerOne.myData.myName + "!");
                    }
                    
                }

                if (GUILayout.Button("Refill Weapons"))
                {
                    List<Item_Held> list = new List<Item_Held>();
                    
                    list.AddRange(Controller_Base.PlayerOne.myItemsHeld); // main
                    list.AddRange(Controller_Base.PlayerOne.myType_Character.myItemsStowed); // back
                    list.Add(Controller_Base.PlayerOne.ReturnThrown_Item()); // throwables
                    
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].amRanged is not null)
                        {
                            list[i].amRanged.ResetAmmo();
                        }
                        
                    }
                    if (Headquarters_Manager.currentManager)
                    {
                        Controller_Base.PlayerOne.ApplyStartingAmmo();
                    }
                    
                    UI_Game.NewNotice("Dissonant reality absorbed! Ammo refilled!");
                }
                if (GUILayout.Button("Spawn Weapons"))
                {
                    InteractiveMode.Event_SpawnWeapon();
                }
                if (GUILayout.Button("Spawn Units"))
                {
                    InteractiveMode.Event_SpawnUnit();
                }
                if (GUILayout.Button("Spawn Friends"))
                {
                    //InteractiveMode.Event_SpawnFriends();
                    SpawnFriends.Event_SpawnFriends();
                }
                if (GUILayout.Button("Change Character"))
                {
                    InteractiveMode.Event_PlayCharacter();
                }
                if (GUILayout.Button("Clear All"))
                {
                    InteractiveMode.Event_ClearAll();
                }
                if (GUILayout.Button("Music"))
                {
                    InteractiveMode.Event_Music();
                }
                if (GUILayout.Button("Give Money"))
                {
                    Controller_Base.PlayerOne.myData.myCash += NEMConfig.GiveMoney.Value;
                    UI_Game.NewNotice("Gave " + NEMConfig.GiveMoney.Value + " money!");
                }
                if (GUILayout.Button("Give Boon Tokens"))
                {
                    Controller_Base.PlayerOne.myData.myTokens += NEMConfig.GiveBoonTokens.Value;
                    UI_Game.NewNotice("Gave " + NEMConfig.GiveBoonTokens.Value + " boon tokens!");
                }
                if (GUILayout.Button("Give Imprints"))
                {
                    Controller_Base.PlayerOne.myData.ApplyImprint(NEMConfig.GiveImprints.Value);
                    UI_Game.NewNotice("Imprinted " + NEMConfig.GiveImprints.Value + " times!");
                }
                if (GUILayout.Button("Complete Arena", Array.Empty<GUILayoutOption>()))
                {
                    foreach (Arena_Manager.ArenaProgress arenaProgress in new List<Arena_Manager.ArenaProgress>(Enum.GetValues(typeof(Arena_Manager.ArenaProgress)).Cast<Arena_Manager.ArenaProgress>().ToList<Arena_Manager.ArenaProgress>()))
                    {
                        Career_Data.UpdateWorldChangeRecord(arenaProgress.ToString(), SwainM.BoolOps.True);
                    }
                    Controller_Base.PlayerOne.myData.myCareer.highestTier = 100;
                    foreach (StatCard_Stage statCard_Stage in StatCard_Stage.ReturnAllEnabledStages(GameModes.Arena, 1000, true))
                    {
                        StatCard_Stage_Arena statCard_Stage_Arena = (StatCard_Stage_Arena)statCard_Stage;
                        StageSaveData stageSaveData = Controller_Base.PlayerOne.myData.myCareer.ReturnStageDataByCard(statCard_Stage_Arena);
                        stageSaveData.difficultiesWon.Add(GameDifficulty.Easy);
                        stageSaveData.difficultiesWon.Add(GameDifficulty.Normal);
                        stageSaveData.difficultiesWon.Add(GameDifficulty.Hard);
                        stageSaveData.difficultiesWon.Add(GameDifficulty.Madness);
                        stageSaveData.timesWon++;
                    }
                    UI_Game.NewNotice("Unlocked every upgrade station, shop, world change, set highest tier to 100, beaten all arena stages on all difficulties.");
                }
                foreach (string scenePath in AssetBundleLoader.customScenePaths)
                {
                    if (GUILayout.Button(scenePath))
                    {
                        Game_Manager.GameTransitions.LoadScene(scenePath, Transitions_Main.TransitionType.WipeSide);
                    }
                }
                foreach (GameObject missionMap in AssetBundleLoader.missionMaps)
                {
                    if (GUILayout.Button(missionMap.name))
                    {
                        Debug.Log("NEM: INSTANTIATING CAMERA");
                        GameObject camera = UnityEngine.Object.Instantiate(missionMap);
                        UI_MissionMap_Arena missionMapObject = camera.GetComponentInChildren<UI_MissionMap_Arena>();
                        if (missionMapObject == null)
                        {
                            Debug.Log("NEM: MISSION MAP OBJECT IS NULL.");
                            continue;
                        }

                        Debug.Log("NEM: INITIALISING CUSTOM MAP");
                        Traverse traverseMap = Traverse.Create(missionMapObject);
                        traverseMap.Method("Init").GetValue();
                    }
                }
            }
        }


        public static byte cameraType = 0;
        public enum CameraTypes : int
        {
            Vanilla,
            Free,
            Freeze,
            Fps
        }


        // Components that are destroyed and created when camera toggles type
        static Dictionary<string, UnityEngine.Object> camComponents = new Dictionary<string, UnityEngine.Object>();
        

        static void ClearCameraTypes()
        {
            foreach (string camComponent in camComponents.Keys)
            {
                UnityEngine.Object.Destroy(camComponents[camComponent]);
            }

            camComponents.Clear();
        }

        public static CameraTypes GetCameraType()
        {
            CameraTypes currentCameraType = (CameraTypes)cameraType;

            return currentCameraType;
        }

        /// <summary>
        /// UPDATE METHOD FOR KEYCODES AND STUFF
        /// </summary>
        void Update()
        {
            if (Game_Manager.currentManager == null || Game_Manager.currentManager.sceneIsMenu)
            {
                cameraType = 0;
                return;
            }

            ///
            ///             TELEPORT TO CURSOR KEYBIND
            ///

            // btw this doesnt work for hirelings
            if (Input.GetKeyDown(NEMConfig.Teleport.Value))
            {
                //ReturnSelectedSquadmates is a private method so we must traverse the forbidden lands
                Traverse playerController = Traverse.Create(Controller_Base.PlayerOne.myController as Controller_Player);

                List<Combatant_Base> hirelings = playerController.Field("squadmatesSelected").GetValue() as List<Combatant_Base>;

                if (hirelings is not null && hirelings.Count > 0)
                {
                    foreach (Combatant_Base hireling in hirelings)
                    {
                        if (hireling is null)
                        {
                            Debug.Log("Hireling is null!");
                            continue;
                        }
                        hireling.TeleportTo(Controller_Base.PlayerOne.aim_Final, Vector3.zero);
                        if (NEMConfig.TeleportEffectEnabled.Value == true)
                        {
                            hireling.TeleportPoof();
                        }
                    }
                }
                else
                {
                    if (Controller_Base.PlayerOne is null)
                    {
                        Debug.Log("Player One is null!");
                        return;
                    }
                    // Vector3.zeroVector randomly started throwing errors after workshop released soooooooo (field access exception, didnt have access from nemmenu.cs to vector3.zerovector for some reason, previously it worked just fine 😭😭😭😭😭😭)
                    Controller_Base.PlayerOne.TeleportTo(Controller_Base.PlayerOne.aim_Final, Vector3.zero);
                    // teleport effect
                    if (NEMConfig.TeleportEffectEnabled.Value == true)
                    {
                        Controller_Base.PlayerOne.TeleportPoof();
                    }
                }
            }

            ///
            ///             CAMERA TYPE TOGGLE
            ///
            if (Cam_Main.gameCam != null)
            {
                if (NEMConfig.CameraToggleEnabled.Value == true)
                {
                    if (Input.GetKeyDown(NEMConfig.CameraToggle.Value))
                    {
                        cameraType += 1;
                        CameraTypes camType = (CameraTypes)cameraType;

                        GameObject mpnCamera = Cam_Main.gameCam.gameObject;
                        Cam_Main mpnCamMain = mpnCamera.GetComponent<Cam_Main>();
                        mpnCamMain.enabled = false;

                        ClearCameraTypes();

                        switch (camType)
                        {

                            case CameraTypes.Free:
                                {
                                    camComponents.Add("FreeCam", mpnCamera.AddComponent<NEMFreeCam>());

                                    break;
                                }
                            case CameraTypes.Freeze:
                                {
                                    camComponents.Add("FreezeCam", mpnCamera.AddComponent<NEMFreezeCam>());

                                    break;
                                }
                            case CameraTypes.Fps:
                                {
                                    camComponents.Add("FpsCam", mpnCamera.AddComponent<NEMFpsCam>());

                                    break;
                                }

                            // default is the vanilla camera type and clears all other types
                            default:
                                {
                                    Cursor.lockState = CursorLockMode.Confined;
                                    mpnCamMain.enabled = true;

                                    cameraType = 0;
                                    camType = CameraTypes.Vanilla;
                                    break;
                                }
                        }

                        UI_Game.NewNotice("Chosen camera type: <color=yellow>" + GetCameraType().ToString() + "</color>");
                    }
                }
            }
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Fix for disabling god mode on scene transition
            // todo: make this apply to all squadlings through patches so you dont have to call this
            if (NEMMenu.toggleBools["GodMode"] == true)
            {
                Controller_Base.PlayerOne.MakeInvincible();
            }

            Debug.Log("NEM: Menu loaded scene \"" + scene.name + "\" with mode " + mode.ToString());

            // Fix for interactive mode menus not working outside of playground mode in vanilla
            InteractiveMode interactiveComponent = GameObject.FindObjectOfType<InteractiveMode>();
            if (interactiveComponent == null)
            {
                Debug.Log("NEM: InteractiveMode not found in scene \" " + scene.name + " \". Attaching InteractiveMode component to Game_Manager.currentManager.gameObject");

                Game_Manager.currentManager.gameObject.AddComponent<InteractiveMode>();
            }
        }
    }
}
