namespace HashTableNamespace 
{
    using ListNamespace;

    /// <summary>
    /// Хеш-таблица.
    /// </summary>
    public class HashTable
    {
        private uint size = 32;

        /// <summary>
        /// Массив из <see cref="List"/>.
        /// </summary>
        private List[] mas;
        
        /// <summary>
        /// Конструктор экземпляра класса <see cref="HashTable"/>.
        /// </summary>
        public HashTable()
        {
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
        public void Add(string value) => mas[Hashing(value)].Add(value, 0);

        /// <summary>
        /// Удаление значения из хеш-таблицы. 
        /// </summary>
        /// <param name="value">Удаляемое значение.</param>
        public void Delete(string value) => mas[Hashing(value)].Delete(value);

        /// <summary>
        /// Проверка на принадлежность значения хеш-таблице.
        /// </summary>
        /// <param name="value">Проверяемое значение.</param>
        /// <returns>True если принадлежит.</returns>
        public bool IsBelong(string value) => mas[Hashing(value)].IsBelong(value);

        /// <summary>
        /// Хеш-функция. 
        /// </summary>
        /// <param name="value">Ключ.</param>
        /// <returns>Хеш.</returns>
        private uint Hashing(string value)
        {
            uint hash = 2139062143;

            for (int i = 0; i < value.Length; i++)
            {
                hash = 37 * hash + value[i];
            }

            return  hash % size;
        }
    }
}
