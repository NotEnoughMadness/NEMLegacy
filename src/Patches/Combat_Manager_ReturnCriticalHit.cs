using HarmonyLib;

namespace NotEnoughMadness.Patches
{
    [HarmonyPatch(typeof(Combat_Manager), "ReturnCriticalHit")]
    public class Combat_Manager_ReturnCriticalHit
    {
        [HarmonyPostfix]
        static void Postfix(ref bool __result)
        {
            if (NEMMenu.toggleBools["CriticalHits"] == true)
            {
                __result = true;
            }
        }

    }
}
