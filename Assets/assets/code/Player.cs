#nullable enable
using System;
using System.Collections;
using System.Xml.Serialization;
using assets.images.mage2;
using JetBrains.Annotations;
using UnityEditor.ShaderGraph;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
        private float _lastHitTime = -10;
        private DelayAction _dropTimer = new();
        private float jesusPotionTimer;
        [HideInInspector] public Vector3 lastGroundedPosition;
        [HideInInspector] public Vector3 lastFixedPosition;
        [HideInInspector] public Vector3 fixedPosition;
        public int health;

        public GameObject _camera;
        private CamFollow camFollow;
        [SerializeField] SpriteRenderer _spriteRenderer;

        [SerializeField] private Vector2 groundOffset = new(0, 0);
        [SerializeField] private float groundRadius = 0.2f;
        [SerializeField] private bool groundRectangle = true;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] public BoxCollider2D waterCollider;
        [SerializeField] private float jumpHeight = 9;
        [SerializeField] private float movementSpeed = 5;
        [SerializeField] private float acceleration = 0.1f;
        [SerializeField] private float reach = 1f;
        [SerializeField] public bool doubleJumpEnabled = false;
        [SerializeField] public bool fireResistanceEnabled = false;
        [SerializeField] public bool jesusPotionEnabled = false;
        [SerializeField] public bool dashPotionEnabled = false;
        [SerializeField] private float fireDeathTimer = 5.0f;
        [SerializeField] private float jumpDelay = 0.2f;
        [SerializeField] private float jumpBuffer = 0.2f;
        [SerializeField] private float coyoteTime = 0.2f;
        [SerializeField] private float damageCooldown = 0.5f;
        [SerializeField] private float impactFrameTime = 0.5f;
        [SerializeField] private Color32 impactFrameColor = Color.red;

        [SerializeField] [Range(0, 16)] private int maxHealth = 6;

        public bool allowDash = true;
        public bool isDashing;
        [SerializeField] private float dashingPower = 10f;
        [SerializeField] private float dashingTime = 0.5f;
        [SerializeField] private bool dashGravity = true;

        [SerializeField] private float valveInteractionDelay = 1.0f;
        private bool canRotateValve = true;


        void Start()
        {
            health = maxHealth;
            rigidbody2D = GetComponent<Rigidbody2D>();
            _playerSound = GetComponent<PlayerSound>();
            _playerAnimator = GetComponent<PlayerAnimator>();
            camFollow = _camera.GetComponent<CamFollow>();
            lastGroundedPosition = transform.position;
        }

        #region Update

        private void FixedUpdate()
        {
            if (isDashing)
            {
                return;
            }

            if (_canMove)
            {
                var target = Input.GetAxisRaw("Horizontal");
                _currentSpeed = Mathf.Lerp(_currentSpeed, target, acceleration);
                // var dx = _currentSpeed * movementSpeed;
                // _rigidbody2D.AddForce(new Vector2(dx, 0), ForceMode2D.Impulse);
                rigidbody2D.velocity = new Vector2(_currentSpeed * movementSpeed, rigidbody2D.velocity.y);
            }

            lastFixedPosition = new Vector3(fixedPosition.x, fixedPosition.y, fixedPosition.z);
            fixedPosition = transform.position;
        }

        void Update()
        {
            // if (isDashing)
            // {
            //     return;
            // }

            var isGrounded = IsGrounded();
            float velocityX = rigidbody2D.velocity.x;
            bool playerMoving = Mathf.Abs(velocityX) > 0.1f;
            HandlePlayerImpactFrame();
            HandleWaterInteraction();
            HandleFlipAnimation();
            HandleJump(isGrounded, playerMoving);
            HandleInput(isGrounded, playerMoving);
            HandleStepSounds(isGrounded, playerMoving);
            
            if(isGrounded)
            {
                allowDash = true;
            }
        }

        private void HandlePlayerImpactFrame()
        {
            var timeSinceLastHit = Time.time - _lastHitTime;
            var normalColor = Color.white;
            var impactColor = impactFrameColor;
            var fade = ((impactFrameTime - Math.Min(timeSinceLastHit, impactFrameTime)) / impactFrameTime);
            var color = Color.Lerp(normalColor, impactColor, fade);
            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = color;
            }
        }

        private void HandleWaterInteraction()
        {
            var waterManager = States.GetWaterManager();
            if (waterManager != null && waterManager.GetCurrentWaterLevel() > this.transform.position.y &&
                !jesusPotionEnabled)
            {
                this.Damage();
            }
        }

        private void HandleFlipAnimation()
        {
            var scaleX = this.gameObject.transform.localScale.x;
            var target = _currentSpeed < 0 ? -1 : 1;
            this.gameObject.transform.localScale =
                new Vector3(Mathf.Lerp(scaleX, target, Time.deltaTime * 16), 1, 1);
        }

        private void HandleStepSounds(bool isGrounded, bool playerMoving)
        {
            if (playerMoving && isGrounded)
            {
                _playerSound.PlayStep(Time.deltaTime);
            }
        }

        private void HandleInput(bool isGrounded, bool playerMoving)
        {  
            if(camFollow.cameraRaised || camFollow.cameraLowered)
            {
                rigidbody2D.velocity = Vector3.zero;
            }
            
            if (Input.GetKey(KeyCode.U) && !camFollow.cameraRaised && !playerMoving && isGrounded)
            {
                camFollow.offset.y += camFollow.cameraRaiseAmount;
                camFollow.cameraRaised = true;
                _canMove = false;
            }

            if (Input.GetKey(KeyCode.J) && !camFollow.cameraLowered && !playerMoving && isGrounded)
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
            
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && allowDash && dashPotionEnabled)
            {
                StartCoroutine(Dash());
                allowDash = false;
            }
        }

        private void HandleJump(bool isGrounded, bool playerMoving)
        {
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
        }

        #endregion

        #region Movement

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
        private IEnumerator Dash()
        {
            _playerAnimator.onDash();
            isDashing = true;
            float originalGravity = rigidbody2D.gravityScale;
            if (!dashGravity)
            {
                rigidbody2D.gravityScale = 0f;
            }

            rigidbody2D.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * dashingPower, 0.0f);
            yield return new WaitForSecondsRealtime(dashingTime);
            rigidbody2D.gravityScale = originalGravity;
            isDashing = false;
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
                return Physics2D.OverlapBox(pos + groundOffset, new Vector2(groundRadius, 0.2f), 0,
                    groundMask);
            }
            else
            {
                return Physics2D.OverlapCircle(pos + groundOffset, groundRadius, groundMask);
            }
        }

        #endregion

        #region Interact

        /// <summary>
        /// Secondary Interaction for interacting with the World and using items 
        /// </summary>
        private void InteractSecondary()
        {
            var hand = GetHandItem();
            var interactable = States.GetInteractable(transform.position, this.reach, _ => true);
            
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

                bool allValvesClosedOnce = States.allValvesClosedOnce;
                if (!interacted && interactable.CompareTag("Valve") && canRotateValve && !allValvesClosedOnce)
                {
                    canRotateValve = false;
                    StartCoroutine(enableValveInteraction());
                    // _playerSound.PlayValve(); // Play Valve Rotation Sound
                    ((Valve)interactable).toggleValve();
                }
            }

        }

        private IEnumerator enableValveInteraction()
        {
            yield return new WaitForSeconds(valveInteractionDelay);
            canRotateValve = true;
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

        #endregion

        #region Items

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
            if (handItem != null && handItem.itemName.Contains("Potion"))
            {
                handItem.GetComponent<Collider2D>().enabled = false;
                handItem.GetComponent<SpriteRenderer>().enabled = false;
                handItem.rigidbody.bodyType = RigidbodyType2D.Static;
                States.RemoveItem(handItem);
            }
            else if (handItem != null)
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

        #endregion

        #region Damage

        public void Damage()
        {
            var deltaTime = Time.time - _lastHitTime;
            if (deltaTime > damageCooldown)
            {
                health -= 1;
                _lastHitTime = Time.time;
                print($"Health :{health}");
            }

            if (health <= 0)
            {
                this.Kill();
                this.health = 0;
            }
        }

        public void Kill()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        #endregion

        #region UnityBuildins

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
                    new Vector2(groundRadius, 0.2f));
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

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("WaterBubble"))
            {
                WaterBubble bubble = other.GetComponent<WaterBubble>();
                if (jesusPotionEnabled)
                {
                    bubble.destroyBubble();
                }
                else
                {
                    this.Damage();
                    bubble.destroyBubble();
                }
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("DeadlyFire") && !fireResistanceEnabled)
            {
                this.Damage();
            }
        }

        #endregion
    }
}