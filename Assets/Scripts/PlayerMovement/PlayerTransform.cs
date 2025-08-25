using UnityEngine;

namespace PlayerMovement
{
    public class PlayerTransform
    {
        public Transform Value { get; private set; }

        public PlayerTransform(Transform value)
        {
            Value = value;
        }
    }
}