using UnityEngine;

namespace assets.code
{
    /// <summary>
    /// follows the Camera, specified in the 'target' field.
    /// </summary>
    public class CamFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] public Vector3 offset = new Vector3(0, 0, 0);
        [SerializeField] private float damping = 0.5f;
        [SerializeField] private bool onlyY = false;
        [SerializeField] private int groundY;
        [SerializeField] private int ceilingY;
        public bool cameraRaised = false;
        public bool cameraLowered = false;
        [SerializeField] public float cameraRaiseAmount = 0;
        [SerializeField] public float cameraLowerAmount = 0;

        private Vector3 _velocity = Vector3.zero;

        private void FixedUpdate()
        {
            var pos = target.transform.position + offset;
            var currentPos = transform.position;
            
            var posTemp = pos;
            if (posTemp.y - groundY <= 6) // if below ground would be visible
            {
                if (!cameraRaised)
                {
                    pos.y = groundY + 6; // show ground at the lowest tilelevel on screen
                }
                else
                {
                    pos.y = currentPos.y;
                }
            }
            else if (ceilingY - posTemp.y <= 6) // if above ceiling would be visible
            {
                if (!cameraLowered)
                {
                    pos.y = ceilingY - 6; // show ceiling at the highest tilelevel on screen
                }
                else
                {
                    pos.y = currentPos.y;
                }
            }
            
            if (onlyY)
            {
                pos.x = currentPos.x;
                pos.z = currentPos.z;
            }

            transform.position = Vector3.SmoothDamp(currentPos, pos, ref _velocity, damping);
        }
    }
}