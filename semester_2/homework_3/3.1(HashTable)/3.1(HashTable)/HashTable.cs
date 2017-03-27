namespace HashTableNamespace 
{
    using System;

    /// <summary>
    /// Класс хеш-таблица.
    /// </summary>
    public class HashTable
    {
        private uint size = 32;
        private IHashFunction hashFunction;
        public List[] mas;
        
        public HashTable(IHashFunction newHashFunction)
        {
            hashFunction = newHashFunction;
            mas = new List[size];

            for (int i = 0; i < size; i++)
            {
                mas[i] = new List();
            }
        }

        /// <summary>
        /// Добавление значения в хеш-таблицу.
        /// </summary>
        /// <param name="value">Добавляемое значение.</param>
        public void Add(string value)
        {
            mas[hashFunction.Hashing(value, size)].Add(value);
        }

        /// <summary>
        /// Удаление значения из хеш-таблицы. 
        /// </summary>
        /// <param name="value">Удаляемое значение.</param>
        public void Delete(string value)
        {
            try
            {
                mas[hashFunction.Hashing(value, size)].Delete(value);
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Проверка на принадлежность значения хеш-таблице.
        /// </summary>
        /// <param name="value">Проверяемое значение.</param>
        /// <returns>True если принадлежит.</returns>
        public bool IsBelong(string value)
        {
            return mas[hashFunction.Hashing(value, size)].IsBelong(value);
        }
    }
}
