using System;

namespace SimpleFtpClient
{
    /// <summary>
    /// Класс, представляющий информацию о файле или директории.
    /// </summary>
    public class FileInfo : IComparable<FileInfo>
    {
        /// <summary>
        /// Имя файла или директории.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Имя файла или директории.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Флаг, принимающий значение True для директорий.
        /// </summary>
        public bool IsDirectory { get; }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="FileInfo"/>.
        /// </summary>
        /// <param name="path">Путь к файлу или директории.</param>
        /// <param name="isDir">Флаг, принимающий значение True для директорий.</param>
        public FileInfo(string path, bool isDir)
        {
            Path = path;
            IsDirectory = isDir;
            FileName = GetFileName();
        }

        /// <summary>
        /// Метод, сравнивающий текущий экземпляр с другим объектом того же типа.
        /// </summary>
        /// <param name="obj">Объект для сравнения с данным экземпляром.</param>
        /// <returns>Значение, указывающее, каков относительный порядок сравниваемых объектов.</returns>
        public int CompareTo(FileInfo other)
            => string.Compare(Path, other.Path);

        /// <summary>
        /// Получение имени файла или папки из пути.
        /// </summary>
        /// <returns>Имя файла или папки.</returns>
        private string GetFileName()
        {
            if (Path == null)
            {
                return null;
            }

            int index = -1;

            for (int i = 0; i < Path.Length; i++)
            {
                if (Path[i] == '\\')
                {
                    index = i;
                }
            }

            return Path.Substring(index + 1);
        }
    }
}
