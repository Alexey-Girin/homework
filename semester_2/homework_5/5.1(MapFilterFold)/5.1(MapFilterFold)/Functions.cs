namespace _5thHomework.Task1
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Класс, реализующий функции.
    /// </summary>
    public class Functions
    {
        /// <summary>
        /// Функция Map.
        /// </summary>
        /// <param name="list">Принимаемый список.</param>
        /// <param name="func">Принимаемая функция.</param>
        /// <returns>Список, полученный применением принимаемой функции к элементам принимаемого списка.</returns>
        public List<int> Map(List<int> list, Func<int, int> func)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = func(list[i]);
            }

            return list;
        }

        /// <summary>
        /// Функция Filter.
        /// </summary>
        /// <param name="list">Принимаемый список.</param>
        /// <param name="func">Принимаемая функция.</param>
        /// <returns>Список из элементов принимаемого списка, элементы которого удовлетворяют условию функции.</returns>
        public List<int> Filter(List<int> list, Func<int, bool> func)
        {
            List<int> resultList = new List<int>();

            foreach (int elem in list)
            {
                if (func(elem))
                {
                    resultList.Add(elem);
                }
            }

            return resultList;
        }

        /// <summary>
        /// Функция Fold/
        /// </summary>
        /// <param name="list">Принимаемый список.</param>
        /// <param name="initial">Начальное значение.</param>
        /// <param name="func">Принимаемая функция.</param>
        /// <returns>Накопленное значение.</returns>
        public int Fold(List<int> list, int initial, Func<int, int, int> func)
        {
            foreach (int elem in list)
            {
                initial = func(initial, elem);
            }

            return initial;
        }
    }
}
