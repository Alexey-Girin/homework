namespace _5thHomework.Task1
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Класс, реализующий функции.
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Функция Map.
        /// </summary>
        /// <param name="list">Принимаемый список.</param>
        /// <param name="func">Принимаемая функция.</param>
        /// <returns>Список, полученный применением принимаемой функции к элементам принимаемого списка.</returns>
        public static List<int> Map(List<int> list, Func<int, int> func)
        {
            var resultList = new List<int>();

            foreach(var element in list)
            {
                resultList.Add(func(element));
            }

            return resultList;
        }

        /// <summary>
        /// Функция Filter.
        /// </summary>
        /// <param name="list">Принимаемый список.</param>
        /// <param name="func">Принимаемая функция.</param>
        /// <returns>Список из элементов принимаемого списка, элементы которого удовлетворяют условию функции.</returns>
        public static List<int> Filter(List<int> list, Func<int, bool> func)
        {
            var resultList = new List<int>();

            foreach (var element in list)
            {
                if (func(element))
                {
                    resultList.Add(element);
                }
            }

            return resultList;
        }

        /// <summary>
        /// Функция Fold.
        /// </summary>
        /// <param name="list">Принимаемый список.</param>
        /// <param name="initial">Начальное значение.</param>
        /// <param name="func">Принимаемая функция.</param>
        /// <returns>Накопленное значение.</returns>
        public static int Fold(List<int> list, int initial, Func<int, int, int> func)
        {
            foreach (int element in list)
            {
                initial = func(initial, element);
            }

            return initial;
        }
    }
}
