using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace assets.code
{
    /// <summary>
    ///     Registers Images of Items, so later items with the same name can be created.
    ///     That means every items that will be created has to be in the scene one,
    ///     and has to call the 'RegisterSprite' method
    /// </summary>
    public class ImageRegister
    {
        private static readonly ImageRegister _instance = new();

        private Dictionary<string, Sprite> _dictionary = new();
        private readonly Dictionary<string, GameObject> _GameObjectDictionary = new();

        public static GameObject GetGameObjectByItemName(string name)
        {
            if (!_instance._GameObjectDictionary.ContainsKey(name.ToLower()))
            {
                Debug.LogError($"cant find gameObject for item called {name}");
                return null;
            }

            var obj = _instance._GameObjectDictionary[name.ToLower()];
            return obj;
        }

        public static void RegisterGameObject(string name, GameObject obj)
        {
            obj = Object.Instantiate(obj);
            obj.SetActive(false);
            _instance._GameObjectDictionary[name.ToLower()] = obj;
        }
    }
}