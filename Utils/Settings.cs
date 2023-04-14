using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoApp01.Utils
{
    internal class Settings : IConfigrationsSettings
    {
        public string[] Bauds { get; } = new string[] { "2400", "9600", "115200" };
        public string[] DataBits { get; } = new string[] { "5", "6", "7", "8" };
        public string[] StopBits { get; } = new string[] { "One", "Two", "OnePointFive" };
        public string[] ParityBits { get; } = new string[] { "None", "Odd", "Even" };
        public SerialPort Device { get; } = new SerialPort();
    }
}
