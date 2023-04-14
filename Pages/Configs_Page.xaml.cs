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
using System.IO.Ports;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;
using ArduinoApp01.Utils;

namespace ArduinoApp01
{
    /// <summary>
    /// Interaction logic for Configs_Page.xaml
    /// </summary>
    public partial class Configs_Page : Window
    {
        

        


        private static string OutPut;
        private static string Input;

        private IConfigrationsSettings settings = null!;
        private string[] Bauds;
        private string[] SDataBits;
        private string[] SStopBits;
        private string[] SParityBits;
        private SerialPort Device;
        private DataHandler dataHandler = new DataHandler();
        public Configs_Page()
        {
            InitializeComponent();
            CHBox_UsingBtn.IsChecked = false;
            CHBox_UsingEnter.IsChecked = false;
             settings = new Settings();
             Bauds = settings.Bauds;
             SDataBits = settings.DataBits;
             SStopBits = settings.StopBits;
             SParityBits = settings.ParityBits;
             Device = settings.Device;
            Device.DataReceived += SerialPort_DataReceived;

        }

        // ==================== On click Event for the Buttons =================
        private void ClosePort_btn_Click(object sender, RoutedEventArgs e)
        {
            if(Device.IsOpen)
            {
                Device.Close();
                ProgressBar_1.Value = 0;
                Label_StatusPort.Foreground = Brushes.Red;
                Label_StatusPort.Content = "Off";
            }
        }

        private void OpenPort_btn_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                Device.PortName = CBox_Ports.Text;
                Device.BaudRate = Convert.ToInt32(CBox_BaudRate.Text);
                Device.Parity = (Parity)Enum.Parse(typeof(Parity),CBox_ParityBits.SelectedItem.ToString());
                Device.StopBits = (StopBits)Enum.Parse(typeof(StopBits),CBox_StopBits.SelectedItem.ToString());
                Device.DataBits = Convert.ToInt32(CBox_DataBits.Text);
                Device.Open();
                ProgressBar_1.Value = 100;
                Label_StatusPort.Foreground = Brushes.Green;
                Label_StatusPort.Content = "On";
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void SendData_Btn_Click(object sender, RoutedEventArgs e)
        {
            if(Device.IsOpen)
            {
                OutPut = TBox_SentDataWindow.Text;

                // New Logic
                if(!CHBox_UsingEnter.IsChecked.Value)
                {
                    if (!CHBox_Write.IsChecked.Value && CHBox_WriteLine.IsChecked.Value)
                    {
                        Device.WriteLine(OutPut);
                    }
                    else if (CHBox_Write.IsChecked.Value && !CHBox_WriteLine.IsChecked.Value)
                    {
                        Device.Write(OutPut);
                    }
                    else
                    {
                        MessageBox.Show("Nothing was Chosen for Sending data .\nPlease chose Write or writeline !", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
              
              
            }

        }

        //===================== Loaded Events ==========================
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {




        }

        // ================== Setting Events on loading the Combo Boxes ============
        private void PopulateComboBox(object sender, string[] values)
        {
            ComboBox comboBox = (ComboBox)sender;
            foreach (string value in values)
            {
                comboBox.Items.Add(value);
            }
        }

        private void CBox_BaudRate_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateComboBox(sender, Bauds);
        }

        private void CBox_DataBits_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateComboBox(sender, SDataBits);
        }

        private void CBox_StopBits_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateComboBox(sender, SStopBits);
        }

        private void CBox_ParityBits_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateComboBox(sender, SParityBits);
        }

        private void CBox_Ports_Loaded(object sender, RoutedEventArgs e)
        {
            string[] Ports = SerialPort.GetPortNames();
            if(Ports.Length > 0)
            {
                PopulateComboBox(sender, Ports);
            }
            
        }

        // ====================== Closing secound Window Event ==============
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(Device.IsOpen)
                
