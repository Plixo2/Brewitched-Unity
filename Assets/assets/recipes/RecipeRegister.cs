using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace assets.recipes
{
    /// <summary>
    /// Register for adding recipes with a markup 
    /// </summary>
    public class RecipeRegister : MonoBehaviour
    {
        public static List<Recipe> AllRecipes = new();
        static RecipeRegister()
        {
            // Add("ash, bottle, phoenix_feather -> potion_fire_resistant");   
            Add("water, feather -> double_Jump_Potion");   
            Add("water, apple -> potion_fire");   
            Add("potion_fire, scroll -> key");   
        }

        /// <summary>
        /// Converts and adds a markup string to the recipe list
        /// </summary>
        /// <param name="markup">markup recipe</param>
        private static void Add(string markup)
        {
            var separator = markup.Split("->");
            var ingredients = separator[0];
            var result = separator[1];
            var resultName = ToIngredientName(result);

            var ingredientNameList = new List<string>();
            foreach (var name in ingredients.Split(","))
            {
                var ingredientName = ToIngredientName(name);
                ingredientNameList.Add(ingredientName);
            }
            var recipe = new Recipe(resultName, ingredientNameList);
            AllRecipes.Add(recipe);
        }

        /// <summary>
        /// Helper for converting to an ingredient.
        /// Might be complexer in the future
        /// </summary>
        /// <param name="txt">the raw string literal</param>
        /// <returns>the converted string</returns>
        private static string ToIngredientName(string txt)
        {
            return txt.Trim();
        }

        /// <summary>
        /// Prints all recipes on start
        /// </summary>
        private void Start()
        {
            print("all recipes:");
            foreach (var recipe in AllRecipes)
            {
                print(recipe.ToString());
            }
        }
    }
}
