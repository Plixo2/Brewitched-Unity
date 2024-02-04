#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace assets.code
{
    /// <summary>
    ///     Main Item Class, to be carried around in the World
    ///     Has to be attached to a 'SpriteRenderer'
    /// </summary>
    public class Item : MonoBehaviour
    {
        public ConnectionPoint? _connectionPoint;

        [SerializeField] public string itemName = "";

        [HideInInspector] public Rigidbody2D rigidbody;

        public SpriteRenderer? _spriteRenderer;
        private readonly List<Collider2D> _collider2Ds = new();
        private Potions potions;

        /// <summary>
        ///     Registers the Item so it can be found.
        ///     Tests if, on start, the parent is a Connection point, so it can properly connect.
        ///     Also Registers the Sprite the the 'itemName' and the
        ///     Sprite found in the attached SpriteRenderer.
        /// </summary>
        private void Start()
        {
            States.AddItem(this);
            rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            foreach (var component in GetComponents<Collider2D>()) _collider2Ds.Add(component);

            var connectionPoint = GetComponentInParent<ConnectionPoint>();
            if (connectionPoint != null) Connect(connectionPoint);

            if (gameObject != null && itemName.Length != 0)
                ImageRegister.RegisterGameObject(itemName, gameObject);
        }

        private void Update()
        {
            transform.localScale = Vector3.one;
        }

        /// <summary>
        ///     Removes the Items from the item list once destroyed
        /// </summary>
        private void OnDestroy()
        {
            States.RemoveItem(this);
        }


        /// <summary>
        ///     Item interaction function for child classes
        /// </summary>
        /// <returns>if the item should be deleted after use</returns>
        public virtual bool Interact(Player player)
        {
            if (itemName.Contains("Potion"))
            {
                potions = GetComponent<Potions>();
                StartCoroutine(potions.EnablePotion());
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Test if a item is connected to a point
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return _connectionPoint != null;
        }

        /// <summary>
        ///     Connects the item to a 'ConnectionPoint'
        /// </summary>
        /// <param name="connectionPoint">the point to connect the item</param>
        public void Connect(ConnectionPoint connectionPoint)
        {
            SetDesiredScale();
            _connectionPoint = connectionPoint;
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            transform.parent = connectionPoint.transform;
            transform.localPosition = new Vector3();
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _spriteRenderer.sortingOrder = 5;
        }

        /// <summary>
        ///     Disconnects the item from a 'ConnectionPoint'
        /// </summary>
        public void Disconnect()
        {
            _connectionPoint = null;
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            transform.parent = null;
            _spriteRenderer.sortingOrder = 20;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            EnableColliders();
        }

        public void Pickup()
        {
            transform.localPosition = new Vector3();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _spriteRenderer.sortingOrder = 20;
            rigidbody.angularVelocity = 0;
            transform.rotation = Quaternion.Euler(0, 0, 45);
            rigidbody.velocity = new Vector2();
            DisableColliders();

            // If the Item is the cauldron change its height so its not in character
            if (name.Equals("cauldron"))
            {
                //    this.transform.position = this.transform.position + new Vector3(0, 1, 0);
            }
        }


        private void DisableColliders()
        {
            _collider2Ds.ForEach(var => var.enabled = false);
        }

        public void EnableColliders()
        {
            _collider2Ds.ForEach(var => var.enabled = true);
        }

        public bool canConnect()
        {
            switch (itemName)
            {
                case "cauldron":
                {
                    return true;
                }
                default:
                {
                    return true;
                }
            }
        }

        private void SetDesiredScale()
        {
            transform.localScale = Vector3.one;
        }

        public bool isCauldron()
        {
            return itemName.Equals("cauldron");
        }
    }
}