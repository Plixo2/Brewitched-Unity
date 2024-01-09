#nullable enable
using System;
using JetBrains.Annotations;
using UnityEngine;

namespace assets.code
{
    /// <summary>
    /// Abstract Behaviour for defining intractables inside the World
    /// The Player will check for them, and can call the
    /// 'Interact' function with the current item
    /// </summary>
    public class Interactable : MonoBehaviour
    {
        /// <summary>
        /// This field extends the players range to
        /// interact with this object
        /// </summary>
        [SerializeField] public float interactionRange = 0; 
        
        /// <summary>
        /// Registers the Interactable so the player can check for it
        /// </summary>
        public void Start()
        {
            print("Registering Interactable");
            States.AddInteractable(this);
        }

        /// <summary>
        /// Main interaction function, called by the player
        /// </summary>
        /// <param name="item">the item the player is interacting with</param>
        /// <returns>if the holding item is used, and should be destroyed</returns>
        public virtual bool Interact(Item? item)
        {
            return false;
        }

        /// <summary>
        /// Debug the interaction range, if the GameObject is selected
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1,1,1,0.5f);
            Gizmos.DrawWireSphere(this.transform.position,interactionRange);
        }
    }
}