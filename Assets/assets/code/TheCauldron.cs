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
    /// <summary>
    /// Main cauldron code.
    /// The Player can add Ingredients by calling 'Add'.
    /// The Script then checks for valid recipes
    /// </summary>
    public class TheCauldron : MonoBehaviour
    {
        [SerializeField] private GameObject? dropParticleSystem;
        [SerializeField] private GameObject? idleParticleSystem;
        [SerializeField] private GameObject? itemPrefab;
        [SerializeField] public int level = 0;

        private SpriteRenderer _spriteRenderer;

        private List<string> _currentItems = new();

        private void Start()
        {
            States.AddCauldron(this);
            _spriteRenderer = GetComponent<SpriteRenderer>();

        }

        /// <summary>
        /// activates the 'idleParticleSystem' if an item is inside to cauldron
        /// </summary>
        private void Update()
        {
            if (idleParticleSystem != null)
            {
                idleParticleSystem.SetActive(_currentItems.Count != 0);
            }

            // The cauldron will always be in the background
            this._spriteRenderer.sortingOrder = 5;
        }

        /// <summary>
        /// Adds an item to the cauldron.
        /// If an item is found the Cauldron will spit it out and play particle effects
        /// </summary>
        /// <param name="item">Item to add</param>
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

        /// <summary>
        /// Creates a Particle Effect 
        /// </summary>
        /// <param name="color">The Color of the Particles</param>
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

        /// <summary>
        /// Searches for a valid recipe 
        /// </summary>
        /// <param name="items">items to test</param>
        /// <returns>a potential Recipe</returns>
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


        /// <summary>
        /// Creates a new Item the the specified name.
        /// Also updates the Image
        /// </summary>
        /// <param name="name">name and id of the item</param>
        private void NewItem(string name)
        {
            if (itemPrefab != null)
            {
                var obj = Instantiate(itemPrefab);
                obj.transform.position = this.transform.position + new Vector3(0, 1, 0);
                var component = obj.GetComponent<Item>();
                component.itemName = name;
                component.UpdateImage();

                // New Items will fly above the cauldron when they spawn
                var componentRigidbody = component.GetComponent<Rigidbody2D>();
                componentRigidbody.bodyType = RigidbodyType2D.Kinematic;

            }
        }
    
    }
    
}