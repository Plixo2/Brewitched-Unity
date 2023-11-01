using System;
using System.Collections.Generic;
using UnityEngine;

namespace assets.code
{
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