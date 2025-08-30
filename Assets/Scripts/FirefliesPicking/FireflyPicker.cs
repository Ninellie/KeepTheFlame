using System;
using FirefliesSpawn;

namespace FirefliesPicking
{
    public class FireflyPicker
    {
        public event Action<Firefly> OnFireflyPickup;

        public void PickUp(Firefly firefly)
        {
            OnFireflyPickup?.Invoke(firefly);
        }
    }
}