namespace HashTableNamespace
{
    /// <summary>
    /// Хеш-функция для хеш-таблицы.
    /// </summary>
    public interface IHashFunction
    {
        /// <summary>
        /// Хеширование по ключу. 
        /// </summary>
        /// <param name="value">Ключ.</param>
        /// <param name="size">Размер.</param>
        /// <returns>Хеш.</returns>
        uint Hashing(string value, uint size);
    }
}
