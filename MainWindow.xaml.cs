using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AxMSTSCLib;
using MSTSCLib;

namespace ConnectionApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public CredentialFileManager credentialFileManager = new CredentialFileManager("credentials");
        public MainWindow()
        {
            InitializeComponent();

            // Подписка на событие закрытия приложения
            Application.Current.Exit += new ExitEventHandler(OnApplicationExit);

        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            // Удаление файла с учетными данными при закрытии приложения
            CredentialFileManager.DeleteCredentialsFile();
        }

        #region Окно результата
        private void ResultWindow(string status)
        {
            // Создание и отображение окна статуса
            ResultWindow resultWindow = new ResultWindow(status);
            resultWindow.Show();
        }
        #endregion


        #region Кнопка Ping 
        private void buttonPing_Click(object sender, RoutedEventArgs e)
        {
            string PCName = computerNameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(PCName))
            {
                MessageBox.Show("Пожалуйста, введите IP адрес или имя компьютера.");
                return;
            }

            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send(PCName);

                if (reply.Status == IPStatus.Success)
                {
                    string status = $"Пинг успешен для {PCName}";
                    ResultWindow(status);
                }
                else
                {
                    string status = $"Пинг не удался для {PCName}";
                    ResultWindow(status);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении пинга: {ex.Message}");
            }
        }
        #endregion


        #region Кнопка ID сессии
        private void GetSessionInfo_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.ShowSessions();
        }
        #endregion


        #region Подключение по RDP
        private void ConnectRdpButton_Click(object sender, RoutedEventArgs e)
        {
            var (username, password) = CredentialFileManager.ReadCredentials();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please save credentials first.");
                return;
            }

            string remotePC = computerNameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(remotePC))
            {
                MessageBox.Show("Please enter Remote Computer.");
                return;
            }

            RdpConnector.ConnectRdp(remotePC, username, password);
        }
        #endregion


        #region Кнопка подключения по шадоу RDP
        private void ConnectShadowRdpButton_Click(object sender, RoutedEventArgs e)
        {
            //string remotePC = computerNameTextBox.Text.Trim();

            //var (username, password) = CredentialFileManager.ReadCredentials();

            //if (string.IsNullOrWhiteSpace(remotePC))
            //{
            //    MessageBox.Show("Please enter Remote Computer.");
            //    return;
            //}

            //string sessionId = SessionManager.GetSessionId(remotePC, username, password);

            //if (string.IsNullOrWhiteSpace(sessionId))
            //{
            //    MessageBox.Show("Failed to get Session ID.");
            //    return;
            //}

            //RdpConnector rdpManager = new RdpConnector();
            //RdpConnector.ConnectShadowRdp(remotePC, username, password);
        }
        #endregion


        #region Кнопка сохранения учетных данных в файл
        private void SaveCredentialsButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем новое окно для ввода учетных данных
            var inputWindow = new InputCredentialsWindow();

            // Отображаем окно и ждем, пока пользователь введет данные
            if (inputWindow.ShowDialog() == true)
            {
                // Получаем введенные учетные данные из окна
                string username = inputWindow.Username;
                string password = inputWindow.Password;



                // Проверяем, не пустые ли они
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    // Пользователь ввел данные, можно выполнить нужные действия
                    CredentialFileManager.WriteCredentials(username, password);
                }
                else
                {
                    MessageBox.Show("Введите логин и пароль.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        #endregion
    }

}
