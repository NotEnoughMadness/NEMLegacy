using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NotEnoughMadness
{
    public static class AssetBundleLoader
    {
        // For custom bundles
        public static List<AssetBundle> loadedBundles = new List<AssetBundle>();
        public static List<string> customScenePaths = new List<string>();

        public static List<GameObject> missionMaps = new List<GameObject>();
       
        public static void Cleanup()
        {
            AssetBundleLoader.customScenePaths.Clear();

            foreach (AssetBundle loadedBundle in AssetBundleLoader.loadedBundles)
            {
                if (loadedBundle == null)
                {
                    Debug.Log("NEM: Ermmm what in the madness. The loaded bundle is null?");
                    continue;
                }

                Debug.Log("NEM: Unloading bundle " + loadedBundle.name);
                loadedBundle.Unload(true);
            }

            AssetBundleLoader.loadedBundles.Clear();
        }
        
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
                        AssetBundleLoader.missionMaps.Add(missionMap);
                        Debug.Log("NEM: NEW LIST COUNT: " + AssetBundleLoader.missionMaps.Count);
                    }
                }

                return;
            }

            foreach (string scenePath in loadedBundle.GetAllScenePaths())
            {
                Debug.Log("NEM: Loaded scene: " + scenePath);
                AssetBundleLoader.customScenePaths.Add(scenePath);
            }

            Debug.Log("NEM: Loaded bundle: " + fileInfo.Name);
            AssetBundleLoader.loadedBundles.Add(loadedBundle);
        }
    }
}
