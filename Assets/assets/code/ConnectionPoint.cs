#nullable enable
using UnityEngine;

namespace assets.code
{
    /// <summary>
    /// Connection point for adding or picking up items
    /// a connected point is added as a child of the GameObject 
    /// </summary>
    public class ConnectionPoint : MonoBehaviour
    {
        /// <summary>
        /// Used for debugging a selected point
        /// </summary>
        [HideInInspector] public bool isSelected = false;

        [SerializeField] public bool isFireplace = false;
        
        /// <summary>
        /// Debug point
        /// </summary>
        private Point? img;

        private void Start()
        {
            States.AddConnectionPoint(this);
            img = GetComponent<Point>();
        }

        /// <summary>
        /// Updates the color of the debug point if the point is selected
        /// used for debug purposes 
        /// </summary>
        private void Update()
        {
            if (img != null)
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

        /// <summary>
        /// test if the point got an item
        /// </summary>
        /// <returns>if the point has an item</returns>
        public bool HasItem()
        {
            return GetHandItem() != null;
        }

        /// <summary>
        /// finds the Item script in the children of the GameObject
        /// </summary>
        /// <returns></returns>
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
    }
}