using System;
using System.Collections;
using System.Collections.Generic;
using assets.code;
using JetBrains.Annotations;
using UnityEngine;

public class Item : MonoBehaviour
{
    [CanBeNull] private ConnectionPoint _connectionPoint = null;

    [SerializeField] public string itemName;

    [HideInInspector] public Rigidbody2D rigidbody;

    private SpriteRenderer _spriteRenderer;
    private List<Collider2D> _collider2Ds = new();

    // Start is called before the first frame update
    void Start()
    {
        States.AddItem(this);
        rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        foreach (var component in GetComponents<Collider2D>())
        {
            _collider2Ds.Add(component);
        }
        
        var connectionPoint = GetComponentInParent<ConnectionPoint>();
        if (connectionPoint != null)
        {
            Connect(connectionPoint);
        }

        if (_spriteRenderer.sprite != null && this.itemName.Length != 0)
        {
            ImageRegister.RegisterSprite(this.itemName, _spriteRenderer.sprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Item interaction function for child classes
    /// </summary>
    /// <returns>if the item should be deleted after use</returns>
    public virtual bool Interact()
    {
        print("using item");
        return true;
    }

    public bool IsConnected()
    {
        return _connectionPoint != null;
    }

    public void Connect(ConnectionPoint connectionPoint)
    {
        this._connectionPoint = connectionPoint;
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        this.transform.parent = connectionPoint.transform;
        this.transform.localPosition = new Vector3();
        this.transform.rotation = Quaternion.Euler(0,0,0);
        _spriteRenderer.sortingOrder = 5;
        DisableColliders();
    }

    public void Disconnect()
    {
        this._connectionPoint = null;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        this.transform.parent = null;
        _spriteRenderer.sortingOrder = 20;
        this.transform.rotation = Quaternion.Euler(0,0,0);
        EnableColliders();
    }

    public void Pickup()
    {
        this.transform.localPosition = new Vector3();
        this.rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _spriteRenderer.sortingOrder = 20;
        this.rigidbody.angularVelocity = 0;
        this.transform.rotation = Quaternion.Euler(0,0,45);
        this.rigidbody.velocity = new Vector2();
        DisableColliders();
    }

    private void OnDestroy()
    {
        States.RemoveItem(this);
    }


    public void UpdateImage()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        var sprite = ImageRegister.GetByItemName(this.itemName);
        _spriteRenderer.sprite = sprite;
    }

    private void DisableColliders()
    {
        _collider2Ds.ForEach(var => var.enabled = false);
    }
    private void EnableColliders()
    {
        _collider2Ds.ForEach(var => var.enabled = true);
    }
}