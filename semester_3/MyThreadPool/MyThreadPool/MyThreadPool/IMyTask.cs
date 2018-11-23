using System;

namespace MyThreadPool
{
    /// <summary>
    /// Представляет задачи, принятые к исполнению.
    /// </summary>
    /// <typeparam name="TResult">Тип результата исполнения задачи.</typeparam>
    public interface IMyTask<TResult>
    {
        /// <summary>
        /// Свойство, которое возвращает true, если задача выполнена.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Возвращает результат выполнения задачи.
        /// </summary>
        TResult Result { get; }

        /// <summary>
        /// Возвращает новую задачу, представляющую вычисление, 
        /// примененное к результату текущей задачи.
        /// </summary>
        /// <typeparam name="TNewResult">Тип результата исполнения новой задачи.</typeparam>
        /// <param name="newFunc">Вычисление, которое представляет новая задача.</param>
        /// <returns>Новая задача, принятая к исполнению.</returns>
        IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newFunc);
    }
}
