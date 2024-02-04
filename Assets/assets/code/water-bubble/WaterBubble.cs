using System.Collections;
using assets.code;
using UnityEngine;

/// <summary>
///     Handles Movement and Effects for a Dangerous Water Bubble
/// </summary>
public class WaterBubble : MonoBehaviour
{
    [SerializeField] private float bubbleRiseVelocity = 1.0f; // Base Bubble Upward Rise Velocity

    [SerializeField]
    private float
        waterSpeedFactorOnBubbleSpeed = 0.6f; // Factor to scale water speed's effect on bubble speed

    [SerializeField] private float bubbleVelocityMax = 2.5f;

    [SerializeField]
    private int
        currentLevelMaxY =
            23; // Highest tile coordinate (ceiling) of the current level for the bubble to hit and be destroyed

    private Animator animator;
    private float timeSinceSpawned;


    private void Start()
    {
        animator = GetComponent<Animator>();
        var waterSpeed = GameObject.Find("water").GetComponent<WaterAsset>().getCurrentSpeed();
        var clampMin = (int)(bubbleRiseVelocity / waterSpeedFactorOnBubbleSpeed);
        var clampMax = (int)((bubbleVelocityMax - bubbleRiseVelocity) / waterSpeedFactorOnBubbleSpeed);
        waterSpeed =
            Mathf.Clamp(waterSpeed, clampMin * -1,
                clampMax); // Adjusted so that the bubble speed is never below 0 or above bubbleVelocityMax
        bubbleRiseVelocity += waterSpeed * waterSpeedFactorOnBubbleSpeed;
    }

    private void Update()
    {
        timeSinceSpawned += Time.deltaTime;
        if (timeSinceSpawned > 1f) animator.SetBool("TransitionFromSpawnToRise", true);
        var newPosition = transform.position;
        newPosition.y += bubbleRiseVelocity * Time.deltaTime;
        transform.position = newPosition;
        if (newPosition.y + transform.localScale.y / 2 > currentLevelMaxY) destroyBubble();
    }


    /// <summary>
    ///     Destroy the Bubble, can later add more effects like splash sound, animation etc.
    /// </summary>
    public void destroyBubble()
    {
        animator.SetBool("CurrentlyGettingDestroyed", true);
        GetComponent<CircleCollider2D>().enabled = false;
        bubbleRiseVelocity /= 2f;
        StartCoroutine(waitForAnimation());
    }

    private IEnumerator waitForAnimation()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}