using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{

    [SerializeField] public Color color = Color.red;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float size = 1;
    [SerializeField] private bool drawAlways;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (drawAlways)
        {
            Draw();
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (!drawAlways)
        {
            Draw();
        }
    }

    private void Draw()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position + offset, size);
    }
    
    
}
