#nullable enable
using System.Collections;
using System.Xml.Serialization;
using assets.images.mage2;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace assets.code
{
    /// <summary>
    /// Main player movement and interaction script
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerSound), typeof(PlayerAnimator))]
    public class Player : MonoBehaviour
    {
        [HideInInspector] public Rigidbody2D rigidbody2D;
        private PlayerSound _playerSound;
        private PlayerAnimator _playerAnimator;

        private float _currentSpeed = 0;
        private int _jumpCount = 1;
        private bool _canMove = true;
        private float lastJumpTime = -10;
        private float lastGroundTime = -10;
        private DelayAction _dropTimer = new();
        private bool inFire = false;
        private float fireResistanceTimer = 0.0f;
        [HideInInspector] public Vector3 lastGroundedPosition;
        [HideInInspector] public Vector3 lastFixedPosition;
        [HideInInspector] public Vector3 fixedPosition;

        public GameObject _camera;
        private CamFollow camFollow;
        private SpriteRenderer _spriteRenderer;

        [SerializeField] private Vector2 groundOffset = new(0, 0);
        [SerializeField] private float groundRadius = 0.2f;
        [SerializeField] private bool groundRectangle = true;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float jumpHeight = 9;
        [SerializeField] private float movementSpeed = 5;
        [SerializeField] private float acceleration = 0.1f;
        [SerializeField] private float reach = 1f;
        [SerializeField] private bool doubleJumpEnabled = false;
        [SerializeField] private bool fireResistanceEnabled = false;
        [SerializeField] private float fireResistanceDuration = 20.0f;
        [SerializeField] private float fireDeathTimer = 5.0f;
        [SerializeField] private float jumpDelay = 0.2f;
        [SerializeField] private float jumpBuffer = 0.2f;
        [SerializeField] private float coyoteTime = 0.2f;

        void Start()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            _playerSound = GetComponent<PlayerSound>();
            _playerAnimator = GetComponent<PlayerAnimator>();

            camFollow = _camera.GetComponent<CamFollow>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            lastGroundedPosition = transform.position;

        }

        private void FixedUpdate()
        {
            if (_canMove)
            {
                var moveInput = Input.GetAxisRaw("Horizontal");
                Move(moveInput);
            }

            lastFixedPosition = new Vector3(fixedPosition.x, fixedPosition.y, fixedPosition.z);
            fixedPosition = transform.position;
        }

        private void Move(float target)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, target, acceleration);
            // var dx = _currentSpeed * movementSpeed;
            // _rigidbody2D.AddForce(new Vector2(dx, 0), ForceMode2D.Impulse);
            rigidbody2D.velocity = new Vector2(_currentSpeed * movementSpeed, rigidbody2D.velocity.y);
        }

        void Update()
        {
            var waterManager = States.GetWaterManager();
            if (waterManager != null && waterManager.GetCurrentWaterLevel() > this.transform.position.y)
            {
                this.Kill();
            }

            {
                var scaleX = this.gameObject.transform.localScale.x;
                var target = _currentSpeed < 0 ? -1 : 1;
                this.gameObject.transform.localScale =
                    new Vector3(Mathf.Lerp(scaleX, target, Time.deltaTime * 16), 1, 1);
            }
            var isGrounded = IsGrounded();
            if (isGrounded)
            {
                lastGroundedPosition = transform.position;
                lastGroundTime = Time.time;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                lastJumpTime = Time.time;
            }

            var lastJumpDelta = Time.time - lastJumpTime;
            var isJumping = lastJumpDelta <= jumpBuffer;
            var lastGroundDelta = Time.time - lastGroundTime;
            var isCoyote = lastGroundDelta <= coyoteTime;
            if (isJumping && _canMove)
            {
                if (isCoyote || (doubleJumpEnabled && _jumpCount > 0))
                {
                    lastGroundTime = 0;
                    lastJumpTime = 0;
                    Jump();
                }
            }

            float velocityX = rigidbody2D.velocity.x;
            bool playerMoving = Mathf.Abs(velocityX) > 0.1f;
            if (playerMoving && isGrounded)
            {
                _playerSound.PlayStep(Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.U) && !camFollow.cameraRaised && !playerMoving)
            {
                camFollow.offset.y += camFollow.cameraRaiseAmount;
                camFollow.cameraRaised = true;
                _canMove = false;
            }

            if (Input.GetKey(KeyCode.J) && !camFollow.cameraLowered && !playerMoving)
            {
                camFollow.offset.y -= camFollow.cameraLowerAmount;
                camFollow.cameraLowered = true;
                _canMove = false;
            }


            if (Input.GetKeyUp(KeyCode.U) && camFollow.cameraRaised)
            {
                camFollow.offset.y -= camFollow.cameraRaiseAmount;
                camFollow.cameraRaised = false;
                _canMove = true;
            }

            if (Input.GetKeyUp(KeyCode.J) && camFollow.cameraLowered)
            {
                camFollow.offset.y += camFollow.cameraLowerAmount;
                camFollow.cameraLowered = false;
                _canMove = true;
            }

            if (isGrounded && doubleJumpEnabled)
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
                    _playerSound.PlayDrop();
                    DropHandItem();
                }
            }

            if (fireResistanceEnabled)
            {
                fireResistanceTimer -= Time.deltaTime;

                if (fireResistanceTimer <= 0.0f)
                {
                    fireResistanceEnabled = false;
                }
            }

            if (inFire && !fireResistanceEnabled)
            {
                fireDeathTimer -= Time.deltaTime;
                if (fireDeathTimer <= 0.0f)
                {
                    this.Kill();
                }
            }
        }

        public void Jump()
        {
            _playerAnimator.OnJump();
            StartCoroutine(JumpInX(jumpDelay));

            IEnumerator JumpInX(float secs)
            {
                yield return new WaitForSeconds(secs);
                _playerSound.PlayJump();
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.y, jumpHeight);

                if (doubleJumpEnabled)
                {
                    _jumpCount--;
                }
            }
        }


        public void EnableDoubleJump()
        {
            this.doubleJumpEnabled = true;
        }
        public void EnableFireResistance()
        {
            this.fireResistanceEnabled = true;
            fireResistanceTimer = fireResistanceDuration;
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
                        _playerSound.PlayPick();
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
                    _playerSound.PlayBottle();
                    DeleteHandItem();
                    return;
                }
            }

            // if (HasHandItem())
            // {
            //     var handItem = GetHandItem()!;
            //
            //     var currentCauldron = States.CurrentCauldron();
            //     if (currentCauldron != null && !handItem.itemName.Equals("cauldron"))
            //     {
            //         if (Vector3.Distance(currentCauldron.transform.position, transform.position) < reach)
            //         {
            //             _playerSound.PlayBottle();
            //             currentCauldron.Add(handItem);
            //             DeleteHandItem();
            //         }
            //     }
            // }
        }

        /// <summary>
        /// Primary Interaction for picking/deposition items
        /// and placing them in the cauldron
        /// </summary>
        private void InteractPrimary()
        {
            var position = transform.position;
            if (HasHandItem())
            {
                var handItem = GetHandItem()!;

                var connectionPoint = States.GetPoint(position, this.reach, point =>
                    !point.HasItem());
                if (connectionPoint != null && handItem.canConnect())
                {
                    _playerSound.PlayDrop();
                    DropHandItem();
                    handItem.Connect(connectionPoint);
                }
            }
            else
            {
                var freeItemNotCauldron = States.GetItem(position, reach, item1 => !item1.IsConnected()
                    && !item1.isCauldron());
                var freeItemCauldron = States.GetItem(position, reach, item1 => !item1.IsConnected()
                    && item1.isCauldron());
                var connectedItem =
                    States.GetItem(position, reach, item1 => item1.IsConnected());
                if (freeItemNotCauldron != null)
                {
                    PickItem(freeItemNotCauldron);
                }
                else if (freeItemCauldron != null)
                {
                    PickItem(freeItemCauldron);
                }
                else if (connectedItem != null)
                {
                    connectedItem.Disconnect();
                    PickItem(connectedItem);
                }

                _playerSound.PlayPick();
            }
        }

        /// <summary>
        /// Tests if the player is holding an item
        /// </summary>
        /// <returns>if the player is holding an item</returns>
        public bool HasHandItem()
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
        public Item? GetHandItem()
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

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("WaterBubble"))
            {
                this.Kill();
            }
            else if (other.gameObject.CompareTag("DeadlyFire"))
            {
                inFire = true;
            }
        }

        public void Kill()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Performs a 'Physics2D.OverlapCircle' to test if the player is touching the ground
        /// </summary>
        /// <returns>if the player is grounded</returns>
        public bool IsGrounded()
        {
            var position = this.transform.position;
            var pos = new Vector2(position.x, position.y);
            if (groundRectangle)
            {
                return Physics2D.OverlapBox(pos + groundOffset, new Vector2(groundRadius, 0.05f), 0,
                    groundMask);
            }
            else
            {
                return Physics2D.OverlapCircle(pos + groundOffset, groundRadius, groundMask);
            }
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

            if (groundRectangle)
            {
                Gizmos.DrawCube(this.transform.position + down,
                    new Vector2(groundRadius, 0.05f));
            }
            else
            {
                Gizmos.DrawSphere(this.transform.position + down,
                    groundRadius);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position,
                reach);
        }
    }
}