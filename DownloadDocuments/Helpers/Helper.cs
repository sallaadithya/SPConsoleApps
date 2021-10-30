using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security;

namespace DownloadDocuments.Helpers
{
    public static class Helper
    {
        public static SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            SecureString securePassword = new SecureString();

            foreach (char c in password)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }

        public static string CreateFile(string filePath)
        {
            DirectoryInfo directory = new DirectoryInfo(new FileInfo(filePath).DirectoryName);
            if (!directory.Exists)
            {
                directory.Create();
            }

            return new FileInfo(filePath).FullName;
        }

        public static string SaveFile(string filePath, byte[] bytes)
        {
            CreateFile(filePath);
            File.WriteAllBytes(filePath, bytes);
            return new FileInfo(filePath).FullName;
        }

        public static string SaveFile(string filePath, Stream fileStream)
        {
            BinaryReader br = new BinaryReader(fileStream);
            byte[] bytes = br.ReadBytes((int)fileStream.Length);
            return SaveFile(filePath, bytes);
        }
    }
}
