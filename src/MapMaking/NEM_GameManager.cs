using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NotEnoughMadness.MapMaking
{

    // https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
    // ^^ Something useful for making the unity inspector nicer for editing 
    // something to do down the line maybe


    // (note to s3l-f)
    // ramblings of a madman:
    /*
     * ---
     * Making NEM components *might* be unfeasible 👽
     * There's a lot of race conditions 
     * The components don't 'just work' and require lots of custom code and conversions
     * It seems to be very buggy despite the established order of execution in NEMMenu.cs onSceneLoaded()
     * 
     * ---
     * The first approach to custom map making was ok but very inconvenient (but ultimately it *WORKED*, unlike the others)
     * M:PN classes had to be manually stripped of most of their code with just the fields remaining and saved as .cs files inside of unity
     * Then NEM would swap out statcard references to the ingame ones at runtime
     * 
     * Automation of copying the MPN classes with a python script did not yield positive results 👽👽👽👽👽 (skill issue?)
     * 
     * ---
     * Hollowing out Assembly-CSharp.dll with a mono cecil program seemed to work fine when viewed through DnSpy but Unity had lots of trouble trying to load the dll
     *                                           (^^^ i had a few different approaches and attempts at that too)
     *
     * Like one time it loaded the dll but didn't recognize some classes as existing even though they did exist and were listed in the asset list 
     * but couldn't be attached to gameobjects because they "didn't exist" 👽👽👽👽👽👽👽
     * 
     * Unity also frequently froze and even crashed a few times when trying to load the dlls, throwing errors such as stack overflow, or missing RDA     
     * 
     * ---
     * The approach of combining NEM classes and copied M:PN classes did not work out because some components fired before NEM had the chance to swap them out and threw errors (Awake() methods and stuff)
     *                       ( ^^^ NEM classes would hold data and swap out some fields in M:PN components at runtime, both NEM classes and M:PN classes existed as components on gameobjects in unity)
     * 
     * ---
     * I think i will have to scrap the current approach and return to the first (in a different way)
     * Recode all of M:PN classes with their fields and empty methods marked as extern in this NEM dll 
     * Swap out statcard objects at runtime
     * 
     * This is quite a head scratcher 👽👽👽👽👽 an almost two years long venture 👽👽👽👽👽👽👽 im going crazy 👽👽👽👽👽👽👽👽👽👽👽👽👽 MAD, EVEN 👽👽👽👽👽👽👽👽👽👽👽👽👽👽👽👽👽👽👽👽👽👽👽👽👽👽
     * 
     * So many attempts
     * Yet we and as we began 👽👽👽👽 The journey is going full circle 👽👽👽
     */

    public class NEM_GameManager : MonoBehaviour
    {
        // // GAME MANAGER SETTINGS // //
        [Header("Game_Manager Settings")]
        
        [Tooltip("Multiplier for the character voicelines volumes, leave at 1 unless you know what's up.")]
        public float volumeBoost_Voices = 1f;

        [Tooltip("What sounds to disable in this scene. None is none, all is all, common is SOME, IDK, CURRENTLY I DONT KNOW RAARGH.")]
        public NEM_DisableSounds disableSounds = NEM_DisableSounds.None;

        [Tooltip("Disables the HUD (corpus, squadmates, weapons, etc, etc.")]
        public bool disableHUD = false;

        [Tooltip("Disables the pause menu, as in you can NOT pause in this scene.")]
        public bool disablePauseMenu = false;

        [Tooltip("If the scene is a menu, check true, and if not, check false.")]
        public bool sceneIsMenu = false;

        [Tooltip("Only enable this if your scene is a HUB WORLD. Like the nexus city sectors in Story Mode, or the Arena HQ. In playground mode, it is set to false.")]
        public bool saveSceneToCareerAsOverworld = false;

        // Another public field that exists is myModules, but I will not expose it here for now. I think it's not used anywhere in M:PN anyways??? (Didn't test didn't confirm but DnSpy brings up nothing in the analyzer)

        // those classes have to inherit from MonoBehaviour to appear properly
        [Header("Stage_Manager Settings")]
        // In mpn this is public but hidden in inspector so dont set it here
        //public NEM_StageCompletion currentStageCompletion = NEM_StageCompletion.None;

        [Tooltip("Default music for the stage, FMOD Event Path. Plays from the beginning to end (If custom event, make sure to make it loop in FMOD Studio itself)\r\n\r\nThis is overriden by the stage's statcard if you launch the scene through the stage menu")]
        public string defaultMusic = "";

        [Tooltip($"Game difficulty set for this stage locally if launched manually (not through the stage menu). \r\n\r\n0 = tourist, 1 = normal, 2 = tough, 3 = madness. \r\n\r\nWhy is this ENUM displayed as an INT in the unity editor you may ask? \r\nI DONT KNOW. ASK UNITY. THEY FIXED THIS BUG IN UNITY v2022. IT WAS FIRST REPORTED IN 2011. \r\n\r\nRAAAARGHHHHH!!!!!! \r\nhttps://discussions.unity.com/t/enum-drop-down-menu-in-inspector-for-nested-arrays/19915\r\nhttps://issuetracker.unity3d.com/issues/enum-field-is-shown-as-an-integer-field-when-it-is-serialized-in-a-nested-structure\r\nhttps://issuetracker.unity3d.com/issues/enum-fields-are-rendered-as-integer-fields-when-serialized-in-a-nested-structure")]
        public NEM_GameDifficulty localDifficulty = NEM_GameDifficulty.Normal;

        [Tooltip("Gamemode for when you launch this scene manually (no stage menu)")]
        public NEM_GameModes localGameModeTest = NEM_GameModes.Playground;

        [Tooltip("Stage statcard for when you launch this scene manually (no stage menu)")]
        public string localStage = "";

        [Tooltip("Replaces whatever stage  you were in before with the local stage of this scene if you enter this scene.")]
        public bool replaceCurrentStageWithLocal = false;

        [Tooltip("Uses the stage's local squad for the player (data assigners), replaces your previous squad if you had any.")]
        public bool useLocalSquad = false;

        [Tooltip("Removes all squadmates when you exit the stage.")]
        public bool wipePlayerSquadOnExit = false;


        // // MISSION MANAGER SETTINGS // //
        [Header("Mission_Manager Settings")]
        [Tooltip("Statcard names list for local missions")]
        public List<string> localMissions = new List<string>();

        //[Tooltip("PLACEHOLDER")]
        //public Dictionary<NEMConditionTypes, List<string>> missionsByCondition = new Dictionary<NEMConditionTypes, List<string>>();

        // leave this out for now, see if you even need it

        //[Tooltip("PLACEHOLDER")]
        //public Dictionary<NEMConditionTypes> objectivesByCondition

        [Tooltip("Statcard names list for test missions")]
        public List<string> testMissions = new List<string>();



        // EVENT MANAGER SETTINGS // //
        /*[Header("Event_Manager Settings")]

        [Tooltip("All conditions to check")]
        public List<NEM_ConditionToCheck> allConditionsToCheck = new List<NEM_ConditionToCheck>();

        [Tooltip("Event tickets for events")]
        public List<NEM_Event_Ticket> allEvents = new List<NEM_Event_Ticket>();

        [Tooltip("Events by condition")]
        public Dictionary<NEM_ConditionTypes, List<NEM_Event_Ticket>> eventsByCondition = new Dictionary<NEM_ConditionTypes, List<NEM_Event_Ticket>>();

        [Tooltip("Local event cards")]
        public List<string> localEventCards = new List<string>();

        [Tooltip("Local events")]
        public NEM_Event_Cluster localEvents = new NEM_Event_Cluster();

        [Tooltip("Serial names of world objects in your scene")]
        public List<string> objectsBySerialNumber = new List<string>();*/
        NEM_GameManager()
        {
            Debug.Log("NEM: NEM_GameManager constructor called");

            //MapManager.OnCreateMapComponents += CreateMapComponents;
            //MapManager.OnConnectMapComponents += ConnectMapComponents;
        }

        

        public void OnCreateMapComponents(object sender, EventArgs e)
        {
            Debug.Log("NEM: CreateMapComponents called on NEM_GameManager " + gameObject.name);
            gameObject.SetActive(false);
            
            // // CREATION OF ALL REQUIRED COMPONENTS // //

            // unity classes
            // Input Module

            StandaloneInputModule inputModule = gameObject.AddComponent<StandaloneInputModule>();

            inputModule.horizontalAxis = "Move Horizontal";
            inputModule.verticalAxis = "Move Vertical";
            inputModule.submitButton = "Menu Submit";
            inputModule.cancelButton = "Menu Cancel";
            inputModule.inputActionsPerSecond = 10;
            inputModule.repeatDelay = .5f;
            inputModule.forceModuleActive = false;

            // event system
            EventSystem eventSystem = gameObject.AddComponent<EventSystem>();

            eventSystem.firstSelectedGameObject = null;
            eventSystem.sendNavigationEvents = true;
            eventSystem.pixelDragThreshold = 5;






            // mpn classes
            // ui keystrokes
            // this one is fine with default values
            // iit just kinda sorta exists
            UI_Keystrokes uiKeystrokes = gameObject.AddComponent<UI_Keystrokes>();

            // code phase tracker, same here
            CodePhaseTracker codePhaseTracker = gameObject.AddComponent<CodePhaseTracker>();


            // Stage_Manager
            // before game manager, there has to exist a stage manager (game manager depends on it)
            Stage_Manager stageManager = gameObject.AddComponent<Stage_Manager>();

            stageManager.defaultMusic = defaultMusic;
            stageManager.localDifficulty = (GameDifficulty)(int)localDifficulty;
            stageManager.localGameModeTest = (GameModes)(int)localGameModeTest;
            // find stage and default to one if not found
            StatCard_Stage foundStage = StatCard_Base.ReturnCardByName<StatCard_Stage>(localStage);
            stageManager.localStage = (foundStage == null ? StatCard_Base.ReturnCardByName<StatCard_Stage>("Interactive") : foundStage);

            stageManager.ReplaceCurrentStageWithLocal = replaceCurrentStageWithLocal;
            stageManager.UseLocalSquad = useLocalSquad;
            stageManager.WipePlayerSquadOnExit = wipePlayerSquadOnExit;


            // Mission_Manager
            Mission_Manager missionManager = gameObject.AddComponent<Mission_Manager>();

            // populate local missions
            missionManager.LocalMissions = new List<StatCard_Mission>();
            foreach(string localMissionName in localMissions)
            {
                StatCard_Mission foundMission = StatCard_Base.ReturnCardByName<StatCard_Mission>(localMissionName);
                if (foundMission == null)
                {
                    continue;
                }

                missionManager.LocalMissions.Add(foundMission);
            }

            missionManager.MissionsByCondition = new Dictionary<ConditionTypes, List<Mission_Instance>>();

            missionManager.testMissions = new List<StatCard_Mission>();
            foreach(string testMissionName in testMissions)
            {
                StatCard_Mission foundMission = StatCard_Base.ReturnCardByName<StatCard_Mission>(testMissionName);
                if (foundMission == null)
                {
                    continue;
                }

                missionManager.testMissions.Add(foundMission);
            }



            // Event_Manager

            Event_Manager eventManager = gameObject.AddComponent<Event_Manager>();

            // Arena_Manager
            Arena_Manager arenaManager = gameObject.AddComponent<Arena_Manager>();

            // Wave_Manager
            Wave_Manager waveManager = gameObject.AddComponent<Wave_Manager>();

            // Decay_Manager
            Decay_Manager decayManager = gameObject.AddComponent<Decay_Manager>();

            // Game_Manager
            //the titular <game manager > itself 😱😱😱

            Game_Manager gameManager = gameObject.AddComponent<Game_Manager>();

            gameManager.VolBoost_Voices = volumeBoost_Voices;
            gameManager.disableSounds = (Game_Manager.DisableSounds) (int)disableSounds; // cast to vanilla enum by int
            gameManager.disableHUD = disableHUD;
            gameManager.disablePauseMenu = disablePauseMenu;
            gameManager.sceneIsMenu = sceneIsMenu;
            gameManager.SaveSceneToCareerAsOverworld = saveSceneToCareerAsOverworld;
            //gameManager.myModules NUH UH 


            // this just has to exist
            StudioListener studioListener = gameObject.AddComponent<StudioListener>();
            studioListener.ListenerNumber = 0; // 0 is default vanilla number


            Dev_Control devControl = gameObject.AddComponent<Dev_Control>();

            Dev_Settings Settings = gameObject.AddComponent<Dev_Settings>();


            // // COMMENCE THE SELF DESTRUCT SEQUENCE // //

            // "this" is the NEM_GameManager 
            //Destroy(this);
            
        }

        public void OnConnectMapComponents(object sender, EventArgs e)
        {
            Debug.Log("NEM: ConnectMapComponents called on NEM_GameManager " + gameObject.name);

            gameObject.SetActive(true);
        }
    }
}
