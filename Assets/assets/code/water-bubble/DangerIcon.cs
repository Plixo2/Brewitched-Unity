using System;
using System.Collections;
using assets.code;
using UnityEngine;

/// <summary>
///     Handles Movement and Effects for the Danger Icon that appears before a Dangerous Bubble is about to spawn
/// </summary>
public class DangerIcon : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [SerializeField]
    public float
        toggleInterval =
            0.6f; // Base Time Interval for Sprite ON/OFF operation to give a warning to the player

    [SerializeField]
    private float
        waterSpeedFactorOnToggleInterval = 0.3f; // Factor to scale water speed's effect on toggle interval

    [SerializeField] private float toggleIntervalMax = 1.0f;

    private Camera _camera;
    private float cameraPositionY;
    private GameObject waterObject;

    private void Start()
    {
        waterObject = GameObject.Find("water");

        _camera = Camera.main;
        cameraPositionY = _camera.transform.position.y;
        adjustY();

        var waterSpeed = waterObject.GetComponent<WaterAsset>().getCurrentSpeed();
        var clampMin = (int)((toggleInterval - toggleIntervalMax) / waterSpeedFactorOnToggleInterval);
        var clampMax = (int)(toggleInterval / waterSpeedFactorOnToggleInterval);
        waterSpeed =
            Mathf.Clamp(waterSpeed, clampMin,
                clampMax); // Adjusted so that the toggle interval is never below 0 or above toggleIntervalMax
        toggleInterval -= waterSpeed * waterSpeedFactorOnToggleInterval;
        StartCoroutine(ToggleSpriteOnOff());
    }

    private void Update()
    {
        cameraPositionY = _camera.transform.position.y;
        adjustY();
    }


    /// <summary>
    ///     Adjust y coordinate of the danger icon so that it shows at the bottom of the screen if it would be out of
    ///     display otherwise
    /// </summary>
    private void adjustY()
    {
        var waterY = waterObject.transform.position.y;
        var halfHeight = transform.localScale.y / 2;
        var distance = Math.Abs(cameraPositionY - (waterY + halfHeight));
        var newPosition = transform.position;
        if (distance > 6)
            newPosition.y = cameraPositionY - 6f + halfHeight - 0.1f;
        else
            newPosition.y = waterY + halfHeight - 0.3f;
        transform.position = newPosition;
    }

    private IEnumerator ToggleSpriteOnOff()
    {
        while (true)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(toggleInterval);
        }
    }
}