using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace assets.recipes
{
    public class RecipeRegister : MonoBehaviour
    {
        public static List<Recipe> AllRecipes = new();
        static RecipeRegister()
        {
            // Add("ash, bottle, phoenix_feather -> potion_fire_resistant");   
            Add("ash, water -> potion_frog");   
            Add("water, feather -> potion_fire");   
            Add("potion_fire, potion_frog -> apple");   
            Add("apple, coal -> upgrade");   
        }

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

        private static string ToIngredientName(string txt)
        {
            return txt.Trim();
        }

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
