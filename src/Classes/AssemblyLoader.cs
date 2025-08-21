using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
// for AssemblyLoadContext
//using System.Runtime.Loader;

namespace NotEnoughMadness
{
    public class AssemblyLoader
    {
        //AssemblyLoadContext

        // Change this later to appdomain so you can unload assembly mods
        static List<Assembly> loadedAssemblies = new List<Assembly>();

        public static void Cleanup()
        {
            // handle cleanup logic here idk
            // unload assemblies
        }

        public static void LoadPlugin(string assemblyPath)
        {
            Debug.Log("NEM: Loading assembly " + assemblyPath);
            Assembly newPlugin = null;
            try
#pragma warning disable CS0168 // Variable is declared but never used
            {
                newPlugin = Assembly.Load(assemblyPath);

                // STEPS TO DO HERE:
                // assembly loaded in memory ✅
                // load it to a domain or loadcontext or twhatever to make unloading easier when reapplying mods!!!
                // try to execute main method with bepinplugin attribute
                // do that using a traverse as the awake method may be private
                // the mod handles itself in awake and stuff like a normal bepinex plugin BUT
                // BUT when unloading, manually unpatch everything from that assembly in case the mod doesnt do that
                // dont want residual when disabling mods ingame
            } catch(FileNotFoundException ex)
            {
                Debug.Log("NEM: Assembly not found at:\n " + assemblyPath);
                return;
            } catch(Exception ex)
            {
                Debug.Log("NEM: Error loading assembly at:\n " + assemblyPath);
                Debug.Log(ex);
            }
#pragma warning restore CS0168 // Variable is declared but never used
            

            if (newPlugin == null)
            {
                Debug.Log("NEM: Plugin assembly is null at:\n " + assemblyPath);
                return;
            }

            loadedAssemblies.Add(newPlugin);

            // To do here:
            // look for a bepinex main class there and execute the method

            //pluginDomain.Load(assemblyPath);
        }
    }
}
