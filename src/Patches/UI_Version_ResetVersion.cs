using HarmonyLib;

namespace NotEnoughMadness.Patches
{
    // perhaps not needed for now
    /*
    [HarmonyPatch(typeof(UI_Version), "ResetVersion")]
    class UI_Version_ResetVersion
    {
        [HarmonyPostfix]
        static void Postfix()
        {
            if (UI_Version.currentWidget == null)
            {
                return;
            }
            UI_Version.currentWidget.Text_Version.text = NEMConfig.Version;
        }
    }
    */
}

