using System;
using System.Collections.Generic;
using UnityEngine;

namespace assets.code
{
    public class CustomCursor : MonoBehaviour
    {
        [SerializeField] private Sprite cursorSprite;
        [SerializeField] private Sprite cursorSpriteDragged;

        [SerializeField] public GameObject cursor;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = cursor.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                
                _spriteRenderer.sprite = cursorSpriteDragged;
                //   Cursor.SetCursor(cursorSpriteDragged.texture, Vector2.zero, cursorMode);
            }
            else
            {
                _spriteRenderer.sprite = cursorSprite;
                // Cursor.SetCursor(cursorSprite.texture, Vector2.zero, cursorMode);
            }
        }
        

        public void SetPosition(Vector3 position)
        {
            cursor.transform.position = position;
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
        public void PickItem(Item item)
        {
            var handItem = HasHandItem();
            if (!handItem)
            {
                item.transform.parent = cursor.transform;
                item.Pickup();
            }
        }

        /// <summary>
        /// Drops the current held item on the ground 
        /// </summary>
        public void DropHandItem()
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
        public void DeleteHandItem()
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
            for (int i = 0; i < cursor.transform.childCount; i++)
            {
                var child = cursor.transform.GetChild(i);
                var item = child.gameObject.GetComponent<Item>();
                if (item != null)
                {
                    return item;
                }
            }

            return null;
        }
    }
}