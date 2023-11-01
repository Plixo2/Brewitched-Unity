#nullable enable
using System;
using JetBrains.Annotations;
using UnityEngine;

namespace assets.code
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] public float interactionRange = 0; 
        public void Start()
        {
            States.AddInteractable(this);
            Debug.Log("registered");
        }

        //returns true, if the holding item is used, and should be destroyed
        //item can be null ofc
        public virtual bool Interact(Item? item)
        {
            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1,1,1,0.5f);
            Gizmos.DrawWireSphere(this.transform.position,interactionRange);
        }
    }
}