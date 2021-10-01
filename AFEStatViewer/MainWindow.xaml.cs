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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using System.Diagnostics;

using NAryDictionary;

namespace AFEStatViewer
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

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string inputPath = Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%\Endeavor\Saved\SaveGames\char.sav");

            if (!File.Exists(inputPath))
            {
                MessageBox.Show(string.Format("Save game not found at: {0}", inputPath));
                return;
            }

            StringBuilder sb = new StringBuilder();
            using (BinaryReader reader = new BinaryReader(File.OpenRead(inputPath)))
            {
                int bytesPerRead = 20;
                byte[] byteArray;
                do
                {
                    byteArray = reader.ReadBytes(bytesPerRead);

                    for (int i = 0; i < byteArray.Count(); i++)
                    {
                        // Offset the byte's value by 1, and use modulo to wrap if it's more than an allowed value.
                        byteArray[i] = (byte)((byteArray[i] + 1) % 127);
                    }

                    // Add the bytes into our output string
                    sb.Append(Encoding.ASCII.GetString(byteArray));
                }
                while (byteArray.Count() > 0);
            }
            string jsonString = sb.ToString();

            var campaignCompletion = new CampaignCompletion();
            DataContext = campaignCompletion.Frontend;

            campaignCompletion.LoadCampaignMapData(jsonString);
        }
    }
}
