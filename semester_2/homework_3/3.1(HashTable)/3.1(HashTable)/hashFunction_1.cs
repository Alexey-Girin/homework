namespace HashTableNamespace
{
    /// <summary>
    /// Хеш-функция для хеш-таблицы.
    /// </summary>
    public class HashFunction_1 : IHashFunction
    {
        /// <summary>
        /// Хеширование по ключу. 
        /// </summary>
        /// <param name="value">Ключ.</param>
        /// <param name="size">Размер.</param>
        /// <returns>Хеш.</returns>
        public uint Hashing(string value, uint size)
        {
            uint hash = 2139062143;

            for (int i = 0; i < value.Length; i++)
            {
                hash = 37 * hash + value[i];
            }

            return hash % size;
        }
    }
}
