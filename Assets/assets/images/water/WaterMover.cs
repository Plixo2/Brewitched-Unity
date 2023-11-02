using UnityEngine;

namespace assets.images.water
{
    public class WaterMover : MonoBehaviour
    {
        private float _yStart = 0;

        [SerializeField] private float speed;
        [SerializeField] private float amplitude;
        [SerializeField] private float phase;
        [SerializeField] private float posOffset;
        // Start is called before the first frame update
        void Start()
        {
            _yStart = this.transform.position.y;
        }

        // Update is called once per frame
        void Update()
        {
            var position = this.transform.position;
            var pos = Mathf.Sin((Time.time * speed) + phase + (position.x * posOffset)) * amplitude;
            this.transform.position = new Vector3(position.x, _yStart + pos, position.z);
        }
    }
}
