using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, 0);
    [SerializeField] private float damping = 0.5f;
    [SerializeField] private bool onlyY = false;

    private Vector3 _velocity = Vector3.zero; 
    // Update is called once per frame
    private void FixedUpdate()
    {
        var pos = target.transform.position + offset;
        var currentPos = transform.position;
        if (onlyY)
        {
            pos.x = currentPos.x;
            pos.z = currentPos.z;
        }
        transform.position = Vector3.SmoothDamp(currentPos, pos, ref _velocity, damping);
    }
}
