using HarmonyLib;

namespace NotEnoughMadness.Patches
{
    [HarmonyPatch(typeof(InteractiveMode), "Awake")]
    public class InteractiveMode_Awake
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            // Here we just don't run the original code, which destroys the InteractiveMode component if the current stage's gamemode isn't Playground mode
            // That's good because we want to use interactive mode menus outside of interactive mode. That is, of course, unless you do not want that, in which case, womp womp 👽👽👽👽👽👽
            return false; 
        }
    }
}
