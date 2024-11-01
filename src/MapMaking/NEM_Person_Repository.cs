using NotEnoughMadness.Classes;
using System;
using UnityEngine;

namespace NotEnoughMadness.MapMaking
{
    public class NEM_Person_Repository : NEM_UseableObject
    {
        [Header("NEM_Person_Repository")]
        [Tooltip("Text that pops up when you try to enter.")]
        public string pressEnterText;

        [Tooltip("Range that you can try to enter from.")]
        public float pressEnterRange;

        [Tooltip("If locked you can't enter.")]
        public bool amLocked = false;

        public bool usersIntangibleOnEnter = false;

        public bool usersInvincible = false;

        public bool usersUntargetable = false;

        [Tooltip("If user has this vocation, he will be unable to enter.")]
        public NEM_VocationList vocationProhibited = NEM_VocationList.NONE;

        [Tooltip("If user does NOT have this vocation, he will be unable to enter.")]
        public NEM_VocationList vocationRequirement = NEM_VocationList.NONE;

        [Tooltip("Drag any gameobject in here, it will use its\' location as the enter point.")]
        public Transform enterPoint;

        public Transform uiPoint;

        [Header("Sounds")]
        [Tooltip("Fmod event path to play when someone enters.")]
        public string enterSound;

        [Tooltip("Fmod event path to play when someone leaves.")]
        public string leaveSound;

        [Tooltip("Fmod event path to play when amLocked is true.")]
        public string lockSound;

        [Header("Animations")]
        public string enterAnimation;
        public string exitAnimation;
        public string[] exitAnimations;
        public string idleAnimation;
        public string[] idleAnimations;
    }
}