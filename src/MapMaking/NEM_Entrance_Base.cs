using System;
using System.Collections.Generic;
using UnityEngine;

namespace NotEnoughMadness.MapMaking
{
    public class NEM_Entrance_Base : NEM_Person_Repository
    {
        // CharacterQueue is set automatically methinks
        // entrancePathRecords
        [Header("NEM_Entrance_Base")]
        public bool disableSpawn = false;
        public bool snapToFloor = true;
        public bool spawnWhenClosed = true;
        public bool exitWhenClosed = false;
        public bool forcePathLockOnExit = false;
        public bool haveGrabber = false;
        public bool ignoreStoryModeLockdown = false;
        public bool noClosingOrOpening = false;
        public bool noReturn = false;

        [Tooltip("The scene that entering this door will transport you to.")]
        public string linkedScene = "";

        [Tooltip("Huh?!")]
        public string linkedScene_Entry = "";


        [Tooltip("Time before door self-closes (-1 means never)")]
        public float doorOpenTime = 30f;

        [Tooltip("Time to wait after being given a guy (when I was empty) before spawning him. Gives time for others to spawn before it opens (ex: Lifts)")]
        public float startupWaitTime = -1f;

        public bool clearStartupAfterFirst = false;

        [Tooltip("FMOD Event path")]
        public string closeSound = "";

        [Tooltip("FMOD Event path")]
        public string openSound = "";

        public float useSpeedMult = 2f;

        [Tooltip("Time between character spawns, how much time before the next one can spawn.")]
        public float spawnBuffer;

        public bool disableLightIfNoLink = true;

        public List<Transform> exitPoints = new List<Transform>();
        public List<Transform> spawnPoints = new List<Transform>();

        public List<NEM_Entrance_Base> mutuallyExclusiveDoors = new List<NEM_Entrance_Base>();

        public NEM_Entrance_AddonEffects addonEffects;

        NEM_Entrance_Base.NEM_DoorStatus myDoorStatus = NEM_Entrance_Base.NEM_DoorStatus.Closed;

        [Tooltip("What other door does this door go to?")]
        public NEM_Entrance_Base myLinkedEntrance;


        List<Animator> accessLights = new List<Animator>();

        public Dictionary<NEM_Entrance_Base, NEM_Entrance_Base.NEM_PathDeets> entrancePathRecords = new Dictionary<NEM_Entrance_Base, NEM_Entrance_Base.NEM_PathDeets>();

        public NEM_Entrance_Base.NEM_ExitSpeed userExitSpeed = NEM_Entrance_Base.NEM_ExitSpeed.Walk;

        // probably dont need
        // Combatant_Base inboundCharacters;
        // outboundCharacters
        // characterqueue
        // transferringcharacters

        // myentranceconnections
        // myentrancekeys
        // ^^ is this just a list of keys for the connections dict??
        // needs investigation

        // my seen entrances
        // my seen entrances distance log


        


        NEM_Entrance_Base()
        {
            Debug.Log("NEM: NEM_Entrance_Base constructor called");
            

            //MapManager.OnCreateMapComponents += OnCreateMapComponents;
            //MapManager.OnConnectMapComponents += OnConnectMapComponents;
        }

        public void OnCreateMapComponents(object sender, EventArgs e)
        {
            Debug.Log("NEM: NEM_Entrance_Base.CreateMapComponents called");
            gameObject.SetActive(false);

            Entrance_Base entrance = gameObject.AddComponent<Entrance_Base>();

            entrance.PressEnterText = pressEnterText;
            entrance.PressEnterRange = pressEnterRange;

            entrance.amLocked = amLocked;
            entrance.UsersIntangibleOnEnter = usersIntangibleOnEnter;
            entrance.UsersInvincible = usersInvincible;
            entrance.UsersUntargetable = usersUntargetable;

            entrance.VocationProhibited = (VocationList)(int)vocationProhibited;
            entrance.VocationRequirement = (VocationList)(int)vocationRequirement;

            entrance.EnterPoint = enterPoint;
            entrance.myUIPoint = uiPoint;

            entrance.EnterSound = enterSound;
            entrance.LeaveSound = leaveSound;
            entrance.LockSound = lockSound;

            // enter, exit, idle animations

            entrance.AccessLights = accessLights;

            // CharacterQueue (dont need to add i think)

            entrance.ClearStartupWaitAfterFirst = clearStartupAfterFirst;
            entrance.CloseSound = closeSound;
            entrance.DisableLightIfNoLink = disableLightIfNoLink;
            entrance.disableSpawn = disableSpawn;
            entrance.doorOpenTime = doorOpenTime;

            entrance.myDoorStatus = (Entrance_Base.DoorStatus)(int)myDoorStatus;

            entrance.noClosingOrOpening = noClosingOrOpening;
            entrance.noReturn = noReturn;

            entrance.startupWaitTime = startupWaitTime;

            entrance.userExitSpeed = (Entrance_Base.ExitSpeed)(int)userExitSpeed;

            entrance.useSpeedMult = useSpeedMult;

            entrance.OutboundCharacters = new List<Combatant_Base>();

            entrance.ExitPoints = exitPoints;
            entrance.SpawnPoints = spawnPoints;

            //todo check for stuff you missed and didnt set here
        }
        public void OnConnectMapComponents(object sender, EventArgs e)
        {
            // entrance path records
            // the entrancebase s already need to be created

            Debug.Log("NEM: NEM_Entrance_Base.ConnectMapComponents called");
            Entrance_Base entrance = gameObject.GetComponent<Entrance_Base>();

            if (entrance == null)
            {
                Debug.LogError("NEM: Entrance_Base in NEM_Entrance_Base.ConnectMapComponents is null!");
                return;
            }

            Dictionary<Entrance_Base, Entrance_Base.PathDeets> newPathRecords = new Dictionary<Entrance_Base, Entrance_Base.PathDeets>();

            foreach(NEM_Entrance_Base key in entrancePathRecords.Keys)
            {
                Entrance_Base newEntrance = key.gameObject.GetComponent<Entrance_Base>();
                if (newEntrance == null) continue;

                Entrance_Base ogEntrance = entrancePathRecords[key].myOriginalEntrance.gameObject.GetComponent<Entrance_Base>();
                if (ogEntrance == null) continue;

                Entrance_Base.PathDeets newDeet = new Entrance_Base.PathDeets();
                newDeet.distanceValue = entrancePathRecords[key].distanceValue;
                newDeet.myOriginalEntrance = ogEntrance;

                newPathRecords.Add(newEntrance, newDeet);
            }

            entrance.entrancePathRecords = newPathRecords;

            Entrance_Base foundEntrance = myLinkedEntrance.gameObject.GetComponent<Entrance_Base>();
            if (foundEntrance == null)
            {
                Debug.LogError("NEM: NEM_Entrance_Base foundEntrance is null");
                return;
            }

            entrance.myLinkedEntrance = foundEntrance;

            gameObject.SetActive(true);
        }


        [Serializable]
        public enum NEM_DoorStatus
        {
            Open,
            Closed,
            Opening,
            Closing
        }

        [Serializable]
        public enum NEM_ExitSpeed
        {
            Walk,
            Run,
            Sprint
        }

        [Serializable]
        public struct NEM_PathDeets
        {
            public float distanceValue;
            public NEM_Entrance_Base myOriginalEntrance;
        }
    }
}
