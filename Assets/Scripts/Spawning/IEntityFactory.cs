namespace Spawning
{
    /// <summary>
    /// Занимается исключительно созданием объектов FirePit и их начальной инициализацией
    /// </summary>
    public interface IEntityFactory
    {
        /// <summary>
        /// Создаёт инстанс объекта в текущей сцене, внедряет все нужные зависимости
        /// и возвращает готовый выключенный объект.
        /// </summary>
        IPooledEntity CreateInstance();
    }
}