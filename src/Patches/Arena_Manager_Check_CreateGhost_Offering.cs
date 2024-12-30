using HarmonyLib;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace NotEnoughMadness.Patches
{
    //[HarmonyPatch(typeof(Arena_Manager), MethodType.Constructor)]
    //class Arena_Manager_Constructor_Patch
    //{
    //    [HarmonyPostfix]
    //    static void Postfix(Arena_Manager __instance)
    //    {
    //        if (NEMConfig.CreateGhostOfferingPatch.Value == false)
    //        {
    //            return;
    //        }

    //        Traverse managerTraverse = Traverse.Create(__instance);

    //        managerTraverse.Field("SummonGhostText").SetValue(new string[] { 
    //            "CUSTOM TEXT WOOOOO",
    //            "I am a customs guy",
    //            "I RISE FROM THE DEEAAAD",
    //            "GRAARGH BRAINSS"
    //        });
    //    }
    //}

    [HarmonyPatch(typeof(Arena_Manager), "Check_CreateGhost_Offering")]
    class Arena_Manager_Check_CreateGhost_Offering
    {
        [HarmonyPrefix]
        static bool Prefix(ref Combatant_Base inVictim)
        {
            if (NEMConfig.CreateGhostOfferingPatch.Value == false)
            {
                return true;
            }

            Traverse currentManagerTraverse = Traverse.Create(Arena_Manager.currentManager);

            Squad_Main allySquad = currentManagerTraverse.Method("ReturnAllySquad").GetValue() as Squad_Main;
            Combatant_Base combatant_Base = Arena_Manager.CreateGhost(inVictim, allySquad, true, (currentManagerTraverse.Field("SummonGhostText").GetValue() as string[]).RandomElement<string>());
            currentManagerTraverse.Field("playerAllySquad").SetValue(allySquad);
            Squad_Main.MakePlayerAllySquad(currentManagerTraverse.Field("playerAllySquad").GetValue() as Squad_Main);


            List<Combatant_Base> bribedDudes = currentManagerTraverse.Field("BribedDudes").GetValue() as List<Combatant_Base>;
            bribedDudes.Add(combatant_Base);
            currentManagerTraverse.Field("BribedDudes").SetValue(bribedDudes);

            Buffs_Manager.AddBuff(combatant_Base, Buffs_Manager.BuffEffects.Bribed, -1f);

            return false;
        }
    }
}

