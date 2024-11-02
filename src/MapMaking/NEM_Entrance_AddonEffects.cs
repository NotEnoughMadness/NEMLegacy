using NotEnoughMadness.Classes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NotEnoughMadness.MapMaking
{
    public class NEM_Entrance_AddonEffects : NEM_Entrance_Peripheral
    {
        [Header("NEM_Entrance_AddonEffects")]
        public List<GameObject> colorByCharacter = new List<GameObject>();
        public Color defaultColor = Color.white;
        public List<GameObject> destroyObjects = new List<GameObject>();
        public List<GameObject> disableObjects = new List<GameObject>();
        public List<GameObject> enableObjects = new List<GameObject>();
        public List<GameObject> enableOnForever = new List<GameObject>();

        public NEM_Entrance_AddonEffects.NEM_FireEventsOn myEnableOn = NEM_Entrance_AddonEffects.NEM_FireEventsOn.SpawnUnit;

        [Tooltip("FMOD Event path")]
        public string oneShotSFX = "";

        public List<ParticleSystem> particles = new List<ParticleSystem>();

        public Vector3 physicsBlast = Vector3.forward * 5f;

        public List<Rigidbody> physicsObjects = new List<Rigidbody>();
        public float randomPhysicsVariance;

        public List<NEM_Entrance_AddonEffects.NEM_SpecialEffects> spawnEffects = new List<NEM_Entrance_AddonEffects.NEM_SpecialEffects>();

        [Serializable]
        public enum NEM_FireEventsOn
        {
            DoorOpen,
            SpawnUnit,
            AnimEvent,
            Locked,
            NEVER = 100
        }

        [Serializable]
        public enum NEM_SpecialEffects
        {
            Teleport
        }

        NEM_Entrance_AddonEffects()
        {
            MapManager.OnCreateMapComponents += OnCreateMapComponents;
            MapManager.OnConnectMapComponents += OnConnectMapComponents;
        }

        public void OnCreateMapComponents(object sender, EventArgs e)
        {
            Entrance_AddonEffects addon = gameObject.AddComponent<Entrance_AddonEffects>();

            addon.ColorByCharacter = colorByCharacter;
            addon.DefaultColor = defaultColor;
            addon.DestroyObjects = destroyObjects;
            addon.DisableObjects = disableObjects;
            addon.EnableObjects = enableObjects;
            addon.EnableOnForever = enableOnForever;

            addon.myEnableOn = (Entrance_AddonEffects.FireEventsOn)(int)myEnableOn;

            addon.OneShotSFX = oneShotSFX;
            addon.Particles = particles;

            addon.PhysicsBlast = physicsBlast;
            addon.PhysicsObjects = physicsObjects;
            addon.RandomPhysicsVariance = randomPhysicsVariance;

            List<Entrance_AddonEffects.SpecialEffects> newSpawnEffectsList = new List<Entrance_AddonEffects.SpecialEffects>();
            foreach(var effect in spawnEffects)
            {
                newSpawnEffectsList.Add((Entrance_AddonEffects.SpecialEffects)(int)effect);
            }

            addon.SpawnEffects = newSpawnEffectsList;
        }

        public void OnConnectMapComponents(object sender, EventArgs e)
        {
            Entrance_AddonEffects addon = gameObject.GetComponent<Entrance_AddonEffects>();

            addon.myEntrance = myEntrance.gameObject.GetComponent<Entrance_Base>();
        }


    }
}