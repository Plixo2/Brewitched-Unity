using UnityEngine;

namespace assets.code
{
    /// <summary>
    /// A Simple Debug Circle Script
    /// if 'drawAlways' is set the circle will always be drawn,
    /// otherwise only if the gameobject is selected 
    /// </summary>
    public class Point : MonoBehaviour
    {
        [SerializeField] public Color color = Color.red;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float size = 1;
        [SerializeField] private bool drawAlways;
        

        private void OnDrawGizmos()
        {
            if (drawAlways)
            {
                Draw();
            }
        }
        private void OnDrawGizmosSelected()
        {
            if (!drawAlways)
            {
                Draw();
            }
        }

        private void Draw()
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position + offset, size);
        }
    
    
    }
}
