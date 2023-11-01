#nullable enable
using System;
using System.Collections.Generic;
using assets.recipes;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace assets.code
{
    public class TheCauldron : MonoBehaviour
    {
        [SerializeField] private GameObject? dropParticleSystem;
        [SerializeField] private GameObject? idleParticleSystem;
        [SerializeField] private GameObject? itemPrefab;
        [SerializeField] public int level = 0;

        private List<string> _currentItems = new();

        private void Start()
        {
            States.AddCauldron(this);
        }

        private void Update()
        {
            if (idleParticleSystem != null)
            {
                idleParticleSystem.SetActive(_currentItems.Count != 0);
            }
        }

        public void Add(Item item)
        {
            var random = new Random();

            _currentItems.Add(item.itemName);
            var findRecipeResult = FindRecipeResult(_currentItems);

            if (findRecipeResult != null)
            {
                _currentItems.Clear();
                foreach (var _ in findRecipeResult.Ingredients)
                {
                    var color = Color.HSVToRGB((float)random.NextDouble(), 0.8f, 0.8f);
                    SpawnParticle(color);
                }

                NewItem(findRecipeResult.Result);
            }

            SpawnParticle(Color.HSVToRGB((float)random.NextDouble(), 0.8f, 0.8f));
            // else 
        }

        private void SpawnParticle(Color color)
        {
            if (dropParticleSystem != null)
            {
                var obGameObject = Instantiate(dropParticleSystem, this.transform);
                obGameObject.transform.localPosition = new Vector3();
                var system = obGameObject.GetComponent<ParticleSystem>();

                var systemMain = system.main;
                systemMain.startColor = color;
                Destroy(obGameObject, 5);
            }
        }


        private Recipe? FindRecipeResult(List<string> items)
        {
            //can be done with IsSubSet method i think
            foreach (var recipe in RecipeRegister.AllRecipes)
            {
                var hashSet = new HashSet<string>(recipe.Ingredients);
                foreach (var item in items)
                {
                    hashSet.Remove(item);
                }

                if (hashSet.Count == 0)
                {
                    return recipe;
                }
            }

            return null;
        }


        public void NewItem(string name)
        {
            if (itemPrefab != null)
            {
                var obj = Instantiate(itemPrefab);
                obj.transform.position = this.transform.position;
                var component = obj.GetComponent<Item>();
                component.itemName = name;
                component.UpdateImage();
            }
        }
    }
}