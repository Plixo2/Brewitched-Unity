using System;
using System.Collections.Generic;
using UnityEngine;

namespace assets.code
{
    /// <summary>
    /// Main Water Manager for timing, water heights
    /// The Instance is registered inside the States class,
    /// can can be retrieved using States.GetWaterManager().
    ///
    /// The `levelHeights` and `levelTimes` lists are used to calculate the water height
    /// and the timing between the levels.
    /// One a new level is reached, the `NextLevel()` function has to be called,
    /// another Script should do that.
    ///
    /// The `currentLevel` variable is used to keep track of the current base level of the water.
    /// An `AnimationCurve` is used to calculate the speed of the water rising.
    /// The `currentLevelTime` variable is used to keep track of the time since the last level change. 
    /// 
    /// This Script also moves itself based on the current water level.
    /// </summary>
    public class WaterAsset : MonoBehaviour
    {
        // Height of the Waters at different levels
        [SerializeField] private List<float> levelHeights = new();

        // Time between the different levels
        [SerializeField] private List<float> levelTimes = new();

        // Current Base Level of the Water
        [SerializeField] private int currentLevel = 0;

        // Curve used to calculate the water level, between the current and next level
        [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

        // Time of the current level
        [SerializeField] private float currentLevelTime = 0;

        [SerializeField] public float timeScale = 1; 

        private void Start()
        {
            States.setWaterManager(this);
        }

        void Update()
        {
            currentLevelTime += Time.deltaTime * timeScale;
            var yLvl = GetCurrentWaterLevel();
            var position = this.transform.position;
            this.transform.position = new Vector3(position.x, yLvl, position.z);
        }

        /// <summary>
        /// Water Level is calculated using the current level and the next level
        /// </summary>
        /// <returns>The World Coordinates Y Level of the Water</returns>
        public float GetCurrentWaterLevel()
        {
            if (currentLevel < 0)
            {
                return levelHeights[0];
            }

            var min = Math.Min(levelHeights.Count, levelTimes.Count);
            if (currentLevel >= min)
            {
                return levelHeights[min];
            }

            var baseHeight = levelHeights[this.currentLevel];
            var nextHeight = levelHeights[this.currentLevel + 1];
            var currentTimespan = levelTimes[this.currentLevel];
            var normalizedTime = currentLevelTime / currentTimespan;

            var waterHeightOffset = nextHeight - baseHeight;
            var curveSample = Mathf.Clamp01(curve.Evaluate(Mathf.Clamp01(normalizedTime)));

            return baseHeight + curveSample * waterHeightOffset;
        }

        /// <summary>
        /// Increases the current level by one and resets the current level time
        /// </summary>
        /// <returns></returns>
        public int NextLevel()
        {
            currentLevelTime = 0;
            return currentLevel += 1;
        }

        /// <summary>
        /// Calculates slope of the water curve plot at call time and returns it to be used as a value proportionate to speed
        /// </summary>
        /// <returns>Current rising speed of the water</returns>
        public float getCurrentSpeed()
        {
            var currentTimespan = levelTimes[this.currentLevel];
            
            var normalizedTime = currentLevelTime / currentTimespan;
            var normalizedTimeDX = (currentLevelTime + Time.deltaTime) / currentTimespan;
            
            var curveSample = Mathf.Clamp01(curve.Evaluate(Mathf.Clamp01(normalizedTime)));
            var curveSampleDX = Mathf.Clamp01(curve.Evaluate(Mathf.Clamp01(normalizedTimeDX)));

            float speed = (curveSampleDX - curveSample) / (normalizedTimeDX - normalizedTime);
            return speed;
        }
    }
}