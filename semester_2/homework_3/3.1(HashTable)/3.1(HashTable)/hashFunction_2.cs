namespace HashTableNamespace
{
    using System;

    /// <summary>
    /// Хеш-функция для хеш-таблицы.
    /// </summary>
    public class HashFunction_2 : IHashFunction
    {
        /// <summary>
        /// Хеширование по ключу. 
        /// </summary>
        /// <param name="value">Ключ.</param>
        /// <param name="size">Размер.</param>
        /// <returns>Хеш.</returns>
        public uint Hashing(string value, uint size)
        {
            uint hash = 0;
            int num = 17;

            for (int i = 0; i < value.Length; i++)
            {
                hash += value[i] * (uint)(Math.Pow(num, i));
            }

            return hash % size;
        }
    }
}
