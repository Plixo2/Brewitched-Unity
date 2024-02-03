using System;
using System.Collections;
using UnityEngine;
using assets.code;


/// <summary>
/// Handles Movement and Effects for the Danger Icon that appears before a Dangerous Bubble is about to spawn
/// </summary>
public class DangerIcon : MonoBehaviour
{
    
    private Camera _camera;
    private float cameraPositionY;
    public SpriteRenderer spriteRenderer;
    [SerializeField] public float toggleInterval = 0.6f; // Base Time Interval for Sprite ON/OFF operation to give a warning to the player
    [SerializeField] float waterSpeedFactorOnToggleInterval = 0.3f; // Factor to scale water speed's effect on toggle interval
    [SerializeField] float toggleIntervalMax = 1.0f;
    private GameObject waterObject;

    private void Start()
    {
        waterObject = GameObject.Find("water");
        
        _camera = Camera.main;
        cameraPositionY = _camera.transform.position.y;
        adjustY();

        float waterSpeed = waterObject.GetComponent<WaterAsset>().getCurrentSpeed();
        int clampMin = (int)((toggleInterval - toggleIntervalMax) / waterSpeedFactorOnToggleInterval);
        int clampMax = (int)(toggleInterval / waterSpeedFactorOnToggleInterval);
        waterSpeed = Mathf.Clamp(waterSpeed, clampMin, clampMax); // Adjusted so that the toggle interval is never below 0 or above toggleIntervalMax
        toggleInterval -= waterSpeed * waterSpeedFactorOnToggleInterval;
        StartCoroutine(ToggleSpriteOnOff());
    }

    void Update()
    {
        cameraPositionY = _camera.transform.position.y;
        adjustY();
    }

    
    /// <summary>
    /// Adjust y coordinate of the danger icon so that it shows at the bottom of the screen if it would be out of display otherwise
    /// </summary>
    private void adjustY()
    {
        float waterY = waterObject.transform.position.y;
        float halfHeight = this.transform.localScale.y / 2;
        float distance = Math.Abs(cameraPositionY - (waterY + halfHeight));
        Vector3 newPosition = this.transform.position;
        if(distance > 6)
        {
            newPosition.y = cameraPositionY - 6f + halfHeight - 0.1f;
        }
        else
        {
            newPosition.y = waterY + halfHeight - 0.3f;
        }
        this.transform.position = newPosition;
    }

    IEnumerator ToggleSpriteOnOff()
    {
        while (true)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(toggleInterval);
        }
    }
}
