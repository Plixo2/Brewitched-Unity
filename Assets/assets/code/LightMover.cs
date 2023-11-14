using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightMover : MonoBehaviour
{
    [SerializeField] private float intensity = 2; 
    [SerializeField] private float frequency = 2; 
    // Start is called before the first frame update
    private Light2D _light2D;
    void Start()
    {
        _light2D = GetComponent<Light2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        _light2D.intensity = 2 + Mathf.Sin(Time.time * frequency) * intensity;
    }
}
