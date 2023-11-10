#nullable enable
using Unity.VisualScripting;
using UnityEngine;

namespace assets.code.interactable
{
    public class Gate : Interactable
    {
        [SerializeField] private Sprite openSprite;
        [SerializeField] private string itemName = "key";
        private Collider2D _collider2D;
        public void Start()
        {
            _collider2D = GetComponent<Collider2D>();
            base.Start();
        }
        public override bool Interact(Item? item)
        {
            if (item != null && item.itemName.Equals(itemName))
            {
                Open();
                return true;
            }

            return false;
        }

        private void Open()
        {
            _collider2D.enabled = false;
            GetComponent<SpriteRenderer>().sprite = openSprite;
        }
    }
}