namespace FirefliesSpawn
{
    public class SpawnTimer
    {
        public float TimeToNextSpawn { get; private set; }

        public void Tick(float deltaTime)
        {
            TimeToNextSpawn -= deltaTime;
        }

        public void Set(float time)
        {
            TimeToNextSpawn = time;
        }
    }
}