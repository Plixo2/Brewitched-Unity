#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using assets.code;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private float _currentSpeed = 0;

    [SerializeField] private Vector2 groundOffset = new Vector2(0, 0);
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private float jumpHeight = 9;
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float acceleration = 0.1f;

    [SerializeField] private float reach = 1f;

    private DelayAction _dropTimer = new DelayAction();


    [SerializeField] private LayerMask groundMask = new LayerMask();

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var moveInput = Input.GetAxisRaw("Horizontal");
        move(moveInput);
    }

    private void move(float target)
    {
        _currentSpeed = Mathf.Lerp(_currentSpeed, target, acceleration);
        var dx = _currentSpeed * movementSpeed;
        // _rigidbody2D.AddForce(new Vector2(dx, 0), ForceMode2D.Impulse);
        _rigidbody2D.velocity = new Vector2(_currentSpeed * movementSpeed, _rigidbody2D.velocity.y);
    }

    void Update()
    {
        if (IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.y, this.jumpHeight);
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            InteractPrimary();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractSecondary();
        }

        if (Input.GetKey(KeyCode.F))
        {
            _dropTimer.Advance(Time.deltaTime);
        }
        else
        {
            _dropTimer.Reset();
        }

        if (_dropTimer.HasJustPassed(0.4f))
        {
            if (HasHandItem())
            {
                DropHandItem();
            }
        }
    }

    /// <summary>
    /// Secondary Interaction for interacting with the World and using items 
    /// </summary>
    private void InteractSecondary()
    {
        var hand = GetHandItem();
        var interactable = States.GetInteractable(transform.position, this.reach, _ => true);
        if (interactable != null)
        {
            var interacted = interactable.Interact(hand);
            if (interacted)
            {
                if (hand != null)
                {
                    DeleteHandItem();
                }

                return;
            }
        }

        if (hand != null)
        {
            var result = hand.Interact();
            if (result)
            {
                DeleteHandItem();
            }
        }
    }

    /// <summary>
    /// Primary Interaction for picking/deposition items
    /// and placing them in the cauldron
    /// </summary>
    private void InteractPrimary()
    {
        if (HasHandItem())
        {
            var handItem = GetHandItem()!;

            var currentCauldron = States.CurrentCauldron();
            var interacted = false;
            if (currentCauldron != null)
            {
                if (Vector3.Distance(currentCauldron.transform.position, transform.position) < reach)
                {
                    currentCauldron.Add(handItem);
                    DeleteHandItem();
                    interacted = true;
                }
            }

            if (!interacted)
            {
                var connectionPoint = States.GetPoint(transform.position, this.reach, point => !point.hasItem
                    ());
                if (connectionPoint != null)
                {
                    DropHandItem();
                    handItem.Connect(connectionPoint);
                    interacted = true;
                }
            }
        }
        else
        {
            var freeItem = States.GetItem(transform.position, this.reach, item1 => !item1.IsConnected());
            if (freeItem != null)
            {
                PickItem(freeItem);
            }
            else
            {
                var connectedItem =
                    States.GetItem(transform.position, this.reach, item1 => item1.IsConnected());
                if (connectedItem != null)
                {
                    connectedItem.Disconnect();
                    PickItem(connectedItem);
                }
            }
        }
    }

    private bool HasHandItem()
    {
        return GetHandItem() != null;
    }

    private void PickItem(Item item)
    {
        var handItem = HasHandItem();
        if (!handItem)
        {
            item.transform.parent = this.transform;
            item.Pickup();
        }
    }

    private void DropHandItem()
    {
        var handItem = GetHandItem();
        if (handItem != null)
        {
            handItem.Disconnect();
        }
    }

    private void DeleteHandItem()
    {
        var handItem = GetHandItem();
        DropHandItem();
        if (handItem != null)
        {
            Destroy(handItem.gameObject);
        }
    }

    private Item? GetHandItem()
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

    private bool IsGrounded()
    {
        var position = this.transform.position;
        var pos = new Vector2(position.x, position.y);
        return Physics2D.OverlapCircle(pos + groundOffset, groundRadius, groundMask);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (IsGrounded())
        {
            Gizmos.color = Color.red;
        }

        var down = new Vector3(groundOffset.x, groundOffset.y, 0);
        Gizmos.DrawSphere(this.transform.position + down,
            groundRadius);
    }
}