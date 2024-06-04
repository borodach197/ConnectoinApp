using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionApp
{
    #region Создание файла хранения логина/пароля
    public class CredentialFileManager
    {
        private static string filePath;

        #region Создание файла по указанному пути

        static CredentialFileManager()
        {
            // Получение пути к директории текущего пользователя
            //string currentUserDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            //fileName = "credentials.txt";
            // Формирование полного пути к файлу в этой директории
            filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "credentials.txt");

            // Формирование полного пути к файлу в директории по умолчанию
            // string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //filePath = Path.Combine(currentDirectory, fileName);

        }
        #endregion

        #region Запись данных в файл
        public static void WriteCredentials(string username, string password)
        {
            try
            {
                // Запись учетных данных в файл
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(username);
                    writer.WriteLine(password);
                }

                // Установка прав доступа только для текущего пользователя
                // File.SetAccessControl(filePath, new FileSecurity(filePath, AccessControlSections.Access));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write credentials to file.\nError: {ex.Message}");
            }
        }
        #endregion

        #region Чтение данных из файла
        public static (string, string) ReadCredentials()
        {
            try
            {
                // Чтение учетных данных из файла
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string username = reader.ReadLine();
                    string password = reader.ReadLine();
                    return (username, password);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to read credentials from file.\nError: {ex.Message}");
                return (null, null);
            }
        }
        #endregion

    }
    #endregion
}
