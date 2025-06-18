using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NotEnoughMadness
{
    public class SceneModLoader
    {
        public static Dictionary<string, SceneData> sceneData = new Dictionary<string, SceneData>();

        public static void Cleanup()
        {
            sceneData.Clear();
        }

        /// <summary>
        /// Loads scene data from json, for additively loading scenes (paths) and modifying at runtime.
        /// </summary>
        /// <param name="jsonPath"></param>
        public static void LoadSceneData(string jsonPath)
        {
            Debug.Log("NEM: NEMSceneData found in json path: " + jsonPath);

            
        }
    }

    public class SceneData
    {
        public List<string> additiveScenePaths = new List<string>();
        public Dictionary<string, GameObjectModification> modifications = new Dictionary<string, GameObjectModification>();
    }

    public class GameObjectModification
    {
        public string tag;

        public int? layer;

        public Vector3? position;

        public Dictionary<string, Dictionary<string, object>> components = new Dictionary<string, Dictionary<string, object>>();
    }
}
