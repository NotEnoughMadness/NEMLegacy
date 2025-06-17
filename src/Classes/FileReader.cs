using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NotEnoughMadness
{
    public static class FileReader
    {
        // For custom bundles
        public static List<AssetBundle> loadedBundles = new List<AssetBundle>();
        public static List<string> customScenePaths = new List<string>();

        public static List<GameObject> missionMaps = new List<GameObject>();
       
        
        public static void ProcessModBundle(FileInfo fileInfo)
        {
            AssetBundle loadedBundle = AssetBundle.LoadFromFile(fileInfo.FullName);
            if (loadedBundle == null)
            {
                Debug.Log("NEM: Loaded bundle is null, skipping bundle: " + fileInfo.Name);
                return;
            }

            if (!loadedBundle.isStreamedSceneAssetBundle)
            {
                foreach (GameObject missionMap in loadedBundle.LoadAllAssets<GameObject>())
                {
                    // if the camera has a child with mission map, it is a MISSION MAP 😱😱😱😱
                    if (missionMap.GetComponentInChildren<UI_MissionMap_Arena>() != null)
                    {
                        Debug.Log("NEM: ADDED UI_MISSIONMAP_ARENA TO LIST.");
                        FileReader.missionMaps.Add(missionMap);
                        Debug.Log("NEM: NEW LIST COUNT: " + FileReader.missionMaps.Count);
                    }
                }

                return;
            }

            foreach (string scenePath in loadedBundle.GetAllScenePaths())
            {
                Debug.Log("NEM: Loaded scene: " + scenePath);
                FileReader.customScenePaths.Add(scenePath);
            }

            Debug.Log("NEM: Loaded bundle: " + fileInfo.Name);
            FileReader.loadedBundles.Add(loadedBundle);
        }


        public static Dictionary<string, SceneData> sceneData = new Dictionary<string, SceneData>();

        /// <summary>
        /// Loads scene data from json, for additively loading scenes (paths) and modifying at runtime.
        /// </summary>
        /// <param name="jsonPath"></param>
        public static void LoadSceneData(string jsonPath)
        {

        }
    }
}
