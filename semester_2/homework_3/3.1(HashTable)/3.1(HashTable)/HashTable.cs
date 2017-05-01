namespace HashTableNamespace 
{
    using ListNamespace;

    /// <summary>
    /// Класс хеш-таблица.
    /// </summary>
    public class HashTable
    {
        private uint size = 32;
        private IHashFunction hashFunction;

        /// <summary>
        /// Массив для хеш-таблицы.
        /// </summary>
        public List[] Mas { get; set; }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="HashTable"/>.
        /// </summary>
        /// <param name="newHashFunction"></param>
        public HashTable(IHashFunction newHashFunction)
        {
            hashFunction = newHashFunction;
            Mas = new List[size];

            for (int i = 0; i < size; i++)
            {
                Mas[i] = new List();
            }
        }

        /// <summary>
        /// Добавление элемента в хеш-таблицу.
        /// </summary>
        /// <param name="value">Значение добавляемого элемента.</param>
        public void Add(string value) => 
            Mas[CheckHash(value)].Add(value, 0);

        /// <summary>
        /// Удаление значения из хеш-таблицы. 
        /// </summary>
        /// <param name="value">Удаляемое значение.</param>
        public void Delete(string value) =>
            Mas[CheckHash(value)].Delete(value);

        /// <summary>
        /// Проверка на принадлежность значения хеш-таблице.
        /// </summary>
        /// <param name="value">Проверяемое значение.</param>
        /// <returns>True если принадлежит.</returns>
        public bool IsBelong(string value) =>
            Mas[CheckHash(value)].IsBelong(value);

        /// <summary>
        /// Проверка хеша.
        /// </summary>
        /// <param name="value">Ключ.</param>
        /// <returns>Хеш.</returns>
        private uint CheckHash(string value)
        {
            uint hash = hashFunction.Hashing(value, size);

            if (hash >= size)
            {
                throw new HashFunctionExeption("некорректная работа хеш-функции");
            }

            return hash;
        }
    }
}
