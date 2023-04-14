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

namespace ArduinoApp01
{
    /// <summary>
    /// Interaction logic for Plotter_Page.xaml
    /// </summary>
    public partial class Plotter_Page : Window
    {

        private SerialPort Device;
        public Plotter_Page(string _PortName,int _BaudRate,Parity _ParityBit,StopBits _StopBit,int _DataBit)
        {
            
           
            InitializeComponent();
            // Recreat a device
            Device = new SerialPort(_PortName, _BaudRate, _ParityBit, _DataBit, _StopBit);
        }

    }
}
