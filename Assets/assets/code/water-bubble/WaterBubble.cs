using assets.code;
using UnityEngine;


/// <summary>
/// Handles Movement and Effects for a Dangerous Water Bubble
/// </summary>
public class WaterBubble : MonoBehaviour
{
    
    [SerializeField] float bubbleRiseVelocity = 1.0f; // Base Bubble Upward Rise Velocity
    [SerializeField] float waterSpeedFactorOnBubbleSpeed = 0.6f; // Factor to scale water speed's effect on bubble speed
    [SerializeField] float bubbleVelocityMax = 2.5f;
    [SerializeField] int currentLevelMaxY = 23; // Highest tile coordinate (ceiling) of the current level for the bubble to hit and be destroyed
    

    void Start()
    {
        float waterSpeed = GameObject.Find("water").GetComponent<WaterAsset>().getCurrentSpeed();
        int clampMin = (int)(bubbleRiseVelocity / waterSpeedFactorOnBubbleSpeed);
        int clampMax = (int)((bubbleVelocityMax - bubbleRiseVelocity) / waterSpeedFactorOnBubbleSpeed);
        waterSpeed = Mathf.Clamp(waterSpeed, clampMin * -1, clampMax); // Adjusted so that the bubble speed is never below 0 or above bubbleVelocityMax
        bubbleRiseVelocity += waterSpeed * waterSpeedFactorOnBubbleSpeed;
    }
    
    void Update()
    {
        Vector3 newPosition = this.transform.position;
        newPosition.y += bubbleRiseVelocity * Time.deltaTime;
        this.transform.position = newPosition;
        if(newPosition.y + (this.transform.localScale.y/2) > currentLevelMaxY)
        {
            destroyBubble();
        }
    }

    
    /// <summary>
    /// Destroy the Bubble, can later add more effects like splash sound, animation etc.
    /// </summary>
    private void destroyBubble()
    {
        Destroy(this.gameObject);
    }

}
