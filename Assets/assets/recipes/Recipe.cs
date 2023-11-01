using System.Collections.Generic;

namespace assets.recipes
{
    public class Recipe
    {
        public readonly string Result;
        public readonly List<string> Ingredients;

        public Recipe(string result, List<string> ingredients)
        {
            Result = result;
            Ingredients = ingredients;
        }

        public override string ToString()
        {
            return string.Join(",", Ingredients) + " -> " + this.Result;
        }
    }
}