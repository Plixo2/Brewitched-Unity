using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Left : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var component = GetComponent<Rigidbody2D>();
        component.velocity = new Vector2(-5, component.velocity.y);
    }
}
