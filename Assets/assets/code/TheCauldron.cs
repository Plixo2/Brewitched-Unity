#nullable enable
using System.Collections.Generic;
using assets.recipes;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;


namespace assets.code
{
    /// <summary>
    ///     Main cauldron code.
    ///     The Player can add Ingredients by calling 'Add'.
    ///     The Script then checks for valid recipes
    /// </summary>
    public class TheCauldron : Interactable
    {
        [SerializeField] private GameObject? dropParticleSystem;
        [SerializeField] private GameObject? idleParticleSystem;
        [SerializeField] private GameObject? itemPrefab;
        [SerializeField] public Image progressBarImage;
        [SerializeField] public Image fillImage;
        [SerializeField] private float brewingTime = 5f;
        [SerializeField] private AudioSource? finishBrewing;

        private float _brewingTimeLeft = -1;

        private List<string> _currentItems;
        private Item _item;
        private string? _itemBrewing;

        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            base.Start();
            _currentItems = new List<string>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _item = GetComponent<Item>();
            if (_item == null) Debug.LogError("The Cauldron needs an Item script");
        }

        /// <summary>
        ///     activates the 'idleParticleSystem' if an item is inside to cauldron
        /// </summary>
        private void Update()
        {
            var brewing = isBrewing();

            if (_item._connectionPoint == null || !_item._connectionPoint.isFireplace) brewing = false;

            if (brewing)
            {
                var fillImageScale = 1 - _brewingTimeLeft / brewingTime;
                progressBarImage.enabled = true;
                fillImage.enabled = true;
                fillImage.rectTransform.transform.localScale = new Vector3(fillImageScale, 1, 1);

                _brewingTimeLeft -= Time.deltaTime;
                if (!isBrewing() && _itemBrewing != null)
                {
                    finishBrewing.Play();
                    NewItem(_itemBrewing);
                    _itemBrewing = null;
                    progressBarImage.enabled = false;
                    fillImage.enabled = false;
                }
            }
            else
            {
                var inFire = _item._connectionPoint != null && _item._connectionPoint.isFireplace;
                if (_currentItems.Count > 0 && inFire)
                {
                    var findRecipeResult = FindRecipeResult(_currentItems);

                    if (findRecipeResult != null)
                    {
                        var random = new Random();
                        var copy = new List<string>(_currentItems);
                        foreach (var s in copy)
                            if (findRecipeResult.Ingredients.Contains(s))
                            {
                                _currentItems.Remove(s);
                                var color = Color.HSVToRGB((float)random.NextDouble(), 1f, 1f);
                                SpawnParticle(color);
                            }

                        _brewingTimeLeft = brewingTime;
                        _itemBrewing = findRecipeResult.Result;
                        DropAll();
                    }
                }
            }

            if (idleParticleSystem != null) idleParticleSystem.SetActive(brewing);

            // The cauldron will always be in the background
            // this._spriteRenderer.sortingOrder = 5;
        }

        private void DropAll()
        {
            foreach (var currentItem in _currentItems) NewItem(currentItem);
            _currentItems.Clear();
        }

        public override bool Interact(Item? item)
        {
            if (item != null)
            {
                if (item.isCauldron()) return false;
                return Add(item);
            }

            return false;
        }

        /// <summary>
        ///     Adds an item to the cauldron.
        ///     If an item is found the Cauldron will spit it out and play particle effects
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns>if the item was added</returns>
        private bool Add(Item item)
        {
            if (isBrewing()) return false;

            var random = new Random();
            _currentItems.Add(item.itemName);

            SpawnParticle(Color.HSVToRGB((float)random.NextDouble(), 1f, 1f));


            return true;
        }

        /// <summary>
        ///     Creates a Particle Effect
        /// </summary>
        /// <param name="color">The Color of the Particles</param>
        private void SpawnParticle(Color color)
        {
            if (dropParticleSystem != null)
            {
                var obGameObject = Instantiate(dropParticleSystem, transform);
                obGameObject.transform.localPosition = new Vector3();
                var system = obGameObject.GetComponent<ParticleSystem>();

                var systemMain = system.main;
                systemMain.startColor = color;
                Destroy(obGameObject, 5);
            }
        }

        /// <summary>
        ///     Searches for a valid recipe
        /// </summary>
        /// <param name="items">items to test</param>
        /// <returns>a potential Recipe</returns>
        private Recipe? FindRecipeResult(List<string> items)
        {
            //can be done with IsSubSet method i think
            foreach (var recipe in RecipeRegister.AllRecipes)
            {
                var hashSet = new HashSet<string>(recipe.Ingredients);
                foreach (var item in items) hashSet.Remove(item);

                if (hashSet.Count == 0) return recipe;
            }

            return null;
        }


        /// <summary>
        ///     Creates a new Item the the specified name.
        ///     Also updates the Image
        /// </summary>
        /// <param name="name">name and id of the item</param>
        private void NewItem(string name)
        {
            if (ImageRegister.GetGameObjectByItemName(name) != null)
            {
                var obj = Instantiate(ImageRegister.GetGameObjectByItemName(name));
                obj.SetActive(true);
                obj.transform.position = transform.position + new Vector3(0, 1, 0);
                obj.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        public bool isBrewing()
        {
            return _brewingTimeLeft > 0;
        }
    }
}