using UnityEngine;

namespace assets.code
{
    public class DelayAction
    {
        private float _triggerTime = 0;
        private float _lastTriggerTime = 0;
        
        
        public void Advance(float delta)
        {
            _lastTriggerTime = _triggerTime;
            _triggerTime += delta;
        }

        public bool HasJustPassed(float time)
        {
            return _triggerTime >= time && _lastTriggerTime < time;
        }

        public bool HasPassed(float time)
        {
            return _triggerTime >= time;
        }

        public void Reset()
        {
            _triggerTime = 0;
            _lastTriggerTime = 0;
        }
    }
}