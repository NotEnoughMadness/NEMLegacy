using HarmonyLib;

namespace NotEnoughMadness.Patches
{
    /// <summary>
    /// Unlocks madness mode by default. Toggle in NEMConfig.cs
    /// </summary>
    ///
    // darksignaled
    /*
    [HarmonyPatch(typeof(UI_StageMenu), "CanPlayHighestDifficulty")]
    class UI_StageMenu_CanPlayHighestDifficulty
    {
        [HarmonyPrefix]
        static bool Prefix(ref bool __result)
        {
            if (NEMConfig.MadnessUnlocked.Value == true)
            {
                __result = true;
                return false; // dont execute original method
            }

            return true; // if madness unlocked false, execute original method instead
        }
    }*/
}

