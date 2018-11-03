using System;

namespace SimpleFTP_Client
{
    /// <summary>
    /// Класс, представляющий информацию о файле или директории.
    /// </summary>
    public class FileInfo : IComparable
    {
        /// <summary>
        /// Имя файла или директории.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Флаг, принимающий значение True для директорий.
        /// </summary>
        public bool IsDirectory { get; }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="FileInfo"/>.
        /// </summary>
        /// <param name="fileName">Имя файла или директории.</param>
        /// <param name="isDir">Флаг, принимающий значение True для директорий.</param>
        public FileInfo(string fileName, bool isDir)
        {
            Name = fileName;
            IsDirectory = isDir;
        }

        /// <summary>
        /// Метод, сравнивающий текущий экземпляр с другим объектом того же типа.
        /// </summary>
        /// <param name="obj">Объект для сравнения с данным экземпляром.</param>
        /// <returns>Значение, указывающее, каков относительный порядок сравниваемых объектов.</returns>
        public int CompareTo(object obj)
            => string.Compare(Name, (obj as FileInfo).Name);
    }
}
