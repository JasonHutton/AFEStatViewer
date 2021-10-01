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
            string inputPath = @"..\..\..\..\char29.sav";

            BinaryReader reader = new BinaryReader(File.OpenRead(inputPath));

            StringBuilder sb = new StringBuilder();
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
            string jsonString = sb.ToString();

            var campaignCompletion = new CampaignCompletion();
            DataContext = campaignCompletion.Frontend;

            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C1|SC-C1M1"]["Easy|Campaign"]);
            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C1|SC-C1M1"]["Hard|Campaign"]);
            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C1|SC-C1M1"]["Insane|Campaign"]);

            campaignCompletion.LoadCampaignMapData(jsonString);

            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C1|SC-C1M1"]["Easy|Campaign"]);
            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C1|SC-C1M1"]["Hard|Campaign"]);
            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C1|SC-C1M1"]["Insane|Campaign"]);

            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C4|SC-C4M3"]["Hard|Campaign"]);
            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C4|SC-C4M1"]["Easy|Campaign"]);
            Debug.WriteLine(campaignCompletion.data["Campaign|SC-C4|SC-C4M3"]["Easy|Campaign"]);

            /*Insane_C1M1.DataContext = campaignCompletion.Frontend.Data_Insane_C1M1;
            Insane_C1M2.DataContext = campaignCompletion.Frontend.Data_Insane_C1M2;
            Insane_C1M3.DataContext = campaignCompletion.Frontend.Data_Insane_C1M3;*/
            


            /*if (campaignCompletion.data["Campaign|SC-C1|SC-C1M1"]["Insane|Campaign"] > 0)
                campaignCompletion.Data_Insane_C1M1 = true;
            else
                campaignCompletion.Data_Insane_C1M1 = false;

            if (campaignCompletion.data["Campaign|SC-C1|SC-C1M2"]["Insane|Campaign"] > 0)
                campaignCompletion.Data_Insane_C1M2 = true;
            else
                campaignCompletion.Data_Insane_C1M2 = false;

            if (campaignCompletion.data["Campaign|SC-C1|SC-C1M3"]["Insane|Campaign"] > 0)
                campaignCompletion.Data_Insane_C1M3 = true;
            else
                campaignCompletion.Data_Insane_C1M3 = false;*/
        }
    }
}
