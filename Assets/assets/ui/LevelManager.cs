using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelManager
{
    private static List<string> levels = new List<string>
    {
        "MainScene",
        "SemihScene",
        "PlixoScene",
        "LarsScene",
        "lvl6"
    };

    private static int currentLevel;

    public static string AdvanceLevel() {
        if (currentLevel < levels.Count - 1)
        {
            currentLevel++;
        }
        return levels[currentLevel];
    }

    public static bool WasLastLevel()
    {
        return currentLevel == levels.Count - 1;
    }

    public static void SetLevel(int level)
    {
        currentLevel = level;
    }

    public static int GetLevelCount()
    {
        return levels.Count;
    }

    public static string GetSelectedSceneName()
    {
        return levels[currentLevel];
    }
}
