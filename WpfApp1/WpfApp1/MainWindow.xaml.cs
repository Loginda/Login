using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public object JsonConvert { get; private set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string res = "";
            HttpClient httpClient = new HttpClient();
            string url = string.Format("http://localhost:57359?user={0}&pass={1}",
                                        Uri.EscapeDataString(user.Text),
                                        Uri.EscapeDataString(pass.Password.ToString()));
            var task = httpClient.GetAsync(url)
                                    .ContinueWith((taskWithResponse) =>
                                    {
                                        var response = taskWithResponse.Result;
                                        var jsonString = response.Content.ReadAsStringAsync();
                                        jsonString.Wait();
                                        res = jsonString.Result;
                                    });
            task.Wait();
            TextBox1.Text = res;
        }
    }
}
