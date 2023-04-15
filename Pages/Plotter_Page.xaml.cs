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
using OxyPlot;
using OxyPlot.Wpf;
using OxyPlot.Series;

namespace ArduinoApp01
{
    /// <summary>
    /// Interaction logic for Plotter_Page.xaml
    /// </summary>
    public partial class Plotter_Page : Window
    {

        private SerialPort Device;
        private PlotModel model;
        private LineSeries series;
        private string SyncKey = "@Key@";
        private string EndKey = "@End@";
        private byte[] DataBuffer;
        public Plotter_Page(string _PortName,int _BaudRate,Parity _ParityBit,StopBits _StopBit,int _DataBit)
        {
            
           
            InitializeComponent();
            // Recreat a device
            Device = new SerialPort(_PortName, _BaudRate, _ParityBit, _DataBit, _StopBit);
            DataBuffer = new byte[1024];
            Device.DataReceived += SerialPort_DataStreamReceived;
            series = new LineSeries();
            model = new PlotModel();
            model.Series.Add(series);
            PV_Serial.Model = model;
            
        }

        // ====================== Status ===============================
        private void Btn_CheckConeection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Device.Open();
                Label_ConnectionState.Content = "..OK..";
                Label_ConnectionState.Foreground = Brushes.Green;
                Label_SyncState.Content = "..Wait..";
                Label_SyncState.Foreground = Brushes.Yellow;
                Label_DataState.Content = "..No Data..";
                Label_DataState.Foreground = Brushes.Red;

            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Btn_Sync_Click(object sender, RoutedEventArgs e)
        {
            if(Device.IsOpen)
            {
                // then check the sysc key from the terminal -> if so then Labee should be ..ok..


            }
            else
            {
                Label_SyncState.Content = "..No Sync..";
                Label_SyncState.Foreground = Brushes.Red;

            }
        }

        private void Btn_CloseConection_Click(object sender, RoutedEventArgs e)
        {
            if(Device.IsOpen)
            {
                Device.Close();
                Label_ConnectionState.Content = "..OFF..";
                Label_ConnectionState.Foreground = Brushes.Red;
                Label_SyncState.Content = "..No Sync..";
                Label_SyncState.Foreground = Brushes.Red;
            }

        }
       //=======================================================================

        // ====================== data Handling ==================================
        private void SerialPort_DataStreamReceived(object sender,SerialDataReceivedEventArgs e)
        {
         
            
            SerialPort serialport = (SerialPort)sender;
            int byteRead = serialport.Read(DataBuffer,0,DataBuffer.Length);
            string data = Encoding.ASCII.GetString(DataBuffer, 0, DataBuffer.Length);
            int StartIdx = data.IndexOf(SyncKey);
            int EndIdx = data.IndexOf(EndKey, StartIdx);

            if(StartIdx !=-1 && EndIdx != -1)
            {
                
                string Msg =data.Substring(StartIdx + SyncKey.Length,EndIdx - StartIdx -SyncKey.Length);
                string[] values = Msg.Split('@');
                Dispatcher.Invoke(() =>
                {
                    Label_SyncState.Content = "..In Sync..";
                    Label_SyncState.Foreground = Brushes.Green;
                });

                if(values.Length == 2 && double.TryParse(values[0],out double x) && double.TryParse(values[1],out double y))
                {
                    Dispatcher.Invoke(() =>
                    {
                        series.Points.Add(new DataPoint(x, y));
                        UpdatePlot();
                        Label_DataState.Content = "..OK..";
                        Label_DataState.Foreground = Brushes.Green;
                    });
                }
            }
        }
        private void UpdatePlot()
        {
            while(series.Points.Count > 1000)
            {
                series?.Points.RemoveAt(0);
            }
            
            model.InvalidatePlot(true);

        }

    }
}
