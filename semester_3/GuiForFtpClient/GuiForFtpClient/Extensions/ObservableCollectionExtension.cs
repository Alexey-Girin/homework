using SimpleFtpClient;
using System.Collections.ObjectModel;

namespace GuiForFtpClient.Extensions
{
    /// <summary>
    /// Расширение для ObservableCollection.
    /// </summary>
    public static class ObservableCollectionExtension
    {
        /// <summary>
        /// Удаление всех элементов коллекции. Добавление элемента - "переход в родительский каталог".
        /// </summary>
        /// <param name="files">Коллекция элементов.</param>
        public static void Update(this ObservableCollection<FileInfo> files)
        {
            files.Clear();
            files.Add(new FileInfo("...", true));
        }
    }
}
