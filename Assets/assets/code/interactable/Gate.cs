#nullable enable
using Unity.VisualScripting;
using UnityEngine;

namespace assets.code.interactable
{
    public class Gate : Interactable
    {
        private Collider2D _collider2D;
        public void Start()
        {
            _collider2D = GetComponent<Collider2D>();
            base.Start();
        }
        public override bool Interact(Item? item)
        {
            if (item != null && item.itemName.Equals("upgrade"))
            {
                _collider2D.enabled = false;
                return true;
            }

            return false;
        }
    }
}