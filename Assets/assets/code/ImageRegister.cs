using System;
using System.Collections.Generic;
using UnityEngine;

namespace assets.code
{
    /// <summary>
    /// Registers Images of Items, so later items with the same name can be created.
    /// That means every items that will be created has to be in the scene one,
    /// and has to call the 'RegisterSprite' method
    /// </summary>
    public class ImageRegister
    {
        private static ImageRegister _instance = new();

        private Dictionary<string, Sprite> _dictionary = new();
        public static Sprite GetByItemName(string name)
        {
            if (!_instance._dictionary.ContainsKey(name.ToLower()))
            {
                Debug.LogError($"cant find sprite for item called {name}");
                return null;
            }

            var sprite = _instance._dictionary[name.ToLower()];
            return sprite;
        }

        public static void RegisterSprite(string name, Sprite sprite)
        {
            _instance._dictionary[name.ToLower()] = sprite;
        }
    }
}