using System;
using System.Collections;
using UnityEngine;


/// <summary>
/// Handles Movement and Effects for the Danger Icon that appears before a Dangerous Bubble is about to spawn
/// </summary>
public class DangerIcon : MonoBehaviour
{
    
    private Camera _camera;
    private float cameraPositionY;
    public SpriteRenderer spriteRenderer;
    [SerializeField] public float toggleInterval = 0.1f; // Time Interval for Sprite ON/OFF operation to give a warning to the player
    private float originalY; // y coordinate of the bubble that is about to spawn after this danger icon

    private void Start()
    {
        originalY = this.transform.position.y;
        _camera = Camera.main;
        cameraPositionY = _camera.transform.position.y;
        adjustY();
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
        float distance = Math.Abs(cameraPositionY - originalY);
        Vector3 newPosition = this.transform.position;
        if(distance > 6)
        {
            newPosition.y = cameraPositionY - 5.5f;
        }
        else
        {
            newPosition.y = originalY;
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
