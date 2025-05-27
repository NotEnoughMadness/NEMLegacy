using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
// for AssemblyLoadContext
//using System.Runtime.Loader;

namespace NotEnoughMadness
{
    public class NEMLoader
    {
        //AssemblyLoadContext

        static List<Assembly> loadedAssemblies = new List<Assembly>();

        public static void LoadPlugin(string assemblyPath)
        {
            Debug.Log("NEM: Loading assembly " + assemblyPath);
            Assembly newPlugin = null;
            try
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
