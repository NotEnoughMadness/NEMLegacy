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

        static List<Assembly> loadedAssemblies = new List<Assembly>();

        public static void Cleanup()
        {
            // handle cleanup logic here idk (clearing the list doesnt unload)
        }

        public static void LoadPlugin(string assemblyPath)
        {
            Debug.Log("NEM: Loading assembly " + assemblyPath);
            Assembly newPlugin = null;
            try
#pragma warning disable CS0168 // Variable is declared but never used
            {
                newPlugin = Assembly.Load(assemblyPath);
            } catch(FileNotFoundException ex)
            {
                Debug.Log("NEM: Assembly " + assemblyPath + " not found.");
                return;
            } catch(Exception ex)
            {
                Debug.Log("NEM: Error loading assembly " + assemblyPath);
                Debug.Log(ex);
            }
#pragma warning restore CS0168 // Variable is declared but never used
            

            if (newPlugin == null)
            {
                return;
            }

            loadedAssemblies.Add(newPlugin);

            // To do here:
            // look for a bepinex main class there and execute the method

            //pluginDomain.Load(assemblyPath);
        }
    }
}
