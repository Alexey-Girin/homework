using System;
using System.IO;
using System.Text;

namespace MD5
{
    public class Checker
    {
        private string commonHash;

        public string Check(string path)
        {
            commonHash = Path.GetDirectoryName(path);

            string[] fileNames;

            try
            {
                fileNames = Directory.GetDirectories(path);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            Array.Sort(fileNames);

            foreach (var fileName in fileNames)
            {
                commonHash += CheckHash(fileName);
            }

            string hash = CalculateHash(commonHash);

            return commonHash;
        }

        private string CheckHash(string path)
        {
            if (IsDir(path))
            {
                var checker = new Checker();
                return checker.Check(path);
            }

            byte[] content = null;

            using (FileStream fileStream = File.OpenRead(path))
            {
                content = new byte[fileStream.Length];
                fileStream.Read(content, 0, content.Length);
            }

            return CalculateHash(content);
        }

        private string CalculateHash(byte[] content)
        {
            byte[] hashBytes = null;

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                hashBytes = md5.ComputeHash(content);
            }

            return Encoding.Default.GetString(hashBytes);
        }

        private string CalculateHash(string contentString)
        {
            byte[] content = Encoding.Default.GetBytes(contentString);

            return CalculateHash(content);
        }

        private bool IsDir(string path) => Directory.Exists(path);
    }
}