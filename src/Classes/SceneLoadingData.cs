using System.Collections.Generic;
using UnityEngine;

namespace NotEnoughMadness
{
    public class SceneData
    {
        public List<string> additiveScenePaths = new List<string>();
        public Dictionary<string, GameObjectModification> modifications = new Dictionary<string, GameObjectModification>();
    }

    public class GameObjectModification
    {
        public string tag;

        public int? layer;

        public Vector3 position;

        public Dictionary<string, Dictionary<string, object>> components = new Dictionary<string, Dictionary<string, object>>();
    }
}
