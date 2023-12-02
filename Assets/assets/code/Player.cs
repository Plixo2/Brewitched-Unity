#nullable enable
using UnityEngine;

namespace assets.code
{
    /// <summary>
    /// Main player movement and interaction script
    /// </summary>
    public class Player : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        private float _currentSpeed = 0;
        private int _jumpCount = 1;

        public GameObject camera;
        private CamFollow camFollow;

        [SerializeField] private Vector2 groundOffset = new Vector2(0, 0);
        [SerializeField] private float groundRadius = 0.2f;
        [SerializeField] private float jumpHeight = 9;
        [SerializeField] private float movementSpeed = 5;
        private bool canMove = true; // Is the player allowed to move or jump
        [SerializeField] private float acceleration = 0.1f;
        [SerializeField] private float reach = 1f;
        [SerializeField] private bool doubleJumpEnabled = false;

        private DelayAction _dropTimer = new();

        [SerializeField] private LayerMask groundMask;

        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            camFollow = camera.GetComponent<CamFollow>();
        }

        private void FixedUpdate()
        {
            if (canMove)
            {
                var moveInput = Input.GetAxisRaw("Horizontal");
                Move(moveInput);
            }
        }

        private void Move(float target)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, target, acceleration);
            var dx = _currentSpeed * movementSpeed;
            // _rigidbody2D.AddForce(new Vector2(dx, 0), ForceMode2D.Impulse);
            _rigidbody2D.velocity = new Vector2(_currentSpeed * movementSpeed, _rigidbody2D.velocity.y);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && canMove)
            {
                if (IsGrounded() || (doubleJumpEnabled && _jumpCount > 0))
                {
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.y, this.jumpHeight);
                    if (doubleJumpEnabled)
                    {
                        _jumpCount--;
                    }
                }
            }

            float velocityX = _rigidbody2D.velocity.x;
            bool playerNotMoving = Mathf.Abs(velocityX) < 0.001f; // Is the player not moving horizontally right now
            bool playerIsGrounded = IsGrounded(); // To not calculate more than once
            if (Input.GetKey(KeyCode.U) && !camFollow.cameraRaised && playerNotMoving && playerIsGrounded)
            {
                camFollow.offset.y += camFollow.cameraRaiseAmount;
                camFollow.cameraRaised = true;
                canMove = false;
            }

            if (Input.GetKey(KeyCode.J) && !camFollow.cameraLowered && playerNotMoving && playerIsGrounded)
            {
                camFollow.offset.y -= camFollow.cameraLowerAmount;
                camFollow.cameraLowered = true;
                canMove = false;
            }


            if (Input.GetKeyUp(KeyCode.U) && camFollow.cameraRaised)
            {
                camFollow.offset.y -= camFollow.cameraRaiseAmount;
                camFollow.cameraRaised = false;
                canMove = true;
            }

            if (Input.GetKeyUp(KeyCode.J) && camFollow.cameraLowered)
            {
                camFollow.offset.y += camFollow.cameraLowerAmount;
                camFollow.cameraLowered = false;
                canMove = true;
            }

            if (IsGrounded() && doubleJumpEnabled)
            {
                _jumpCount = 1;
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

        public void EnableDoubleJump()
        {
            this.doubleJumpEnabled = true;
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
                var result = hand.Interact(this);

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
                if (currentCauldron != null && !handItem.itemName.Equals("cauldron"))
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
                    var connectionPoint = States.GetPoint(transform.position, this.reach, point =>
                        !point.HasItem
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

        /// <summary>
        /// Tests if the player is holding an item
        /// </summary>
        /// <returns>if the player is holding an item</returns>
        private bool HasHandItem()
        {
            return GetHandItem() != null;
        }

        /// <summary>
        /// Pick up an item, and set its properties
        /// </summary>
        /// <param name="item">item to pick up</param>
        private void PickItem(Item item)
        {
            var handItem = HasHandItem();
            if (!handItem)
            {
                item.transform.parent = this.transform;
                item.Pickup();
            }
        }

        /// <summary>
        /// Drops the current held item on the ground 
        /// </summary>
        private void DropHandItem()
        {
            var handItem = GetHandItem();
            if (handItem != null)
            {
                handItem.Disconnect();
            }
        }

        /// <summary>
        /// removes the current held item from the world
        /// </summary>
        private void DeleteHandItem()
        {
            var handItem = GetHandItem();
            DropHandItem();
            if (handItem != null)
            {
                Destroy(handItem.gameObject);
            }
        }

        /// <summary>
        /// Searches for a item inside the children of the GameObject 
        /// </summary>
        /// <returns>A potential Item</returns>
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

        /// <summary>
        /// Performs a 'Physics2D.OverlapCircle' to test if the player is touching the ground
        /// </summary>
        /// <returns>if the player is grounded</returns>
        private bool IsGrounded()
        {
            var position = this.transform.position;
            var pos = new Vector2(position.x, position.y);
            return Physics2D.OverlapCircle(pos + groundOffset, groundRadius, groundMask);
        }

        /// <summary>
        /// Draws a debug circle to display the ground check, if the GameObject is selected  
        /// </summary>
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
}