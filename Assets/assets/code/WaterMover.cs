using UnityEngine;

namespace assets.images.water
{
    public class WaterMover : MonoBehaviour
    {

        [SerializeField] private float speed;
        [SerializeField] private float amplitude;
        [SerializeField] private float phase;
        [SerializeField] private float posOffset;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            var position = this.transform.localPosition;
            var pos = Mathf.Sin((Time.time * speed) + phase + (position.x * posOffset)) * amplitude;
            this.transform.localPosition = new Vector3(position.x, pos, position.z);
        }
    }
}
