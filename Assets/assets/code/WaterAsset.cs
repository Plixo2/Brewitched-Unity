using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Serialization;

namespace assets.code
{
    public class WaterAsset : MonoBehaviour
    {
        [SerializeField] private List<float> levelHeights = new();
        [SerializeField] private List<float> levelTimes = new();
        
        [SerializeField] private int currentLevel = 0;
        
        [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField] private float currentLevelTime = 0;
        
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            currentLevelTime += Time.deltaTime;
            var yLvl = GetCurrentWaterLevel();
            var position = this.transform.position;
            this.transform.position = new Vector3(position.x, yLvl, position.z);
        }

        private float GetCurrentWaterLevel()
        {
            if (currentLevel < 0)
            {
                return levelHeights[0];
            }
            var baseHeight = levelHeights[this.currentLevel];
            var nextHeight = levelHeights[this.currentLevel + 1];
            var currentTimespan = levelTimes[this.currentLevel];
            var normalizedTime = currentLevelTime / currentTimespan;
            
            var waterHeightOffset = nextHeight - baseHeight;
            var curveSample = Mathf.Clamp01(curve.Evaluate(Mathf.Clamp01(normalizedTime)));

            return baseHeight + curveSample * waterHeightOffset;
        }

        public int NextLevel()
        {
            currentLevelTime = 0;
            return currentLevel += 1;
        }
    }
}