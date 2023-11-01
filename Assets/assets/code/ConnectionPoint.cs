using System;
using UnityEngine;


public class ConnectionPoint : MonoBehaviour
{
    [HideInInspector] public bool isSelected = false;

    private Rigidbody2D _rigidbody2D;
    
    private Point img; 
    private void Start()
    {
        States.AddConnectionPoint(this);
        img = GetComponent<Point>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isSelected)
        {
            img.color = Color.red;
        }
        else
        {
            img.color = Color.white;
        }
        isSelected = false;  
    }

    public bool hasItem()
    {
        return getHandItem() != null;
    }
    
    private Item? getHandItem()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            var child = this.transform.GetChild(i);
            var item = child.gameObject.GetComponent<Item>();
            if (item != null)
            {
                return item;
            }
        }

        return null;
    }
}