            {
                Device.Close();
                MessageBox.Show("Port is Closed Now", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            MessageBox.Show("No ports are opened \nClosing Program", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
           
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;
        }

        //===================== Clear Data Event ======================
        private void Clear_Screan_Btn_Click(object sender, RoutedEventArgs e)
        {
            if(TBox_SentDataWindow.Text != "")
            {
                TBox_SentDataWindow.Text = "";
            }
            
        }
        //======================= Check Box Events ============================
        private void CHBox_UsingEnter_Checked(object sender, RoutedEventArgs e)
        {
            SendData_Btn.IsEnabled = false;
            CHBox_UsingBtn.IsChecked = false;
        }

        private void CHBox_UsingEnter_Unchecked(object sender, RoutedEventArgs e)
        {
            SendData_Btn.IsEnabled=true;
            CHBox_UsingBtn.IsChecked = true;


        }

        private void TBox_SentDataWindow_KeyDown(object sender, KeyEventArgs e)
        {
            OutPut = TBox_SentDataWindow.Text;
            if (CHBox_UsingEnter.IsChecked.Value)
            {
                if(e.Key == Key.Enter)
                {
                    if (!CHBox_Write.IsChecked.Value && CHBox_WriteLine.IsChecked.Value)
                    {
                        Device.WriteLine(OutPut);
                    }
                    else if (CHBox_Write.IsChecked.Value && !CHBox_WriteLine.IsChecked.Value)
                    {
                        Device.Write(OutPut);
                    }
                    else
                    {
                        MessageBox.Show("Nothing was Chosen for Sending data .\nPlease chose Write or writeline !", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
            }
        }

        private void TBox_SentDataWindow_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(CHBox_UsingEnter.IsChecked.Value)
            {
                TBox_SentDataWindow.Text = TBox_SentDataWindow.Text.Replace(Environment.NewLine, "");
            }
        }

        private void CHBox_UsingBtn_Checked(object sender, RoutedEventArgs e)
        {
            CHBox_UsingEnter.IsChecked = false;
        }

        private void CHBox_UsingBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            CHBox_UsingEnter.IsChecked = true;
        }

        private  void SerialPort_DataReceived(object sender,SerialDataReceivedEventArgs e)
        {


            Task.Run(() =>
            {
                Debug.WriteLine($"Thread is : {Thread.CurrentThread.ManagedThreadId} is the Serial Thread");
                SerialPort serialPort = (SerialPort)sender;
                Input = serialPort.ReadExisting();
                dataHandler.HandelRecivedData(Input);
                TBox_ReceivedData.Dispatcher.Invoke(() =>
                {
                    Debug.WriteLine($"Thread is : {Thread.CurrentThread.ManagedThreadId} is the UI Thread");
                    TBox_ReceivedData.AppendText(Input);
                });

            });
        }

        private void Btn_SaveDatato_txt_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt";
            saveFileDialog.Title = "Save Received Data";
            if(saveFileDialog.ShowDialog() == true)
            {
                dataHandler.SaveToFile(saveFileDialog.FileName);
            }
        }

        private void BtnClear_recevied_Data_Click(object sender, RoutedEventArgs e)
        {
            if(TBox_ReceivedData != null)
            {
                TBox_ReceivedData.Text = "";
            }
        }

        private void Btn_Exit2_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
            MessageBox.Show("Program is Closed", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Btn_GoToPlotter_Click(object sender, RoutedEventArgs e)
        {

            // Close the Port 
            if(Device.IsOpen)
            {
                Device.Close();
                MessageBox.Show("Port is Closed Now", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            // Passing the Device Args to the next Form -> reopen the Port
            Plotter_Page plotterPage = new Plotter_Page(Device.PortName,Device.BaudRate,Device.Parity
                ,Device.StopBits,Device.DataBits);
            this.Visibility = Visibility.Hidden;

            plotterPage.Show();
        }
    }
}
