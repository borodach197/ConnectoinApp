using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConnectionApp
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow(string result)
        {
            InitializeComponent();
            resultTextBlock.Text = result;
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            // Закрыть окно ResultWindow
            this.Close();
        }
    }
}
