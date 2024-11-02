using System;
using UnityEngine;

namespace NotEnoughMadness.MapMaking
{
    [Serializable]
    public class NEM_UniformWearable
    {
        [Tooltip("List of StatCard_Armor names")]
        public string[] armorList = new string[0];

        [Tooltip("0 is the default material of the armor")]
        public int altMaterialIndex = 0;

        [Tooltip("0 is the default mesh of the armor")]
        public int altMeshIndex = 0;

        [Tooltip("Primary swatch color")]
        public Color tint1Color = Color.black;

        [Tooltip("Secondary swatch color")]
        public Color tint2Color = Color.black;

        [Tooltip("Tertiary/Glow/Glass swatch color")]
        public Color glassColor = Color.black;
    }
}