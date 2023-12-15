using UnityEngine;


/// <summary>
/// Handles Movement and Effects for a Dangerous Water Bubble
/// </summary>
public class WaterBubble : MonoBehaviour
{
    
    [SerializeField] float bubbleRiseVelocity = 1.5f; // Bubble Upward Rise Velocity
    [SerializeField] int currentLevelMaxY = 23; // Highest tile coordinate (ceiling) of the current level for the bubble to hit and be destroyed
    

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
